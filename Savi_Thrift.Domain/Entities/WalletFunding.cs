using Savi_Thrift.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Savi_Thrift.Domain.Entities
{
	public class WalletFunding : BaseEntity
	{
		public decimal FundAmount { get; set; }
		public string Reference { get; set; } = string.Empty;
		public string Naration { get; set; } = string.Empty;
		public TransactionType TransactionType { get; set; }
        public string WalletNumber { get; set; }

        [ForeignKey("WalletId")]
		public string WalletId { get; set; } = string.Empty;
	}
}
