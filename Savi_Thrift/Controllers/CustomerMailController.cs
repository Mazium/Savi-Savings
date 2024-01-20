using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NETCore.MailKit.Core;
using Org.BouncyCastle.Security;
using Savi_Thrift.Application;
using Savi_Thrift.Application.Interfaces.Services;
using Savi_Thrift.Domain.Entities.Helper;

namespace Savi_Thrift.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerMailController : ControllerBase
    {
        private readonly IEmailServices emailService;
        private readonly ICloudinaryServices<CustomerMailController> _cloudinaryServices;

        public CustomerMailController(IEmailServices emailService, ICloudinaryServices<CustomerMailController> cloudinaryServices)

        {
            this.emailService = emailService;
            _cloudinaryServices = cloudinaryServices;
        }
        //[HttpPost("SendMail")]
        //public async Task<IActionResult> SendMail()
        //{
        //    try
        //    {
        //        MailRequest mailRequest = new MailRequest();
        //        mailRequest.ToEmail = "appjob06@gmail.com";
        //        //mailRequest.ToEmail = "oliverchuks@gmail.com";
        //        mailRequest.Subject = "Welcome To Savi Savings";
        //        mailRequest.Body = "Thanks For Saving With Us1";

        //        await emailService.SendmailAsync(mailRequest);
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        [HttpPost("UploadFile")]
        public async Task<IActionResult> CloudinaryTest(IFormFile image)
        {
              var imageToUpload = HttpContext.Request.Form.Files[0];
              var response = await _cloudinaryServices.UploadImage(imageToUpload);
           
              return Ok();

        }
    }
}
