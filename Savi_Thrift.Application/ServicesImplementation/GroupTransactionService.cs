using AutoMapper;
using Microsoft.AspNetCore.Http;
using Savi_Thrift.Application.DTO.GroupTransaction;
using Savi_Thrift.Application.Interfaces.Repositories;
using Savi_Thrift.Application.Interfaces.Services;
using Savi_Thrift.Domain;
using Savi_Thrift.Domain.Entities;

namespace Savi_Thrift.Application.ServicesImplementation
{
    public class GroupTransactionService : IGroupTransactionService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public GroupTransactionService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_mapper = mapper;
			_unitOfWork = unitOfWork;
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
				var response = _mapper.Map<List<GroupUserTransactionResponseDto>>(transactions);
				
				foreach (var transaction in response)
				{
					var user = await _unitOfWork.UserRepository.GetByIdAsync(transaction.UserId);
					if(user != null)
					{
						transaction.Firstname = user.FirstName;
						transaction.Avatar = user.ImageUrl;
					}
				}
				
				return ApiResponse<List<GroupUserTransactionResponseDto>>.Success(response, "Group transactions retrieved", StatusCodes.Status200OK);
			}
			catch (Exception ex)
			{
				return ApiResponse<List<GroupUserTransactionResponseDto>>.Failed(new List<string>() { ex.Message });
			}


		}
	}
}
