using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace NoteMarketPlace.EmailTemplates
{
    public class DownloadAllowedEmail
    {
        public static void DoenloadAllowedNotifyEmail(User buyerUser, User sellerUser)
        {
            var fromEmail = new MailAddress("balanitinku@gmail.com", "Notes Marketplace"); //Need System Email Address
            var toEmail = new MailAddress(buyerUser.EmailID);
            var fromEmailPassword = "Pass#43210"; // Replace this password with actual password
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