using NotesMarketplace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace NotesMarketplace.EmailTemplates
{
    public class ReportedSpamEmail
    {
        public static void BuyerReportebSpamNotifyEmail(string supportEmail, string emailPassword, User sellerUser, User buyerUser, string noteTitle, string emails)
        {

            foreach (string email in emails.Split(','))
            {
                var fromEmail = new MailAddress(supportEmail, "Notes Marketplace"); //Need system email address
                var toEmail = new MailAddress(email);
                var fromEmailPassword = "password"; // Replace with actual password of support email
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
}