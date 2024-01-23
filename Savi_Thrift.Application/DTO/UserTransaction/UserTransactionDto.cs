using Savi_Thrift.Domain.Enums;

namespace Savi_Thrift.Application.DTO.UserTransaction
{
    public class UserTransactionDto
    {
        public TransactionType TransactionType { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string Reference { get; set; }
    }
}
