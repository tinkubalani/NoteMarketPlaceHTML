using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace NoteMarketPlace.EmailTemplates
{
    public class NoteUnPublishedEmail
    {
        public static void NoteUnPublishedNotifyEmail(User sellerUser, string NoteTitle, string adminRemark)
        {
            var fromEmail = new MailAddress("balanitinku@gmail.com", "Notes Marketplace"); //Need System Email Address
            var toEmail = new MailAddress(sellerUser.EmailID);
            var fromEmailPassword = "Pass#43210"; // Replace this password with actual password
            string subject = "Sorry! We need to remove your notes from our portal.";
            string body = "Hello "+ sellerUser.FirstName + " " + sellerUser.LastName;
            body += "<br/>We would like to inform you that,<b> " + NoteTitle + "</b>" + " has been removed from the portal.";
            body += "<br/><br/>Please find our remarks as below -<br/>";
            body += ""+ adminRemark + "";
            body += "<br/><br/>Regards,<br/>";
            body += "Notes Marketplace";

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