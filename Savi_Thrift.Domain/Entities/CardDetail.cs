using System.ComponentModel.DataAnnotations.Schema;

namespace Savi_Thrift.Domain.Entities
{
	public class CardDetail : BaseEntity
	{
		public string NameOnCard { get; set; } = string.Empty;
		public string CardNumber { get; set;} = string.Empty;
		public string Expiry {  get; set; } = string.Empty;
		public string CVV { get; set; } = string.Empty;

		[ForeignKey("AppUserId")]
		public string UserId { get; set; } = string.Empty;
	}
}
