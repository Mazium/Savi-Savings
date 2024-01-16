using Savi_Thrift.Domain.Enums;

namespace Savi_Thrift.Application.DTO.Group
{
    public class GroupRequestDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Avatar { get; set; }
        public int Schedule { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public bool IsActive { get; set; }
        public decimal ContributionAmount { get; set; }
        public bool IsOpen { get; set; }
        public int MaxNumberOfParticipants { get; set; }
        public DateTime CashoutDate { get; set; }
        public DateTime NextDueDate { get; set; }
        public SavingFrequency FundFrequency { get; set; }
    }
}
