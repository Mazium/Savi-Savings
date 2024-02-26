namespace Savi_Thrift.Domain.Entities
{
	public class DefaultingUser: BaseEntity
	{
        public string AppUserId { get; set; }
        public string GroupSavingId { get; set; }
        public string RecipientUserId { get; set; }
        public decimal AmountDefaulted { get; set; }
        public DateTime ActualDebitDate { get; set; }
        public int DefaultingPaymentStatus{ get; set; }
    }
}
