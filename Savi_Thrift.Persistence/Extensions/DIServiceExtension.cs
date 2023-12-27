using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Savi_Thrift.Application;
using Savi_Thrift.Application.Interfaces.Services;
using Savi_Thrift.Domain.Entities;

namespace Savi_Thrift.Persistence.Extensions
{
    public static class DIServiceExtension
    {
        public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            var cloudinarySettings = new CloudinarySettings();
            configuration.GetSection("CloudinarySettings").Bind(cloudinarySettings);
            services.AddSingleton(cloudinarySettings);
            services.AddScoped(typeof(ICloudinaryServices), typeof(CloudinaryServices));
        }
    }
}