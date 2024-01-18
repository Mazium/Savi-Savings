using Savi_Thrift.Domain.Entities.Helper;

namespace Savi_Thrift.Configurations
{
    public static class MailServiceExtension
    {
        public static void AddMailService(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<EmailSettings>(config.GetSection("EmailSettings"));
        }
    }
}
