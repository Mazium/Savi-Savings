namespace Savi_Thrift.Application.DTO.Saving
{
    public class FundsPersonalGoalDto
    {
		public string Description { get; set; } = string.Empty;
		public decimal Amount { get; set; }
		public string WalletNumber { get; set; } = string.Empty;
		public string SavingsId { get; set; }
		public string UserId { get; set; }
	}
}
