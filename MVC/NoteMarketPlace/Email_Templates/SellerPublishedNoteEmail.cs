using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace NoteMarketPlace.EmailTemplates
{
    public class SellerPublishedNoteEmail
    {
        public static void SellerPublishedNoteNotifyEmail(User sellerUser, string NoteTitle)
        {
            var fromEmail = new MailAddress("balanitinku@gmail.com", "Notes Marketplace"); //Need System Email Address
            var toEmail = new MailAddress("balanitinku@gmail.com");
            var fromEmailPassword = "Pass#43210"; // Replace this password with actual password
            string subject = "" + sellerUser.FirstName + " " + sellerUser.LastName + " - sent his note for review";
            string body = "Hello Admins,";
            body += "<br/>We would like to inform you that,<b> " + sellerUser.FirstName + " " + sellerUser.LastName + "</b>" +
                " sent his note <b> " + NoteTitle + "</b>" + " for review.Please look at the notes and take required actions.";
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