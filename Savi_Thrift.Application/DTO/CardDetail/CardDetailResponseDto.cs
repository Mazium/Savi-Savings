namespace Savi_Thrift.Application.DTO.CardDetail
{
    public class CardDetailResponseDto
    {
        public string NameOnCard { get; set; }
        public string CardNumber { get; set; }
        public string Expiry { get; set; }
        public string CVV { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime ModifiedAt { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; }
    }
}
