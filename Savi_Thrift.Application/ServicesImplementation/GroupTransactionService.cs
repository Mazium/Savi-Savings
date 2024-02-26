using AutoMapper;
using AutoMapper.Execution;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Savi_Thrift.Application.DTO.GroupTransaction;
using Savi_Thrift.Application.Interfaces.Repositories;
using Savi_Thrift.Application.Interfaces.Services;
using Savi_Thrift.Domain;
using Savi_Thrift.Domain.Entities;
using Savi_Thrift.Domain.Enums;
using System.Collections.Generic;

namespace Savi_Thrift.Application.ServicesImplementation
{
	public class GroupTransactionService : IGroupTransactionService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IBackgroundJobClient _backgroundJobClient;
		public GroupTransactionService(IUnitOfWork unitOfWork, IMapper mapper, IBackgroundJobClient backgroundJobClient)
		{
			_mapper = mapper;
			_unitOfWork = unitOfWork;
			_backgroundJobClient = backgroundJobClient;
		}


		public async Task<ApiResponse<List<GroupTransactionResponseDto>>> AutoFundGroup(string groupId)
		{
			var findGroup = await _unitOfWork.GroupSavingsRepository.FindAsync(x => x.IsDeleted == false && x.GroupStatus == GroupStatus.Ongoing && x.Id == groupId);
			if (findGroup.Count == 0)
			{
				return ApiResponse<List<GroupTransactionResponseDto>>.Failed("This group is not currently running.", StatusCodes.Status404NotFound, new List<string>());
			}
			var group = findGroup.First();

			var contributionAmount = group.ContributionAmount;

			//find all members of the group

			var groupMembers = await _unitOfWork.GroupMembersRepository.FindAsync(x => x.GroupSavingsId == groupId);

			var collected = groupMembers.Where(x => x.HasCollected == true).ToList();
			if (collected.Count == 5)
			{
				return ApiResponse<List<GroupTransactionResponseDto>>.Failed("This group contribution round is complete.", StatusCodes.Status404NotFound, new List<string>());
			}

			var listOfTransactions = new List<GroupTransactionResponseDto>();

			decimal amountCollected = 0;

			GroupSavingsMembers collector = null;

			foreach (var member in groupMembers)
			{
				if (!member.HasCollected)
				{
					collector = member;
					break;
				}
			}
			if (collector == null)
			{
				return ApiResponse<List<GroupTransactionResponseDto>>.Failed("No group collector available.", StatusCodes.Status404NotFound, new List<string>());
			}

			

			foreach (var member in groupMembers)
			{
				var wallets = await _unitOfWork.WalletRepository.FindAsync(x => x.UserId == member.UserId);
				if (wallets.Count < 0)
				{

					AddDefaultingUsers(member.UserId, groupId, collector.UserId, group.ContributionAmount);
				}
				else
				{
					var wallet = wallets.First();
					if (wallet.Balance < contributionAmount)
					{
						AddDefaultingUsers(member.UserId, groupId, collector.UserId, group.ContributionAmount);
					}
					else
					{
						var previousGroupTransactions = await _unitOfWork.GroupTransactionRepository
							.FindAsync(x => x.GroupSavingsId == groupId && x.UserId == member.UserId && x.CreatedAt.Year == group.RunTime.Year
							&& x.CreatedAt.Month == group.RunTime.Month && x.CreatedAt.Day == group.RunTime.Day);
						if (previousGroupTransactions.Count == 0)
						{
							wallet.Balance -= contributionAmount;
							_unitOfWork.WalletRepository.Update(wallet);
							amountCollected += contributionAmount;
							await _unitOfWork.SaveChangesAsync();

							var transaction = new GroupTransactions
							{
								ActionId = "2",
								Amount = contributionAmount,
								GroupSavingsId = groupId,
								UserId = member.UserId,
							};

							await _unitOfWork.GroupTransactionRepository.AddAsync(transaction);
							await _unitOfWork.SaveChangesAsync();

							var transactionDto = _mapper.Map<GroupTransactionResponseDto>(transaction);
							listOfTransactions.Add(transactionDto);
						}

					}
				}
			}
			groupMembers = groupMembers.OrderBy(x => x.Position).ToList();

			if (amountCollected > 0)
			{
				var wallets = await _unitOfWork.WalletRepository.FindAsync(x => x.UserId == collector.UserId);
				if (wallets.Count > 0)
				{
					var wallet = wallets.First();
					wallet.Balance += amountCollected;

					collector.HasCollected = true;
					_unitOfWork.GroupMembersRepository.Update(collector);
					await _unitOfWork.SaveChangesAsync();

					var transaction = new GroupTransactions
					{
						ActionId = "1",
						Amount = amountCollected,
						GroupSavingsId = groupId,
						UserId = collector.UserId,
					};

					await _unitOfWork.GroupTransactionRepository.AddAsync(transaction);
					await _unitOfWork.SaveChangesAsync();
				}
			}


			DateTime runtime = group.RunTime;
			switch (group.Frequency)
			{
				case SavingFrequency.Daily:
					runtime = group.RunTime.AddDays(1);
					break;
				case SavingFrequency.Monthly:
					runtime = group.RunTime.AddMonths(1);
					break;
				case SavingFrequency.Weekly:
					runtime = group.RunTime.AddDays(7);
					break;

			}
			group.RunTime = runtime;
			_unitOfWork.GroupSavingsRepository.Update(group);

			await _unitOfWork.SaveChangesAsync();
			_backgroundJobClient.Schedule<IRecurringGroupJobs>((job) => job.FundNow(group.Id), runtime);

			return ApiResponse<List<GroupTransactionResponseDto>>.Success(listOfTransactions, "Group automatic funding has completed successfully.", StatusCodes.Status200OK);

		}

