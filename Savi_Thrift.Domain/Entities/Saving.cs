using Savi_Thrift.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Savi_Thrift.Domain.Entities
{
	public class Saving : BaseEntity
	{
		public string Title { get; set; } = string.Empty;
		public decimal GoalAmount { get; set; }
        public decimal Balance { get; set; }
		public string Avatar { get; set; } = string.Empty;
		public DateTime StartDate { get; set; }
		public DateTime TargetDate { get; set; }
		public bool IsAutomatic { get; set; }
		public decimal AmountToAdd { get; set; }
		public SavingFrequency Frequency { get; set; }
		public string WalletNumber { get; set; } = string.Empty;
	}
}
