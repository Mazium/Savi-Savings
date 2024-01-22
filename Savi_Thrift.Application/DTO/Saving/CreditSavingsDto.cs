namespace Savi_Thrift.Application.DTO.Saving
{
    public class CreditSavingsDto  
    {
        public string UserId { get; set; } = string.Empty;
        public string WalletNumber { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public decimal CreditAmount { get; set; }
    }

    //input data needed for the credit operation.
}
