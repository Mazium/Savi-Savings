using Microsoft.AspNetCore.Http;

namespace Savi_Thrift.Application.DTO.Group
{
    public class UpdateGroupPhotoDto
    {
        public IFormFile PhotoFile { get; set; }
    }
}
