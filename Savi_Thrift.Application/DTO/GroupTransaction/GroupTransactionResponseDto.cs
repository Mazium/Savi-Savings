using Savi_Thrift.Domain.Entities;
using Savi_Thrift.Domain.Enums;

namespace Savi_Thrift.Application.DTO.GroupTransaction
{
	public class GroupUserTransactionResponseDto : BaseEntity
	{
		public string UserId { get; set; }
		public string ActionId { get; set; }
		public string GroupSavingsId { get; set; }
		public decimal Amount { get; set; }
		public string GroupName { get; set; }
		public string Position { get; set; }
		public string Fullname { get; set; }

		public string Avatar { get; set; }
	}
}
