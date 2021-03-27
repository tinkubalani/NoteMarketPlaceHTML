using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace NoteMarketPlace.EmailTemplates
{
    public class ContactUsEmail
    {
        public static void ContactEmail(ContactUs contactUS)
        {
            var fromEmail = new MailAddress("balanitinku@gmail.com", contactUS.EmailID); //Need System Email Address
            var toEmail = new MailAddress("balanitinku@gmail.com");
            var fromEmailPassword = "Pass#43210"; // Replace this password with actual password
            string subject = "" + contactUS.FullName + " - " + contactUS.Subject + " ";
            string body = "Hello Admin,<br/></br>" + contactUS.Comments + "<br/>";
            body += "<br/><br/>Regards,<br/>";
            body += contactUS.FullName;


            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }
    }
}