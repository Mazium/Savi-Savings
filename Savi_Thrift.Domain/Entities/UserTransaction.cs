using Savi_Thrift.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Savi_Thrift.Domain.Entities
{
    public class UserTransaction : BaseEntity
    {
        public int ActionId { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string WalletNumber { get; set; } = string.Empty;
        public string SavingsId { get; set; }

        public string UserId { get; set; }
	}
}
