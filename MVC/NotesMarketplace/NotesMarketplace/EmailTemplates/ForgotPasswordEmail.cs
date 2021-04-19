using NotesMarketplace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace NotesMarketplace.EmailTemplates
{
    public class ForgotPasswordEmail
    {
        public static void SendOtpToEmail(string supportEmail, string emailPassword, User objUser, int otp)
        {
            var fromEmail = new MailAddress(supportEmail, "Notes Marketplace"); //Need system email address
            var toEmail = new MailAddress(objUser.EmailID);
            var fromEmailPassword = "password"; // Replace with actual password of support email
            string subject = "New Temporary Password has been created for you";
            string body = "Hello " + objUser.FirstName + " " + objUser.LastName + "<br/>";
            body += "We have generated a new password for you <br/>"; 
            body += "Password: "+otp;
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