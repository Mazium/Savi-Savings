using AutoMapper;
using Microsoft.AspNetCore.Http;
using Savi_Thrift.Application.DTO.Wallet;
using Savi_Thrift.Application.Interfaces.Repositories;
using Savi_Thrift.Application.Interfaces.Services;
using Savi_Thrift.Domain.Entities;
using Savi_Thrift.Domain.Enums;
using Savi_Thrift.Domain;

namespace Savi_Thrift.Application.ServicesImplementation
{
    public class WalletService : IWalletService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public WalletService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
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
					Naration = fundWalletDto.Naration,
					TransactionType = TransactionType.Credit,
					WalletId = wallet.Id				

				};
				await _unitOfWork.WalletFundingRepository.AddAsync(walletFunding);
				await _unitOfWork.SaveChangesAsync();


				var creditResponse = new CreditResponseDto
				{
					WalletNumber = fundWalletDto.WalletNumber,
					Balance = bal,
					Message = "Your account has been credited successfully.",
                };

               	return ApiResponse<CreditResponseDto>.Success(creditResponse, "Wallet funded successfully", StatusCodes.Status200OK);
			}
			catch (Exception e)
			{
				return ApiResponse<CreditResponseDto>.Failed("Failed to fund wallet. ", StatusCodes.Status400BadRequest, new List<string>() { e.InnerException.ToString()});
			}
		}

	}
}
