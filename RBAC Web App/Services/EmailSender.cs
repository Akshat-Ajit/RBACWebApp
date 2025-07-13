using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace RBACWebApp.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }

    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var smtpClient = new SmtpClient(_configuration["EmailSettings:Host"])
            {
                Port = int.Parse(_configuration["EmailSettings:Port"]),
                Credentials = new NetworkCredential(
                    _configuration["EmailSettings:UserName"],
                    _configuration["EmailSettings:Password"]),
                EnableSsl = bool.Parse(_configuration["EmailSettings:EnableSSL"])
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["EmailSettings:UserName"]),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