		private async void AddDefaultingUsers(string userId, string groupId, string collectorId, decimal contributionAmount )
		{

			var defaultingUsers = new DefaultingUser()
			{
				AppUserId = userId,
				GroupSavingId = groupId,
				RecipientUserId = collectorId,
				AmountDefaulted = contributionAmount,
				ActualDebitDate = DateTime.Now,
				DefaultingPaymentStatus = 0,

			};
			await _unitOfWork.SaveChangesAsync();
			await _unitOfWork.DefaultingUserRepository.AddAsync(defaultingUsers);
		}

		public async Task<ApiResponse<GroupTransactionResponseDto>> FundGroup(GroupFundDto groupFundDto)
		{
			try
			{
				var group = await _unitOfWork.GroupSavingsRepository.GetByIdAsync(groupFundDto.GroupSavingsId);
				if (group == null)
				{
					return ApiResponse<GroupTransactionResponseDto>.Failed("Group not found.", StatusCodes.Status401Unauthorized, new List<string>());
				}

				var user = await _unitOfWork.UserRepository.GetByIdAsync(groupFundDto.UserId);
				if (user == null)
				{
					return ApiResponse<GroupTransactionResponseDto>.Failed("Invalid user Id.", StatusCodes.Status401Unauthorized, new List<string>());
				}
				var groupMembers = await _unitOfWork.GroupMembersRepository.FindAsync(x => x.GroupSavingsId == groupFundDto.GroupSavingsId && x.UserId == groupFundDto.UserId);

				if (groupMembers.Count == 0)
				{
					return ApiResponse<GroupTransactionResponseDto>.Failed("User is not a member of this group.", StatusCodes.Status401Unauthorized, new List<string>());
				}

				var wallets = await _unitOfWork.WalletRepository.FindAsync(x => x.UserId == groupFundDto.UserId);
				if (wallets.Count < 0)
				{
					return ApiResponse<GroupTransactionResponseDto>.Failed("Wallet not found for this user.", StatusCodes.Status401Unauthorized, new List<string>());
				}
				var wallet = wallets.First();

				DateTime today = DateTime.Now;
				decimal amount = group.ContributionAmount;
				if (wallet.Balance < amount)
				{
					return ApiResponse<GroupTransactionResponseDto>.Failed("Insufficient funds in wallet. Please top up.", StatusCodes.Status401Unauthorized, new List<string>());
				}

				if (today.Date != group.RunTime.Date)
				{
					return ApiResponse<GroupTransactionResponseDto>.Failed("Unable to fund. Funding date not today.", StatusCodes.Status401Unauthorized, new List<string>());
				}

				if (today.TimeOfDay > group.RunTime.TimeOfDay)
				{
					return ApiResponse<GroupTransactionResponseDto>.Failed("Unable to fund. Funding time has passed.", StatusCodes.Status401Unauthorized, new List<string>());
				}

				var previousGroupTransactions = await _unitOfWork.GroupTransactionRepository.FindAsync(x => x.GroupSavingsId == groupFundDto.GroupSavingsId && x.UserId == groupFundDto.UserId && x.CreatedAt.Date == today.Date);
				if (previousGroupTransactions.Count > 0)
				{
					return ApiResponse<GroupTransactionResponseDto>.Failed("You have already funded for this round.", StatusCodes.Status401Unauthorized, new List<string>());
				}

				wallet.Balance -= amount;
				_unitOfWork.WalletRepository.Update(wallet);
				await _unitOfWork.SaveChangesAsync();

				var transaction = new GroupTransactions
				{
					ActionId = "2",
					Amount = amount,
					GroupSavingsId = groupFundDto.GroupSavingsId,
					UserId = groupFundDto.UserId,
				};

				await _unitOfWork.GroupTransactionRepository.AddAsync(transaction);
				await _unitOfWork.SaveChangesAsync();

				var transactionDto = _mapper.Map<GroupTransactionResponseDto>(transaction);
				return ApiResponse<GroupTransactionResponseDto>.Success(transactionDto, "Group funded successfully.", StatusCodes.Status200OK);

			}
			catch (Exception ex)
			{
				return ApiResponse<GroupTransactionResponseDto>.Failed("Error occurred while funding group.", StatusCodes.Status401Unauthorized, new List<string>() { ex.Message });
			}
		}

