using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MyContactsAPI.Extensions;
using MyContactsAPI.Helper;
using MyContactsAPI.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace MyContactsAPI.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendVerificationEmailAsync(string email, string verificationCode)
        {
            try
            {
                // Construir mensagem de e-mail
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress(Configuration.Email.DefaultFromName, Configuration.Email.DefaultFromEmail));
                emailMessage.To.Add(new MailboxAddress("", email));
                emailMessage.Subject = "Verificação da conta";

                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = HtmlEmail.GenerateVerificationEmailHTML(verificationCode);
                emailMessage.Body = bodyBuilder.ToMessageBody();

                // Enviar e-mail usando SmtpClient
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(Configuration.Email.DefaultFromHost, Configuration.Email.DefaultFromPort, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(Configuration.Email.DefaultFromEmail, Configuration.Email.DefaultFromPassword);
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
