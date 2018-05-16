using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ElectronicJournal
{
    public class EmailService
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MailMessage();

            emailMessage.From = new MailAddress("electronicjournalgroup@gmail.com", "Адміністрація сайту");
            emailMessage.To.Add(new MailAddress(email));
            emailMessage.Subject = subject;
            emailMessage.IsBodyHtml = true;
            emailMessage.Body = message;

            using (var client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("electronicjournalgroup@gmail.com", "ElectronicJournal123");

                await client.SendMailAsync(emailMessage);
            }
        }
    }
}
