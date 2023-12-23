using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Savi_Thrift.Application.Interfaces.Repositories;
using Savi_Thrift.Application.Interfaces.Services;

namespace Savi_Thrift.Application
{
    public class CloudinaryServices<T> : ICloudinaryServices<T> where T : class
        {
            private readonly IGenericRepository<T> _repository;
            public CloudinaryServices(IGenericRepository<T> repository)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));

            }

            public async Task<string> UploadImage(string entityId, IFormFile file)
            {
                var entity = _repository.GetByIdAsync(entityId);

                if (entity == null)
                {
                    return $"{typeof(T).Name} not found";
                }

                var cloudinary = new Cloudinary(new Account(
                "dxaadkzlr",
                "261756745827284",
                "SGiYD5_wyF2WIEYSWT2S7b0fNLQ"
                ));

                var upload = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, file.OpenReadStream())
                };
                var uploadResult = await cloudinary.UploadAsync(upload);
            _repository.UpdateAsync(entity);
                try
                {
                    _repository.SaveChangesAsync();
                    return uploadResult.SecureUrl.AbsoluteUri;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex}");
                    return "Database update error occurred";
                }
            }
        }

    }

