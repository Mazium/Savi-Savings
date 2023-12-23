using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NETCore.MailKit.Core;
using Savi_Thrift.Application.Interfaces.Services;
using Savi_Thrift.Domain.Entities.Helper;

namespace Savi_Thrift.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerMailController : ControllerBase
    {
        private readonly IEmailServices emailService;

        public CustomerMailController(IEmailServices emailService)
        {
            this.emailService = emailService;
        }
        [HttpPost("SendMail")]
        public async Task<IActionResult> SendMail()
        {
            try
            {
                MailRequest mailRequest = new MailRequest();
                 mailRequest.ToEmail = "appjob06@gmail.com";
                //mailRequest.ToEmail = "oliverchuks@gmail.com";
                mailRequest.Subject = "Welcome To Savi Savings";
                mailRequest.Body = "Thanks For Saving With Us1";

                await emailService.SendEmailAsync(mailRequest);
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
