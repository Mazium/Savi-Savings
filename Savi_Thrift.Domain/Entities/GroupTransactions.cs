namespace Savi_Thrift.Domain.Entities
{
    public class GroupTransactions : BaseEntity
    {
        public string UserId { get; set; }
        public string ActionId { get; set; }
        public string GroupSavingsId { get; set; } 
        public decimal Amount { get; set; }

    }
}
