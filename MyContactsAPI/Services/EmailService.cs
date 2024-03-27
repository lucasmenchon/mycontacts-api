using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
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
                string host = _configuration.GetValue<string>("smtp:host");
                int port = _configuration.GetValue<int>("smtp:port");
                string username = _configuration.GetValue<string>("smtp:username");
                string password = _configuration.GetValue<string>("smtp:password");
                string name = _configuration.GetValue<string>("smtp:name");

                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress(name, username));
                emailMessage.To.Add(new MailboxAddress("", email));
                emailMessage.Subject = "Verificação da conta";

                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = $"Seu código de verificação é: {verificationCode}";
                emailMessage.Body = bodyBuilder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(host, port, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(username, password);
                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao enviar e-mail de verificação.", ex);
            }
        }

    }
}
