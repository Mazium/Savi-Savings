using Savi_Thrift.Domain.Enums;

namespace Savi_Thrift.Application.DTO.GroupTransaction
{
    public class GroupTransactionResponseDto
    {
        public TransactionType TransactionType { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string Reference { get; set; }
        public string UserId { get; set; }
        public string GroupId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
