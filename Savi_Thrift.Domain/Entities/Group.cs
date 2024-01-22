using Savi_Thrift.Domain.Enums;

namespace Savi_Thrift.Domain.Entities
{
	public class Group : BaseEntity
	{
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public string Avatar { get; set; } = string.Empty;
		public PaymentMethod PaymentMethod { get; set; }

		public bool IsActive { get; set; }
		public decimal ContributionAmount { get; set; }
		public decimal EstimatedCollection { get; set; }
		public bool IsOpen { get; set; }
		public int MaxNumberOfParticipants { get; set; }
		public string AvailableSlots { get; set; } = string.Empty.ToString();
		public string Terms { get; set; } = string.Empty.ToString();
		public decimal Fee { get; set; }
		public DateTime StartDate { get; set; }
		public int DurationInMonths { get; set; }
		public DateTime EndDate => StartDate.AddMonths(DurationInMonths);
		public DateTime CashoutDate { get; set; }
		public DateTime NextDueDate { get; set; }
		public SavingFrequency SavingFrequency { get; set; }
		public ICollection<AppUser> Users { get; set; } = new List<AppUser>();
		public ICollection<GroupTransaction> GroupTransactions { get; set; } = new List<GroupTransaction>();

		public void SetAvailableSlots(int maxNumberOfParticipants)
		{
			string aSLots = "";
			for (int i = 1; i <= maxNumberOfParticipants; i++)
			{
				if (aSLots == "")
				{
					aSLots=i.ToString();
				}
				else
				{
					aSLots += ","+i.ToString();
				}
			}
			AvailableSlots = aSLots;
		}
	}
}
