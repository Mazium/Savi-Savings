using Microsoft.AspNetCore.Http;

namespace Savi_Thrift.Application.Interfaces.Services
{
    public interface ICloudinaryServices<T> where T : class
    {
        Task<string> UploadImage(string entityId, IFormFile file);
    }
}