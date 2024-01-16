using Microsoft.AspNetCore.Http;
using Savi_Thrift.Application.DTO;

namespace Savi_Thrift.Application.Interfaces.Services
{
    public interface ICloudinaryServices<T> where T : class
    {
		Task<CloudinaryUploadResponse> UploadImage(IFormFile file);
	}
}