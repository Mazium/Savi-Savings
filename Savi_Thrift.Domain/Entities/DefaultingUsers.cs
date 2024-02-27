namespace Savi_Thrift.Domain.Entities
{
    public class DefaultingUsers: BaseEntity
    {
        public string AppUserId { get; set; }
        public string GroupSavingId { get; set; }
        public string ReceipientUserId { get; set; }
        public decimal AmountDebited { get; set; }
        public DateTime ActualDebitdate { get; set; }
        public int DefaultingPaymentStatus { get; set; }

    }
}
