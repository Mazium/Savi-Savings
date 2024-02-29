

using Microsoft.AspNetCore.Http;

namespace Savi_Thrift.Application.DTO.AppUser
{
	public class UpdatePhotoDto
	{
		public IFormFile ImageUrl { get; set; }
		public string UserId { get; set; }
	}
}
