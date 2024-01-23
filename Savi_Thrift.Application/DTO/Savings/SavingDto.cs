using Savi_Thrift.Domain.Enums;

namespace Savi_Thrift.Application.DTO.Savings
{
    public class SavingDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal GoalAmount { get; set; }
        public decimal AmountSaved { get; set; }
        public string Purpose { get; set; }
        public string Avatar { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime TargetDate { get; set; }
        public decimal AmountToAdd { get; set; }
        public SavingFrequency Frequency { get; set; }
    }
}
