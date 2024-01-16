using Savi_Thrift.Application.DTO.Wallet;
using TicketEase.Domain;

namespace Savi_Thrift.Application.Interfaces.Services
{
    public interface IWalletService
	{
		Task<ApiResponse<bool>> CreateWallet(CreateWalletDto createWalletDto);
		Task<ApiResponse<List<WalletResponseDto>>> GetAllWallets();
		Task<ApiResponse<WalletResponseDto>> GetWalletByPhone(string phone);
		Task<ApiResponse<WalletResponseDto>> FundWallet(FundWalletDto fundWalletDto);
	}
}
