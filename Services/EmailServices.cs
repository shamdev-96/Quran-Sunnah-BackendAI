using Quran_Sunnah_BackendAI.Interfaces;
using System.Net.Mail;
using System.Net;
using System.Text;
using Quran_Sunnah_BackendAI.Dtos;

namespace Quran_Sunnah_BackendAI.Services
{
    public class EmailServices : IEmailServices
    {
        public void SendExceptionEmail(EmailExceptionContent content)
        {
            try
            {
                // Set up SMTP client
                SmtpClient client = new SmtpClient("smtp.gmail.com", 465);
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
      
                // Create email message
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("syafishamsalleh@gmail.com");
                mailMessage.To.Add("quransunnahai@gmail.com");
                mailMessage.Subject = content.Subject;
                mailMessage.IsBodyHtml = true;
                StringBuilder mailBody = new StringBuilder();
                mailBody.AppendFormat($"<p>DateTime Exceptionn: {content.ExceptionDateTime}</p>");
                //mailBody.AppendFormat("<br />");
                //mailBody.AppendFormat($"<p>Exceptionn: {content.ExceptionMessage}</p>");
                mailBody.AppendFormat("<br />");
                mailBody.AppendFormat($"<p>Stack Trace: {content.StackTrace}</p>");
                mailMessage.Body = mailBody.ToString();

                // Send email
                client.Send(mailMessage);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

    }
}

