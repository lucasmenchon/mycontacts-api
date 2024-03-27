using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MyContactsAPI.Helper;
using MyContactsAPI.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace MyContactsAPI.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendVerificationEmailAsync(string email, string verificationCode)
        {
            try
            {
                // Construir mensagem de e-mail
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress(_configuration["smtp:name"], _configuration["smtp:username"]));
                emailMessage.To.Add(new MailboxAddress("", email));
                emailMessage.Subject = "Verificação da conta";

                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = HtmlEmail.GenerateVerificationEmailHTML(verificationCode);
                emailMessage.Body = bodyBuilder.ToMessageBody();

                // Enviar e-mail usando SmtpClient
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_configuration["smtp:host"], _configuration.GetValue<int>("smtp:port"), SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_configuration["smtp:username"], _configuration["smtp:password"]);
                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao enviar email de verificação.", ex);
            }
        }

    }
}
