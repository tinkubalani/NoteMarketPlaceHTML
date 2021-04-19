using NotesMarketplace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace NotesMarketplace.EmailTemplates
{
    public class DownloadAllowedEmail
    {
        public static void DoenloadAllowedNotifyEmail(string supportEmail, string emailPassword, User buyerUser, User sellerUser)
        {
            var fromEmail = new MailAddress(supportEmail, "Notes Marketplace"); //Need system email address
            var toEmail = new MailAddress(buyerUser.EmailID);
            var fromEmailPassword = "password"; // Replace with actual password of support email
            string subject = "<" + sellerUser.FirstName + " " + sellerUser.LastName + "> - Allows you to download a note";
            string body = "Hello " + buyerUser.FirstName + " " + buyerUser.LastName + "<br/>";
            body += "<br/>We would like to inform you that,<b> " + sellerUser.FirstName + " " + sellerUser.LastName + "</b>" +
                " Allows you to download a note. Please login and see My Download tabs to download particular note.";
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