		public Task<ApiResponse<List<GroupTransactionResponseDto>>> GetGroupRecentTransaction(string groupId)
		{
			throw new NotImplementedException();
		}
		public async Task<ApiResponse<List<GroupUserTransactionResponseDto>>> GetGroupTransactionsByUserId(string userId)
		{
			try
			{
				var myGroups = await _unitOfWork.GroupMembersRepository.FindAsync(x=>x.UserId == userId);

				if (myGroups.Count==0)
				{
					return ApiResponse<List<GroupUserTransactionResponseDto>>.Failed("User not part of any group", StatusCodes.Status404NotFound, new List<string>() { });
				}
				List <GroupTransactions> groupTransactions = new ();
				
				foreach (var myGroup in myGroups)
				{
					var transactions = await _unitOfWork.GroupTransactionRepository.FindAsync(x => x.GroupSavingsId == myGroup.GroupSavingsId);
					if (transactions.Count > 0)
					{
						foreach(var transaction in transactions)
						{
							groupTransactions.Add (transaction);
						}
					}
				}
				groupTransactions = groupTransactions.OrderByDescending(x => x.CreatedAt).ToList();


				//return ApiResponse<List<GroupUserTransactionResponseDto>>.Failed("Group id does not exist", StatusCodes.Status404NotFound, new List<string>() { });
				var response = _mapper.Map<List<GroupUserTransactionResponseDto>>(groupTransactions);

				foreach (var transaction in response)
				{
					var user = await _unitOfWork.UserRepository.GetByIdAsync(transaction.UserId);
					if (user != null)
					{
						var gname = await _unitOfWork.GroupSavingsRepository.GetByIdAsync(transaction.GroupSavingsId);
						if(gname != null)
						{
							transaction.GroupName = gname.GroupName;
						}
						transaction.Fullname = user.FirstName+" "+user.LastName;
						transaction.Avatar = user.ImageUrl;
					}
				}

				return ApiResponse<List<GroupUserTransactionResponseDto>>.Success(response, "User group transactions retrieved successfully", StatusCodes.Status200OK);
			}
			catch (Exception ex)
			{
				return ApiResponse<List<GroupUserTransactionResponseDto>>.Failed(new List<string>() { ex.Message });
			}


		}
		public async Task<ApiResponse<List<GroupUserTransactionResponseDto>>> GetGroupTransactions(string groupId)
		{
			try
			{
				var group = await _unitOfWork.GroupSavingsRepository.GetByIdAsync(groupId);
				if (group == null)
				{
					return ApiResponse<List<GroupUserTransactionResponseDto>>.Failed("Group id does not exist", StatusCodes.Status404NotFound, new List<string>() { });
				}

				var transactions = await _unitOfWork.GroupTransactionRepository.FindAsync(x => x.IsDeleted == false && x.GroupSavingsId == groupId);

				transactions = transactions.OrderByDescending(x => x.CreatedAt).ToList();

				var response = _mapper.Map<List<GroupUserTransactionResponseDto>>(transactions);

				foreach (var transaction in response)
				{
					var user = await _unitOfWork.UserRepository.GetByIdAsync(transaction.UserId);
					if (user != null)
					{
						transaction.Fullname = user.FirstName +" "+user.LastName;
						transaction.Avatar = user.ImageUrl;
					}
					var gname = await _unitOfWork.GroupSavingsRepository.GetByIdAsync(transaction.GroupSavingsId);
					if (gname != null)
					{
						transaction.GroupName = gname.GroupName;
					}
					var transmem = await _unitOfWork.GroupMembersRepository.FindAsync(x=>x.GroupSavingsId==transaction.GroupSavingsId && x.UserId==transaction.UserId);
					if(transmem.Count>0)
					{
						var trans = transmem.First();
						transaction.Position = trans.Position;
					}
				}

				return ApiResponse<List<GroupUserTransactionResponseDto>>.Success(response, "Group transactions retrieved", StatusCodes.Status200OK);
			}
			catch (Exception ex)
			{
				return ApiResponse<List<GroupUserTransactionResponseDto>>.Failed(new List<string>() { ex.Message });
			}


		}

		public async Task<ApiResponse<decimal>> TotalGroupSavings(string userId)
		{
			try
			{
				var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
				if (user == null)
				{
					return ApiResponse<decimal>.Failed("user does not exist.", StatusCodes.Status404NotFound, new List<string>());
				}
				var transactions = await _unitOfWork.GroupTransactionRepository.FindAsync(u => u.UserId == userId && u.ActionId == "2");
				if (transactions.Count == 0)
				{
					return ApiResponse<decimal>.Success(0, "No transactions", StatusCodes.Status200OK);
				}
				decimal totalSavings = 0;
				foreach (var transaction in transactions)
				{
					totalSavings += transaction.Amount;
				}

				return ApiResponse<decimal>.Success(totalSavings, "Total Transactions Retrieved Successfully", StatusCodes.Status200OK);
			}
			catch (Exception ex)
			{
				return ApiResponse<decimal>.Failed("Error Occurred", StatusCodes.Status404NotFound, new List<string>() { ex.Message });
			}


		}
	}
}
