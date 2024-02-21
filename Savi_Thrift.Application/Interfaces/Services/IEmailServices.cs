using Savi_Thrift.Domain.Entities.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savi_Thrift.Application.Interfaces.Services
{
    public interface IEmailServices
    {
        Task<string> SendEmailAsync(string link, string email, string id);

		Task SendMailAsync(MailRequest mailRequest);
    }
}
