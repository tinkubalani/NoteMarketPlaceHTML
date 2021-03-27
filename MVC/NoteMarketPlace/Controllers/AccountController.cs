using NoteMarketPlace.EmailTemplates;
using NoteMarketPlace.Password_Encryption;
using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace NoteMarketPlace.Controllers
{

    [RoutePrefix("Account")]
    public class AccountController : Controller
    {

        private readonly NoteMarketplaceEntities objNoteMarketplaceEntities = new NoteMarketplaceEntities();

        [Route("Login")]
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [Route("Login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin user)
        {
            if (ModelState.IsValid)
            {
                //Encrypt Password and Save
                var newPassword = EncryptPassword.EncryptPasswordMd5(user.Password);

                bool isValid = objNoteMarketplaceEntities.Users.Any(x => x.EmailID == user.EmailID && x.Password == newPassword);
                if (isValid)
                {
                    User userDetails = objNoteMarketplaceEntities.Users.Where(x => x.EmailID == user.EmailID && x.Password == newPassword).FirstOrDefault();
                    if (userDetails.IsEmailVerified)
                    {
                        FormsAuthentication.SetAuthCookie(user.EmailID, user.RememberMe);

                        if (userDetails.RoleID == objNoteMarketplaceEntities.UserRoles.Where(x => x.Name.ToLower() == "admin").Select(x => x.ID).FirstOrDefault())
                        {
                            return RedirectToAction("Index", "Admin");
                        }
                        else
                        {
                            var FirstTime = objNoteMarketplaceEntities.UserProfiles.Any(x => x.UserID == userDetails.ID);
                            if (FirstTime)
                            {
                                return RedirectToAction("SearchNotes", "Home");
                            }
                            return RedirectToAction("UserProfile", "User");
                        }
                    }
                    TempData["Error"] = "Email Address Is Not Verified";
                    return View();
                }
                TempData["Error"] = "Invalid username or password";
                return View();
            }
            return View();
        }

        [Route("Register")]
        // GET: 
        public ActionResult Register()
        {
            return View();
        }

        public JsonResult EmailExits(string EmailID)
        {
            return Json(!objNoteMarketplaceEntities.Users.Any(u => u.EmailID == EmailID), JsonRequestBehavior.AllowGet);
        }

        [Route("Register")]
        [HttpPost]
        public ActionResult Register(UserRegistration objUser)
        {
            if (ModelState.IsValid)
            {
                UserRole role = objNoteMarketplaceEntities.UserRoles.Where(x => x.Name.ToLower() == "member").FirstOrDefault();
                User obj = new User
                {
                    FirstName = objUser.FirstName,
                    LastName = objUser.LastName,
                    EmailID = objUser.EmailID,
                    RoleID = role.ID,
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                    IsEmailVerified = false
                };

                string activationCode = Guid.NewGuid().ToString();

                //Encrypt Password and Save
                obj.VerificationCode = activationCode;
                obj.Password = EncryptPassword.EncryptPasswordMd5(objUser.Password);
                var userName = objUser.FirstName.ToString();

                // Adding User To DataBase
                objNoteMarketplaceEntities.Users.Add(obj);
                objNoteMarketplaceEntities.SaveChanges();

                // Generating Email Verification Link
                var verifyUrl = "/Account/VerifyAccount/?VerificationCode=" + activationCode;
                var activationlink = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

                // Sending Email
                EmailVerification.SendVerificationLinkEmail(obj, activationlink);


                //Redirect To VerifyEmail Page
                TempData["userName"] = userName;
                return new RedirectResult(@"~\Account\VerifyEmail\");

            }
            return View();
        }



        [HttpGet]
        public ActionResult VerifyAccount(string VerificationCode)
        {
            using (NoteMarketplaceEntities dc = new NoteMarketplaceEntities())
            {
                dc.Configuration.ValidateOnSaveEnabled = false; // This line I have added here to avoid 
                                                                // Confirm password does not match issue on save changes
                var v = dc.Users.Where(x => x.VerificationCode == VerificationCode).FirstOrDefault();
                if (v != null)
                {
                    v.IsEmailVerified = true;
                    dc.SaveChanges();
                }
                else
                {
                    ViewBag.Message = "Invalid Request";
                }
            }

            @TempData["Message"] = "Your Email Is Verified You Can Login Here";
            return RedirectToAction("Login", "Account");
        }


        [Route("VerifyEmail")]
        public ViewResult VerifyEmail()
        {
            return View();
        }


        [Route("ForgotPassword")]
        // GET:
        public ActionResult ForgotPassword()
        {
            return View();
        }


        [Route("ForgotPassword")]
        [HttpPost]
        public ActionResult ForgotPassword(ForgotPassword obj)
        {
            if (ModelState.IsValid)
            {
                bool isValid = objNoteMarketplaceEntities.Users.Any(x => x.EmailID == obj.EmailID);
                if (isValid)
                {
                    User userDetails = objNoteMarketplaceEntities.Users.Where(x => x.EmailID == obj.EmailID).FirstOrDefault();
                    Random rand = new Random();
                    var otp = rand.Next(10000, 99999);
                    var strotp = otp.ToString();

                    //Encrypt Password and Save
                    userDetails.Password = EncryptPassword.EncryptPasswordMd5(strotp);
                    objNoteMarketplaceEntities.SaveChanges();

                    //Sent Otp On email address
                    ForgotPasswordEmail.SendOtpToEmail(userDetails, otp);

                    TempData["Message"] = "Otp Sent To Your Registered EmailAddress use it for login";
                    return RedirectToAction("Login", "Account");

                }
                TempData["Error"] = "Invalid EmailAddress";
                return View();
            }
            return View();
        }

        [Authorize]
        [Route("ChangePassword")]
        // GET:
        public ActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [Route("ChangePassword")]
        [HttpPost]
        // GET:
        public ActionResult ChangePassword(ChangePassword changePassword)
        {
            if (ModelState.IsValid)
            {
                if (User.Identity.IsAuthenticated)
                {
                    var Emailid = User.Identity.Name.ToString();

                    User user = objNoteMarketplaceEntities.Users.Where(x => x.EmailID == Emailid).FirstOrDefault();

                    var pwd = EncryptPassword.EncryptPasswordMd5(changePassword.OldPassword);

                    if (user.Password == pwd)
                    {
                        user.Password = EncryptPassword.EncryptPasswordMd5(changePassword.NewPassword);
                        user.ModifiedDate = DateTime.Now;
                        user.ModifiedBy = user.ID;

                        objNoteMarketplaceEntities.Entry(user).State = EntityState.Modified;
                        objNoteMarketplaceEntities.SaveChanges();

                        return RedirectToAction("Index", "User");
                    }
                    else
                    {
                        TempData["IncorrectPwd"] = "Old Password Is Incorrect !";
                        return View();
                    }
                }
                else
                {
                    return View();
                }
            }
            return View();
        }


        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }


    }
}