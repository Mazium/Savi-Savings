namespace Savi_Thrift.Application.DTO.Saving
{
    public class CreditWalletFromGoalDto
    {
        public string WalletId { get; set; } = string.Empty;
        public decimal GoalAmount { get; set; }
        public string WalletNumber { get; set; } = string.Empty;
    }
}
