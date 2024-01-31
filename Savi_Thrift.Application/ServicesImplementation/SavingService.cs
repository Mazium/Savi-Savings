using AutoMapper;
using Microsoft.AspNetCore.Http;
using Savi_Thrift.Application.DTO.Saving;
using Savi_Thrift.Application.Interfaces.Repositories;
using Savi_Thrift.Application.Interfaces.Services;
using Savi_Thrift.Domain.Entities;
using Savi_Thrift.Domain;
using Savi_Thrift.Application.DTO.Wallet;
using Savi_Thrift.Domain.Enums;
using System;

namespace Savi_Thrift.Application.ServicesImplementation
{
    public class SavingService : ISavingService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
        private readonly IWalletService _walletService;

        public SavingService(IUnitOfWork unitOfWork, IMapper mapper, IWalletService walletService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
            _walletService = walletService;
        }

		public async Task<ApiResponse<GoalResponseDto>> CreateGoal(CreateGoalDto createGoalDto)
		{

			var existingBoard = await _unitOfWork.SavingRepository.GetByIdAsync(createGoalDto.Title);
			if (existingBoard != null)
			{
				return ApiResponse<GoalResponseDto>.Failed("Goal already exists with this title",400, new List<string>());
			}

			try
			{
				var saving = _mapper.Map<Saving>(createGoalDto);
				await _unitOfWork.SavingRepository.AddAsync(saving);
				await _unitOfWork.SaveChangesAsync();

				var reponseDto = _mapper.Map<GoalResponseDto>(saving);
				return ApiResponse<GoalResponseDto>.Success(reponseDto, "Goal Created Successfully",StatusCodes.Status200OK);

			}
			catch (Exception ex)
			{
				return ApiResponse<GoalResponseDto>.Failed("Error occurred while creating a goal", StatusCodes.Status500InternalServerError, new List<string> { ex.Message });
			}
		}

        public async Task<ApiResponse<List<Saving>>> GetListOfAllUserGoals(string UserId)
        {
            
            try
            {
                var listOfTargets = await _unitOfWork.SavingRepository.FindAsync(u => u.Id == UserId);
                if (listOfTargets.Any())
                {
                    return ApiResponse<List<Saving>>.Success(listOfTargets, " Goals Retrived Successfully", StatusCodes.Status200OK);
                    
                }
                return ApiResponse<List<Saving>>.Failed("Goal not found", StatusCodes.Status400BadRequest, new List<string>());
            
            }

            catch (Exception ex)
            {
                return ApiResponse<List<Saving>>.Failed("Error occurred while creating a goal", StatusCodes.Status500InternalServerError, new List<string> { ex.Message });

            }
        }

        public async Task<ApiResponse<List<GoalResponseDto>>> ViewGoals()
		{
			var wallets = await _unitOfWork.SavingRepository.GetAllAsync();

			List<GoalResponseDto> result = new();	
			foreach (var wallet in wallets)
			{
				var reponseDto = _mapper.Map<GoalResponseDto>(wallet);
				result.Add(reponseDto);
			}
			return ApiResponse<List<GoalResponseDto>>.Success(result, "Goals retrieved successfully",StatusCodes.Status200OK); 
		}


