using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace NoteMarketPlace.EmailTemplates
{
    public class ReportedSpamEmail
    {
        public static void BuyerReportebSpamNotifyEmail(User sellerUser, User buyerUser, string noteTitle)
        {
            var fromEmail = new MailAddress("balanitinku@gmail.com", "Notes Marketplace"); //Need System Email Address
            var toEmail = new MailAddress("balanitinku@gmail.com");
            var fromEmailPassword = "Pass#43210"; // Replace this password with actual password
            string subject = "" + buyerUser.FirstName + " " + buyerUser.LastName + " Reported an issue for " + noteTitle;
            string body = "Hello Admins,";
            body += "<br/>We would like to inform you that,<b> " + buyerUser.FirstName + " " + buyerUser.LastName + "</b>" 
                + " Reported an issue for <b>" + sellerUser.FirstName + " " + sellerUser.LastName + "</b>" + "’s Note with title <b>" 
                + noteTitle + "</b>" + ". Please look at the notes and take required actions.";
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