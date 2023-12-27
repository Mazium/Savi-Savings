using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Savi_Thrift.Application.DTO;
using Savi_Thrift.Application.Interfaces.Repositories;
using Savi_Thrift.Application.Interfaces.Services;
using Savi_Thrift.Common.Utilities;
using Savi_Thrift.Domain.Entities.Helper;

namespace Savi_Thrift.Application
{
    public class CloudinaryServices : ICloudinaryServices
    {
            private readonly Cloudinary _cloudinary;
            private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration = ConfigurationHelper.GetConfigurationInstance();
            private readonly CloudinaryConfig cloudinaryOptions = new CloudinaryConfig();

         public CloudinaryServices()
            {
              _configuration.Bind(cloudinaryOptions.SectionName, cloudinaryOptions);
              var account = new Account(
                cloudinaryOptions.CloudName,
                cloudinaryOptions.ApiKey,
                cloudinaryOptions.ApiSecret
                );

             _cloudinary = new Cloudinary(account);
         }

        public async Task<CloudinaryUploadResponse> UploadImage(IFormFile fileToUpload)
        {
            try
            {
                var uploadResult = new RawUploadResult();
                using (var fileStream = fileToUpload.OpenReadStream())
                {
                    var UploadParams = new RawUploadParams()
                    {
                        File = new FileDescription(fileToUpload.FileName, fileStream)
                    };
                    uploadResult = await _cloudinary.UploadAsync(UploadParams);
                }
                CloudinaryUploadResponse response = new CloudinaryUploadResponse
                {
                    PublicId = uploadResult.PublicId,
                    Url = uploadResult.Url.ToString(),
                };

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                return null;
            }
        }
    }

}

