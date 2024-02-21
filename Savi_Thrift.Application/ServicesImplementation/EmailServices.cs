using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using MimeKit;
using Savi_Thrift.Application.Interfaces.Repositories;
using Savi_Thrift.Application.Interfaces.Services;
using Savi_Thrift.Domain.Entities.Helper;

namespace Savi_Thrift.Infrastructure.Services
{
    public class EmailServices : IEmailServices
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailServices> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public EmailServices(ILogger<EmailServices> Logger, EmailSettings emailSettings, IUnitOfWork unitOfWork)
        {
            _emailSettings = emailSettings;
            _logger = Logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> SendEmailAsync(string link, string email,string id)
        {
            try
            {
                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = $"<a href='{link}'>Click here to confirm your email</a>"
                };

                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress(_emailSettings.DisplayName, _emailSettings.Email));
                emailMessage.To.Add(new MailboxAddress(email, email));
                emailMessage.Subject = "Confirm your email";
                emailMessage.Body = bodyBuilder.ToMessageBody();

                using var client = new SmtpClient();

                // Adjustments for SSL/TLS handshake issue
                client.ServerCertificateValidationCallback = (s, c, h, e) => true; // Permissive certificate validation
                await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port); // Use STARTTLS
                await client.AuthenticateAsync(_emailSettings.Email, _emailSettings.Password);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
                return "success";
            }
            catch (Exception ex)
            {
                var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
                if(user != null)
                {
                     _unitOfWork.UserRepository.DeleteAsync(user);
                    await _unitOfWork.SaveChangesAsync();
                }
                _logger.LogError(ex, "Error occurred while sending email.");
                return "Error occurred while sending email. Check your internet connection.";
            }
        }

        public async Task SendMailAsync(MailRequest mailRequest)
        {
            try
            {
                var emailMessage = new MimeMessage();

                emailMessage.From.Add(new MailboxAddress(_emailSettings.DisplayName, _emailSettings.Email));
                emailMessage.To.Add(new MailboxAddress(mailRequest.ToEmail, mailRequest.ToEmail));

                emailMessage.Subject = mailRequest.Subject;

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = $"{mailRequest.Body}<br/>"
                };

                emailMessage.Body = bodyBuilder.ToMessageBody();

                using var client = new SmtpClient();
                await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.SslOnConnect);
                    await client.AuthenticateAsync(_emailSettings.Email, _emailSettings.Password);
                    await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending email.");
                throw;
            }

            

        }        

    }
}
