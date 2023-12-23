using Savi_Thrift.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Savi_Thrift.Domain.Entities
{
	public class Saving : BaseEntity
	{
		public string Title { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public decimal GoalAmount { get; set; }
		public decimal AmountSaved { get; set; }
		public string Purpose { get; set; } = string.Empty;
		public string Avatar { get; set; } = string.Empty;
		public DateTime StartDate { get; set; }
		public DateTime TargetDate { get; set; }
		public decimal AmountToAdd { get; set; }
		public FundFrequency Frequency { get; set; }

		[ForeignKey("WalletId")]
		public string WalletId { get; set; } = string.Empty;
	}
}
