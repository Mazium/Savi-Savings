using CloudinaryDotNet;
using Microsoft.Extensions.Options;
using Savi_Thrift.Domain.Entities;

namespace Savi_Thrift.Configurations
{
    public static class CloudinaryServiceExtension
    {
        public static void AddCloudinaryService(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));


            services.AddSingleton(provider =>
            {
                var cloudinarySettings = provider.GetRequiredService<IOptions<CloudinarySettings>>().Value;

                Account cloudinaryAccount = new(
                    cloudinarySettings.CloudName,
                    cloudinarySettings.ApiKey,
                cloudinarySettings.ApiSecret);

                return new Cloudinary(cloudinaryAccount);
            });
        }

    }
}
