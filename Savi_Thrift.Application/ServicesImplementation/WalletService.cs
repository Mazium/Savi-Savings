using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Savi_Thrift.Application.DTO;
using Savi_Thrift.Application.DTO.Wallet;
using Savi_Thrift.Application.Interfaces.Repositories;
using Savi_Thrift.Application.Interfaces.Services;
using Savi_Thrift.Domain;
using Savi_Thrift.Domain.Entities;
using Savi_Thrift.Domain.Enums;

namespace Savi_Thrift.Application.ServicesImplementation
{
    public class WalletService : IWalletService
	{
        private readonly ILogger<WalletService> _logger;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
		public WalletService(ILogger<WalletService> logger, IUnitOfWork unitOfWork, IMapper mapper, IConfiguration config)
		{
            _logger = logger;
			_unitOfWork = unitOfWork;
			_mapper = mapper;
            _httpClient = new HttpClient();
            _config = config;
            string secretKey = _config["PaystackApi:SecretKey"];
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {secretKey}");
		}

		public async Task<ApiResponse<bool>> CreateWallet(CreateWalletDto createWalletDto)
		{
			try
			{
				var wallet = _mapper.Map<Wallet>(createWalletDto);
				wallet.SetWalletID(createWalletDto.PhoneNumber);
				wallet.Currency = Currency.Naira;
				wallet.TransactionPin = "1234";

				await _unitOfWork.WalletRepository.AddAsync(wallet);
				await _unitOfWork.SaveChangesAsync();

				var reponseDto = _mapper.Map<WalletResponseDto>(wallet);
				return new ApiResponse<bool>(true, "Wallet Created Successful");

			}
			catch (Exception ex)
			{
				return new ApiResponse<bool>(false, "Failed to create wallet. "+ex);
			}
		}

		public async Task<ApiResponse<List<WalletResponseDto>>> GetAllWallets()
		{
			var wallets = await _unitOfWork.WalletRepository.GetAllAsync();
			List<WalletResponseDto> result = new();	
			foreach (var wallet in wallets)
			{
				var reponseDto = _mapper.Map<WalletResponseDto>(wallet);
				result.Add(reponseDto);
			}
			return new ApiResponse<List<WalletResponseDto>>(result, "Wallet retrieved successfully"); 
		}

		public async Task<ApiResponse<Wallet>> GetWalletByNumber(string phone)
		{
			var wallets = await _unitOfWork.WalletRepository.FindAsync(x => x.WalletNumber == phone);

			if (wallets.Count < 1)
			{
				return ApiResponse<Wallet>.Failed("Wallet with this number not found", StatusCodes.Status404NotFound, new List<string>());
			}

			// Assuming you want to use the details of the first wallet if multiple wallets are found
			var firstWallet = wallets.First();

			return ApiResponse<Wallet>.Success(firstWallet, "Wallet retrieved successfully", StatusCodes.Status200OK);
		}


		public async Task<ApiResponse<CreditResponseDto>> FundWallet(FundWalletDto fundWalletDto)
		{
			try
			{
				var response = await GetWalletByNumber(fundWalletDto.WalletNumber);

				if (!response.Succeeded)
				{
					return ApiResponse<CreditResponseDto>.Failed(response.Message, response.StatusCode, response.Errors);
				}

				var wallet = response.Data;
				decimal bal = wallet.Balance + fundWalletDto.FundAmount;
                wallet.Balance = bal;
				_unitOfWork.WalletRepository.Update(wallet);
                await _unitOfWork.SaveChangesAsync();

                var walletFunding = new WalletFunding
				{
					FundAmount = fundWalletDto.FundAmount,
					Narration = fundWalletDto.Narration,
					TransactionType = TransactionType.Credit,
					WalletId = wallet.Id				

				};
				await _unitOfWork.WalletFundingRepository.AddAsync(walletFunding);
				await _unitOfWork.SaveChangesAsync();


				var creditResponse = new CreditResponseDto
				{
					WalletNumber = fundWalletDto.WalletNumber,
					Balance = bal,
					Message = "Your wallet has been credited successfully.",
                };

               	return ApiResponse<CreditResponseDto>.Success(creditResponse, "Wallet funded successfully", StatusCodes.Status200OK);
			}
			catch (Exception e)
			{
				return ApiResponse<CreditResponseDto>.Failed("Failed to fund wallet. ", StatusCodes.Status400BadRequest, new List<string>() { e.InnerException.ToString()});
			}
		}

