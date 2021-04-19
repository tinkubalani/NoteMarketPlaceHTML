using NotesMarketplace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace NotesMarketplace.EmailTemplates
{
    public class NoteUnPublishedEmail
    {
        public static void NoteUnPublishedNotifyEmail(string supportEmail, string emailPassword, User sellerUser, string NoteTitle, string adminRemark)
        {
            var fromEmail = new MailAddress(supportEmail, "Notes Marketplace"); //Need system email address
            var toEmail = new MailAddress(sellerUser.EmailID);
            var fromEmailPassword = "password"; // Replace with actual password of support email
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