using Savi_Thrift.Domain.Entities;
using Savi_Thrift.Domain.Enums;

namespace Savi_Thrift.Application.DTO.GroupTransaction
{
    public class GroupTransactionResponseDto : BaseEntity
    {
		public string UserId { get; set; }
		public string ActionId { get; set; }
		public string GroupSavingsId { get; set; }
		public decimal Amount { get; set; }
	}
}
