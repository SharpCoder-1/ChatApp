using ChatApp.Server.DTOs.Account;
using Mailjet.Client;
using Mailjet.Client.TransactionalEmails;

namespace ChatApp.Server.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<bool> SendEmailAsync(EmailSendDto emailDto)
        {
            var client = new MailjetClient(_config["Mailjet:ApiKey"], _config["Mailjet:SecretKey"]);

            var email = new TransactionalEmailBuilder()
                .WithFrom(new SendContact(_config["Email:From"], _config["Email:ApplicationName"]))
                .WithSubject(emailDto.Subject)
                .WithHtmlPart(emailDto.Body)
                .WithTo(new SendContact(emailDto.To))
                .Build();
            var response = await client.SendTransactionalEmailAsync(email);
            if (response.Messages != null)
            {
                if (response.Messages[0].Status == "success")
                    return true;
            }
            return false;
                
        }
    }
}
