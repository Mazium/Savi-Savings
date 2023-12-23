using System.ComponentModel.DataAnnotations;

namespace Savi_Thrift.Domain.Entities
{
	public class BaseEntity
	{
		[Required]
		public string Id { get; set; } = string.Empty;
		public DateTime CreatedAt { get; set; } = DateTime.Now;
		public DateTime ModifiedAt { get; set; } = DateTime.Now;
		public bool IsDeleted { get; set; }
	}
}
