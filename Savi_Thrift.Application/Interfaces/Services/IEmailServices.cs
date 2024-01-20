using Savi_Thrift.Domain.Entities.Helper;

namespace Savi_Thrift.Application.Interfaces.Services
{
    public interface IEmailServices
    {
        Task SendEmailAsync(string link, string email);
        Task SendMailAsync(MailRequest mailRequest);
    }
}
