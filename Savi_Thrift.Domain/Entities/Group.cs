using Savi_Thrift.Domain.Enums;

namespace Savi_Thrift.Domain.Entities
{
	public class Group : BaseEntity
	{
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public string Avatar { get; set; } = string.Empty;
		public int Schedule {  get; set; }
		public PaymentMethod PaymentMethod { get; set; }
		public bool IsActive { get; set; }
		public decimal ContributionAmount { get; set; }
		public bool IsOpen { get; set; }
		public int MaxNumberOfParticipants { get; set; }
		public DateTime CashoutDate { get; set; }
		public DateTime NextDueDate { get; set; }
		public FundFrequency FundFrequency { get; set; }
		public ICollection<AppUser>? Users { get; set; }
		public ICollection<GroupTransaction>? GroupTransactions { get; set; }
	}
}
