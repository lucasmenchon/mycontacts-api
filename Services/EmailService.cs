using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MyContactsAPI.Extensions;
using MyContactsAPI.Interfaces;
using MyContactsAPI.Models.EmailModels;

namespace MyContactsAPI.Services;

public class EmailService : IEmailService
{
    public async Task SendVerificationEmailAsync(string email, string verificationCode)
    {
        try
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(Configuration.Email.Name, Configuration.Email.FromEmail));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = "Verificação da conta";

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = HtmlEmail.GenerateVerificationEmailHTML(verificationCode);
            emailMessage.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(Configuration.Email.Host, Configuration.Email.Port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(Configuration.Email.FromEmail, Configuration.Email.Password);
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
