
using Savi_Thrift.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Savi_Thrift.Domain.Entities
{
	public class GroupTransaction : BaseEntity
	{
		public TransactionType TransactionType { get; set; }
		public string Description { get; set; } = string.Empty;
		public Decimal Amount { get; set; }
		public string Reference { get; set; } = string.Empty;

		[ForeignKey("GroupId")]
		public string GroupId { get; set; } = string.Empty;

		[ForeignKey("AppUserId")]
		public string UserId {  get; set; } = string.Empty;
	}
}