        public async Task<ApiResponse<SavingsResponseDto>> CreditPersonalSavings(CreditSavingsDto creditDto)
        {
            try
              {
                // Assuming you have a method to get personal savings by user ID
                var personalSavings = await _unitOfWork.SavingRepository.GetByIdAsync(creditDto.UserId);

                if (personalSavings == null)
                {
                   
                    var savingEntity =  _mapper.Map<Saving>(creditDto);

                    await _unitOfWork.SavingRepository.AddAsync(savingEntity);
                    await _unitOfWork.SaveChangesAsync();
                }

                var response = await _walletService.GetWalletByNumber(creditDto.WalletNumber);

                if (!response.Succeeded)
                {
                    return ApiResponse<SavingsResponseDto>.Failed(response.Message, response.StatusCode, response.Errors);
                }

                var wallet = response.Data;

                if (wallet == null)
                {
                    //wallet is not found
                    return ApiResponse<SavingsResponseDto>.Failed("Wallet not found", StatusCodes.Status404NotFound, new List<string>());
                }

                if (wallet.Balance < creditDto.CreditAmount)
                {
                   
                    return ApiResponse<SavingsResponseDto>.Failed("Insufficient funds in the wallet.", StatusCodes.Status400BadRequest, new List<string>());
                }

                // Debit the wallet
                decimal newWalletBalance = wallet.Balance - creditDto.CreditAmount;
                wallet.Balance = newWalletBalance;
                _unitOfWork.WalletRepository.Update(wallet);
                await _unitOfWork.SaveChangesAsync();

                // Credit personal savings
                personalSavings.Balance += creditDto.CreditAmount;
                _unitOfWork.SavingRepository.Update(personalSavings);
                await _unitOfWork.SaveChangesAsync();

                var responseDto = new SavingsResponseDto
                {
                    UserId = creditDto.UserId,
                    Balance = personalSavings.Balance,
                    Message = "Personal savings credited successfully.",
                };

                return ApiResponse<SavingsResponseDto>.Success(responseDto, "Personal savings credited successfully", StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return ApiResponse<SavingsResponseDto>.Failed("Failed to credit personal savings. ", StatusCodes.Status500InternalServerError, new List<string> { e.InnerException.ToString() });
            }

        }


        public async Task<ApiResponse<GetPersonalSavingsDTO>> GetPersonalSavings(string Id)
        {

            try
            {
                var listOfSavings = await _unitOfWork.SavingRepository.FindAsync(u => u.Id == Id);
                if (!listOfSavings.Any())
                {
                    return ApiResponse<GetPersonalSavingsDTO>.Failed("Saving Not Found", StatusCodes.Status404NotFound, new List<string>());

                }
                var Savings = listOfSavings.First();

                var SavingsDTO = _mapper.Map<GetPersonalSavingsDTO>(Savings);



                return ApiResponse<GetPersonalSavingsDTO>.Success(SavingsDTO, "Saving Retrieved Successfully", StatusCodes.Status200OK);

            }

            catch (Exception ex)
            {
                return ApiResponse<GetPersonalSavingsDTO>.Failed("Error occurred while retrieving savings", StatusCodes.Status500InternalServerError, new List<string> { ex.Message });

            }
        }

        public async Task<ApiResponse<SavingsResponseDto>> WithdrawFundsFromGoalToWallet(CreditWalletFromGoalDto creditDto)
        {
            try
            {
                var goalsSaving = await _unitOfWork.SavingRepository.FindAsync(u => u.WalletId == creditDto.WalletId);
                var goalsSavings = goalsSaving.FirstOrDefault();

                if (goalsSavings == null)
                {
                    return ApiResponse<SavingsResponseDto>.Failed("Goal savings is null.", StatusCodes.Status400BadRequest, new List<string>());
                }

                // Check if there are insufficient funds in the savings goal
                if (goalsSavings.Balance < creditDto.GoalAmount)
                {
                    return ApiResponse<SavingsResponseDto>.Failed("Insufficient funds in the savings goal.", StatusCodes.Status400BadRequest, new List<string>());
                }

                // Calculate new balance and update
                decimal newBalance = goalsSavings.Balance - creditDto.GoalAmount;
                goalsSavings.Balance = newBalance;
                _unitOfWork.SavingRepository.Update(goalsSavings);
                await _unitOfWork.SaveChangesAsync();

                // Check if the user already has a wallet
                var existingWalletDetails = await _unitOfWork.WalletRepository.FindAsync(w => w.UserId == creditDto.WalletId);
                var existingWallet = existingWalletDetails.FirstOrDefault();

                if (existingWallet != null)
                {
                    // Update existing wallet balance
                    existingWallet.Balance += creditDto.GoalAmount;
                    _unitOfWork.WalletRepository.Update(existingWallet);
                }
                else
                {
                    // Create a new wallet entry
                    var wallet = new Wallet
                    {
                        UserId = creditDto.WalletId,
                        Balance = creditDto.GoalAmount
                    };
                    await _unitOfWork.WalletRepository.AddAsync(wallet);
                }

                await _unitOfWork.SaveChangesAsync();

                // Create a user transaction
                var userFund = new UserTransaction
                {
                    ModifiedAt = DateTime.Now,
                    Amount = creditDto.GoalAmount,
                    TransactionType = TransactionType.Debit,
                    UserId = creditDto.WalletNumber // Verify if this should be set to creditDto.WalletId
                };
                await _unitOfWork.UserTransactionRepository.AddAsync(userFund);
                await _unitOfWork.SaveChangesAsync();

                // Prepare and return the response
                var responseDto = new SavingsResponseDto
                {
                    UserId = creditDto.WalletId,
                    Balance = goalsSavings.Balance,
                    Message = "Funds successfully withdrawn from savings goal to wallet.",
                };

                return ApiResponse<SavingsResponseDto>.Success(responseDto, "Funds withdrawn successfully", StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                // Handle exceptions and return an error response
                return ApiResponse<SavingsResponseDto>.Failed("Failed to withdraw funds.", StatusCodes.Status500InternalServerError, new List<string> { e.Message });
            }
        }


        public async Task<ApiResponse<DebitResponseDto>> FundsPersonalGoal(FundsPersonalGoalDto personalGoalDto)
        {
            try
            {
                // Retrieve user's savings goal              
                var walletInfo = await _unitOfWork.WalletRepository.FindAsync(u => u.WalletNumber == personalGoalDto.WalletNumber);
                var wallet = walletInfo.FirstOrDefault();
                //Check if the goalsSavings is null
                if (wallet == null)
                {
                    return ApiResponse<DebitResponseDto>.Failed("Goal savings is null.", StatusCodes.Status400BadRequest, new List<string>());
                }
                // Check if there are sufficient funds in the savings goal
                if (wallet.Balance < personalGoalDto.FundAmount)
                {
                    return ApiResponse<DebitResponseDto>.Failed("Insufficient funds in the savings goal.", StatusCodes.Status400BadRequest, new List<string>());
                }

                decimal newBalance = wallet.Balance - personalGoalDto.FundAmount;
                wallet.Balance = newBalance;
                _unitOfWork.WalletRepository.Update(wallet);
                await _unitOfWork.SaveChangesAsync();

                var walletSavings = new Saving
                {
                    WalletId = personalGoalDto.WalletNumber,
                    Balance = personalGoalDto.FundAmount
                };

                await _unitOfWork.SavingRepository.AddAsync(walletSavings);
                await _unitOfWork.SaveChangesAsync();

                var debitResponse = new DebitResponseDto
                {
                    WalletNumber = personalGoalDto.WalletNumber,
                    Balance = newBalance,
                    Message = "Your wallet has been debited successfully.",
                };
                return ApiResponse<DebitResponseDto>.Success(debitResponse, "Funds withdrawn successfully", StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                // Handle exceptions and return an error response
                return ApiResponse<DebitResponseDto>.Failed("Failed to withdraw funds.", StatusCodes.Status500InternalServerError, new List<string> { e.Message });
            }
        }

     
    }
}
