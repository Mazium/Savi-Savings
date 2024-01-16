using AutoMapper;
using Microsoft.AspNetCore.Http;
using Savi_Thrift.Application.DTO.Wallet;
using Savi_Thrift.Application.Interfaces.Repositories;
using Savi_Thrift.Application.Interfaces.Services;
using Savi_Thrift.Domain.Entities;
using Savi_Thrift.Domain.Enums;
using TicketEase.Domain;

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

		public async Task<ApiResponse<WalletResponseDto>> GetWalletByPhone(string phone)
		{
			var wallets = await _unitOfWork.WalletRepository.FindAsync(x => x.WalletNumber == phone);

			if (wallets.Count < 1)
			{
				return ApiResponse<WalletResponseDto>.Failed("Wallet with this number not found", StatusCodes.Status404NotFound, new List<string>());
			}

			// Assuming you want to use the details of the first wallet if multiple wallets are found
			var firstWallet = wallets.First();

			var response = new WalletResponseDto
			{
				WalletNumber = firstWallet.WalletNumber,
				Currency = firstWallet.Currency,
				Balance = firstWallet.Balance
			};

			return ApiResponse<WalletResponseDto>.Success(response, "Wallet retrieved successfully", StatusCodes.Status200OK);
		}


		public async Task<ApiResponse<WalletResponseDto>> FundWallet(FundWalletDto fundWalletDto)
		{
			try
			{
				var response = await GetWalletByPhone(fundWalletDto.WalletNumber);

				if (!response.Succeeded)
				{
					return ApiResponse<WalletResponseDto>.Failed(response.Message, response.StatusCode, response.Errors);
				}

				var userWallet = response.Data;
				userWallet.Balance += fundWalletDto.FundAmount;
				var wallet = _mapper.Map<Wallet>(userWallet);
				_unitOfWork.WalletRepository.Update(wallet);

				var walletFunding = _mapper.Map<WalletFunding>(fundWalletDto);
				walletFunding.TransactionType = TransactionType.Credit;
				walletFunding.WalletNumber = userWallet.WalletNumber;

				await _unitOfWork.WalletFundingRepository.AddAsync(walletFunding);
				await _unitOfWork.SaveChangesAsync();


				var walletResponse = _mapper.Map<WalletResponseDto>(userWallet);

				return ApiResponse<WalletResponseDto>.Success(walletResponse, "Wallet funded successfully", StatusCodes.Status200OK);
			}
			catch (Exception e)
			{
				return ApiResponse<WalletResponseDto>.Failed("Failed to fund wallet. ", StatusCodes.Status400BadRequest, new List<string>() { e.InnerException.ToString()});
			}
		}

	}
}
