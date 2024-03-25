using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MyContactsAPI.Interfaces;

namespace MyContactsAPI.Helper
{
    public class EmailModel : IEmail
    {
        private readonly IConfiguration _configuration;

        public EmailModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendEmailAsync(string email, string subject, string message)
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
                emailMessage.Subject = subject;

                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = message;
                emailMessage.Body = bodyBuilder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(host, port, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(username, password);
                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
