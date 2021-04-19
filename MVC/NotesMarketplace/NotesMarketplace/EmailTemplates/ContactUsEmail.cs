using NotesMarketplace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace NotesMarketplace.EmailTemplates
{
    public class ContactUsEmail
    {
        public static void ContactEmail(string supportEmail, string emailPassword, ContactUs contactUS, string emails)
        {
            foreach (string email in emails.Split(','))
            {

                var fromEmail = new MailAddress(supportEmail, contactUS.EmailID); //Need system email address
                var toEmail = new MailAddress(email);
                var fromEmailPassword = "password"; // Replace with actual password of support email
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
}