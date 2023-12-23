using Microsoft.AspNetCore.Http;

namespace Savi.Core.IServices
{
    public interface ICloudinaryServices<T> where T : class
    {
        Task<string> UploadImage(string entityId, IFormFile file);
    }
}