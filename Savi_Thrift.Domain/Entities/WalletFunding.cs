using Savi_Thrift.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savi_Thrift.Domain.Entities
{
	public class WalletFunding : BaseEntity
	{
		public decimal FundAmount { get; set; }
		public string Reference { get; set; } = string.Empty;
		public string Naration { get; set; } = string.Empty;
		public TransactionType TransactionType { get; set; }
		
		[ForeignKey("WalletId")]
		public string WalletId { get; set; } = string.Empty;
	}
}