        public async Task<ApiResponse<DebitResponseDto>> DebitWallet(DebitWalletDto debitWalletDto)
        {
            try
            {
                var response = await GetWalletByNumber(debitWalletDto.WalletNumber);

                if (!response.Succeeded)
                {
                    return ApiResponse<DebitResponseDto>.Failed(response.Message, response.StatusCode, response.Errors);
                }

                var wallet = response.Data;

                // Check if there is enough balance to perform the debit
                if (wallet.Balance < debitWalletDto.DebitAmount)
                {
                    return ApiResponse<DebitResponseDto>.Failed("Insufficient funds for debit", StatusCodes.Status400BadRequest, new List<string>());
                }

                decimal newBalance = wallet.Balance - debitWalletDto.DebitAmount;
                wallet.Balance = newBalance;
                _unitOfWork.WalletRepository.Update(wallet);
                await _unitOfWork.SaveChangesAsync();

                var walletDebit = new WalletFunding
                {
                    FundAmount = debitWalletDto.DebitAmount,
                    Narration = debitWalletDto.Narration,
                    TransactionType = TransactionType.Debit,
                    WalletId = wallet.Id
                };
                await _unitOfWork.WalletFundingRepository.AddAsync(walletDebit);
                await _unitOfWork.SaveChangesAsync();

                var debitResponse = new DebitResponseDto
                {
                    WalletNumber = debitWalletDto.WalletNumber,
                    Balance = newBalance,
                    Message = "Your wallet has been debited successfully.",
                };

                return ApiResponse<DebitResponseDto>.Success(debitResponse, "Wallet debited successfully", StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return ApiResponse<DebitResponseDto>.Failed("Failed to debit wallet. ", StatusCodes.Status400BadRequest, new List<string>() { e.InnerException?.ToString() });
            }
        }

        public async Task<string> VerifyTransaction(string referenceCode, string userId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(referenceCode))
                {
                    throw new Exception("Please provide a valid reference number");
                }
                HttpResponseMessage response = await _httpClient.GetAsync($"https://api.paystack.co/transaction/verify/{referenceCode}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<PaystackResponse>(content);
                    if (result.Status)
                    {
                        var data = result.Data;
                        if (data.Status == "success")
                        {
                            var amount = data.Amount / 100;
                            var email = data.Customer.Email;
                            var updateWallet = UpdateWallet(userId, amount); //email to be changed to userId, remember blah
                            var walletFunding = new WalletFunding()
                            {
                                FundAmount = amount,
                                Reference = referenceCode,
                                WalletNumber = updateWallet.Result.WalletNumber,
                                WalletId = updateWallet.Result.Id,
                                CumulativeAmount = updateWallet.Result.Balance,
                                TransactionType =TransactionType.Credit,
                            };
                            await _unitOfWork.WalletFundingRepository.AddAsync(walletFunding);
                            await _unitOfWork.SaveChangesAsync();
                            return ($"Payment of {amount} Naira from {email} was successful!");
                        }
                        else
                        {
                            return ($"Payment was not successful. Status: {data.Status}");
                        }
                    }
                    else
                    {
                        return ($"Paystack API returned an error. Message: {result.Message}");
                    }
                }
                else
                {
                    return $"Error: {response.StatusCode} - {response.ReasonPhrase}";
                }
            }
            catch (HttpRequestException ex)
            {
                return $"HttpRequestException: {ex.Message}";

            }
        }

        private ResponseDto<Wallet> UpdateWallet(string userId, decimal amount)
        {
            var wallet = _unitOfWork.WalletRepository.WalletById(userId);
            if (wallet == null)
            {
                return new ResponseDto<Wallet>
                {
                    StatusCode = 400,
                    DisplayMessage = "User does not have a wallet",
                };
            }
            wallet.Balance += amount;
            wallet.ModifiedAt = DateTime.UtcNow;

            _unitOfWork.WalletRepository.Update(wallet);
            _unitOfWork.SaveChangesAsync();
            return new ResponseDto<Wallet>
            {
                StatusCode = 200,
                DisplayMessage = "wallet updated successfully",
                Result = wallet
            };
        }

    }
}
