using Savi_Thrift.Application.DTO.Wallet;
using Savi_Thrift.Domain.Entities;
using Savi_Thrift.Domain;

namespace Savi_Thrift.Application.Interfaces.Services
{
    public interface IWalletService
	{
		Task<ApiResponse<bool>> CreateWallet(CreateWalletDto createWalletDto);
		Task<ApiResponse<List<WalletResponseDto>>> GetAllWallets();
        Task<ApiResponse<Wallet>> GetWalletByNumber(string phone);
		Task<ApiResponse<CreditResponseDto>> FundWallet(FundWalletDto fundWalletDto);
		Task<ApiResponse<DebitResponseDto>> DebitWallet(DebitWalletDto debitWalletDto);
        Task<string> VerifyTransaction(string referenceCode, string userId);
		Task<ApiResponse<Wallet>> GetWalletByUserId(string userId);
        Task<ApiResponse<decimal>> GetTotalWalletBalance(string userId);






    }
}
