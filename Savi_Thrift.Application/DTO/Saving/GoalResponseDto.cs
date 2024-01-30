using Savi_Thrift.Domain.Enums;

namespace Savi_Thrift.Application.DTO.Saving
{
    public class GoalResponseDto
    {
        public string Id { get; set; }
        public string Title { get; set; } = string.Empty;
		//public string Description { get; set; } = string.Empty;
		public decimal GoalAmount { get; set; }
		//public decimal AmountSaved { get; set; }
		//public string Purpose { get; set; } = string.Empty;
		public string Avatar { get; set; } = string.Empty;
		public DateTime StartDate { get; set; }
		public DateTime TargetDate { get; set; }
		public decimal AmountToAdd { get; set; }
		public SavingFrequency Frequency { get; set; }
	}
}
