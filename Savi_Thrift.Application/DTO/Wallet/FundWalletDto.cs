

namespace Savi_Thrift.Application.DTO.Wallet
{
	public class FundWalletDto
	{
		public string WalletNumber { get; set; }
		public decimal FundAmount { get; set; }
		public string Naration { get; set; } = string.Empty;
	}
}
