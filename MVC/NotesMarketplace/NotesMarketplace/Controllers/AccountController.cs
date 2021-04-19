using NotesMarketplace.EmailTemplates;
using NotesMarketplace.Password_Encryption;
using NotesMarketplace.Models;
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

namespace NotesMarketplace.Controllers
{

    [RoutePrefix("Account")]
    public class AccountController : Controller
    {

        private readonly NoteMarketplaceEntities dbobj  = new NoteMarketplaceEntities();

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

                bool isValid = dbobj .Users.Any(x => x.EmailID == user.EmailID && x.Password == newPassword && x.IsActive == true);
                if (isValid)
                {
                    User userDetails = dbobj .Users.Where(x => x.EmailID == user.EmailID && x.Password == newPassword).FirstOrDefault();
                    if (userDetails.IsEmailVerified)
                    {
                        FormsAuthentication.SetAuthCookie(user.EmailID, user.RememberMe);

                        if (userDetails.RoleID == dbobj .UserRoles.Where(x => x.Name.ToLower() == "admin" && x.IsActive == true).Select(x => x.ID).FirstOrDefault())
                        {
                            UserProfile userprofile = dbobj .UserProfiles.Where(x => x.UserID == userDetails.ID).FirstOrDefault();
                            if (userprofile != null)
                            {
                                return RedirectToAction("Index", "Admin");
                            }
                            return RedirectToAction("MyProfile", "Admin");
                        }
                        else if (userDetails.RoleID == dbobj .UserRoles.Where(x => x.Name.ToLower() == "superadmin" && x.IsActive == true).Select(x => x.ID).FirstOrDefault())
                        {
                            UserProfile userprofile = dbobj .UserProfiles.Where(x => x.UserID == userDetails.ID).FirstOrDefault();
                            if (userprofile != null)
                            {
                                return RedirectToAction("Index", "Admin");
                            }
                            return RedirectToAction("MyProfile", "Admin");
                        }
                        else
                        {
                            var FirstTime = dbobj .UserProfiles.Any(x => x.UserID == userDetails.ID);
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
            return Json(!dbobj .Users.Any(u => u.EmailID == EmailID), JsonRequestBehavior.AllowGet);
        }

        [Route("Register")]
        [HttpPost]
        public ActionResult Register(UserRegistration objUser)
        {
            if (ModelState.IsValid)
            {
                UserRole role = dbobj .UserRoles.Where(x => x.Name.ToLower() == "member").FirstOrDefault();
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


                //Encrypt Password and Save
                obj.Password = EncryptPassword.EncryptPasswordMd5(objUser.Password);
                var userName = objUser.FirstName.ToString();

                // Adding User To DataBase
                dbobj .Users.Add(obj);
                dbobj .SaveChanges();

                // Generating Email Verification Link
                //obj.password will also work as a activation code for verifying email address
                var verifyUrl = "/Account/VerifyAccount/?VerificationCode=" + obj.Password;
                var activationlink = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

                var SupportEmailAddress = dbobj .SystemConfigurations.Where(x => x.Key.ToLower() == "supportemailaddress").Select(y => y.Value).FirstOrDefault();
                var EmailPassword = dbobj .SystemConfigurations.Where(x => x.Key.ToLower() == "emailpassword").Select(y => y.Value).FirstOrDefault();

                // Sending Email
                EmailVerification.SendVerificationLinkEmail(SupportEmailAddress, EmailPassword, obj, activationlink);


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
                dc.Configuration.ValidateOnSaveEnabled = false; 
                                                                // Confirm password does not match issue on save changes
                var v = dc.Users.Where(x => x.Password == VerificationCode).FirstOrDefault();
                if (v != null)
                {
                    v.IsEmailVerified = true;
                    v.IsActive = true;
                    dc.SaveChanges();
                    @TempData["Message"] = "Your Email Is Verified You Can Login Here";
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    @TempData["Error"] = "Invalid Request";
                    return RedirectToAction("Login", "Account");
                }
            }
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
                bool isValid = dbobj .Users.Any(x => x.EmailID == obj.EmailID);
                if (isValid)
                {
                    User userDetails = dbobj .Users.Where(x => x.EmailID == obj.EmailID).FirstOrDefault();
                    Random rand = new Random();
                    var otp = rand.Next(10000, 99999);
                    var strotp = otp.ToString();

                    //Encrypt Password and Save
                    userDetails.Password = EncryptPassword.EncryptPasswordMd5(strotp);
                    dbobj .SaveChanges();

                    var SupportEmailAddress = dbobj .SystemConfigurations.Where(x => x.Key.ToLower() == "supportemailaddress").Select(y => y.Value).FirstOrDefault();
                    var EmailPassword = dbobj .SystemConfigurations.Where(x => x.Key.ToLower() == "emailpassword").Select(y => y.Value).FirstOrDefault();

                    //Sent Otp On email address
                    ForgotPasswordEmail.SendOtpToEmail(SupportEmailAddress, EmailPassword, userDetails, otp);

                    TempData["Message"] = "Otp Sent To Your Registered Email Address use it for login";
                    return RedirectToAction("Login", "Account");

                }
                TempData["Error"] = "Invalid Email Address";
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

                    User user = dbobj .Users.Where(x => x.EmailID == Emailid).FirstOrDefault();

                    var pwd = EncryptPassword.EncryptPasswordMd5(changePassword.OldPassword);

                    if (user.Password == pwd)
                    {
                        user.Password = EncryptPassword.EncryptPasswordMd5(changePassword.NewPassword);
                        user.ModifiedDate = DateTime.Now;
                        user.ModifiedBy = user.ID;

                        dbobj .Entry(user).State = EntityState.Modified;
                        dbobj .SaveChanges();

                        return RedirectToAction("Index", "User");
                    }
                    else
                    {
                        TempData["Password-Incorrect"] = "Old Password Is Incorrect !";
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
            Session.Abandon();
            return RedirectToAction("Login");
        }


    }
}