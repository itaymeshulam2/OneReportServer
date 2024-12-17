using System.Net;
using System.Net.Mail;
using Newtonsoft.Json;
using OneReportServer.Client.Interface;
using OneReportServer.Contract.Response;
using OneReportServer.Model;

namespace OneReportServer.Client.Implementation
{
    public class EmailClient: IEmailClient
    {

        public EmailClient(ILogger<EmailClient> logger)
        {

        }

        public void SendMessage(string message, string ToAddress)
        {
            string fromAddress = "hativa.11.esh@gmail.com";
            string subject = "Hativa 11";
            string body = message;

            // Create the email message
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(fromAddress);
            mail.To.Add(ToAddress);
            mail.Subject = subject;
            mail.Body = body;

            // Set up the SMTP client
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("hativa.11.esh@gmail.com", SettingsDetails.EmailPassword);
            smtp.EnableSsl = true;

            // Send the email
            smtp.Send(mail);
        }
    }
}
