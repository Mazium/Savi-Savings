using Savi_Thrift.Domain.Enums;

namespace Savi_Thrift.Application.DTO.Saving
{
    public class CreateGoalDto
    {
        public string Title { get; set; } = string.Empty;
        public decimal GoalAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime TargetDate { get; set; }
        public SavingFrequency Frequency { get; set; }
		public decimal AmountToAdd { get; set; }
		public string WalletId { get; set; } = string.Empty;
    }
}
