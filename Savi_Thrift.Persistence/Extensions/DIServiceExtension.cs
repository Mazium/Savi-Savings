using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Savi_Thrift.Domain.Entities.Helper;
using Savi_Thrift.Application.ServicesImplementation;
using Savi_Thrift.Application.Interfaces.Services;
using Savi_Thrift.Infrastructure.Services;
//using NETCore.MailKit.Core;

namespace Savi_Thrift.Persistence.Extensions
{
    public static class DIServiceExtension
    {
        

        public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EmailSettings>(formulaOption =>
            {
            });

            var registry = new EmailSettings();

            services.AddSingleton(registry);

            //services.AddTransient<IEmailService, NETCore.MailKit.Core.EmailService>();
            services.AddTransient <IEmailServices, EmailServices>();

        }

    }
}

