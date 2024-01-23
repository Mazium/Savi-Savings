namespace Savi_Thrift.Application.DTO.Wallet
{
    public class DebitResponseDto
    {
        public string WalletNumber { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
