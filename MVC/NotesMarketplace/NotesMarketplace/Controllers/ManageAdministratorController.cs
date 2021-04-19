using NotesMarketplace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using NotesMarketplace.Password_Encryption;
using NotesMarketplace.EmailTemplates;
using System.IO;
using System.Net;
using System.Data.Entity;

namespace NotesMarketplace.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    [RoutePrefix("Admin")]
    public class ManageAdministratorController : Controller
    {
        private readonly NoteMarketplaceEntities dbobj = new NoteMarketplaceEntities();

        [Route("Administrators")]
        public ActionResult Administrators(int? page, string Search, string SortOrder)
        {
            ViewBag.DateSortParam = string.IsNullOrEmpty(SortOrder) ? "CreatedDate_asc" : "";
            ViewBag.FirstNameSortParam = SortOrder == "FirstName" ? "FirstName_desc" : "FirstName";
            ViewBag.LastNameSortParam = SortOrder == "LastName" ? "LastName_desc" : "LastName";
            ViewBag.EmailIDSortParam = SortOrder == "EmailID" ? "EmailID_desc" : "EmailID";
            ViewBag.ActiveSortParam = SortOrder == "Active" ? "Active_desc" : "Active";

            var admin = dbobj.Users.Where(x => x.RoleID == dbobj.UserRoles.Where(y => y.Name.ToLower() == "admin").Select(z => z.ID).FirstOrDefault() && (x.FirstName.Contains(Search) || x.LastName.Contains(Search) || x.EmailID.Contains(Search)|| x.UserProfiles.Select(u=>u.PhoneNumber).Contains(Search) || (x.ModifiedDate.Value.Day + "-" + x.ModifiedDate.Value.Month + "-" + x.ModifiedDate.Value.Year).Contains(Search) || Search == null)).AsQueryable();
            ViewBag.UsersProfiles = dbobj.UserProfiles.ToList();

            switch (SortOrder)
            {
                case "CreatedDate_asc":
                    admin = admin.OrderBy(x => x.ModifiedDate);
                    break;
                case "FirstName_desc":
                    admin = admin.OrderByDescending(x => x.FirstName);
                    break;
                case "FirstName":
                    admin = admin.OrderBy(x => x.FirstName);
                    break;
                case "LastName_desc":
                    admin = admin.OrderByDescending(x => x.LastName);
                    break;
                case "LastName":
                    admin = admin.OrderBy(x => x.LastName);
                    break;
                case "EmailID_desc":
                    admin = admin.OrderByDescending(x => x.EmailID);
                    break;
                case "EmailID":
                    admin = admin.OrderBy(x => x.EmailID);
                    break;
                case "Active_desc":
                    admin = admin.OrderByDescending(x => x.IsActive);
                    break;
                case "Active":
                    admin = admin.OrderBy(x => x.IsActive);
                    break;
                default:
                    admin = admin.OrderByDescending(x => x.ModifiedDate);
                    break;
            }

            return View(admin.ToList().ToPagedList(page ?? 1, 5));

        }

        [Route("AddAdministrator")]
        [HttpGet]
        public ActionResult AddAdministrator()
        {
            ViewBag.PhoneNumberCountryCode = dbobj.Countries.Where(x => x.IsActive == true);
            return View();
        }

        [Route("AddAdministrator")]
        [HttpPost]
        public ActionResult AddAdministrator(AddAdministrator addadministrator)
        {
            if (ModelState.IsValid)
            {
                var Emailid = User.Identity.Name.ToString();
                User userObj = dbobj.Users.Where(x => x.EmailID == Emailid).FirstOrDefault();

                User admin = new User
                {
                    FirstName = addadministrator.FirstName,
                    LastName = addadministrator.LastName,
                    EmailID = addadministrator.EmailID,
                    RoleID = dbobj.UserRoles.Where(x => x.Name.ToLower() == "admin").Select(y => y.ID).FirstOrDefault(),
                    Password = EncryptPassword.EncryptPasswordMd5("Admin@123"),
                    CreatedBy = userObj.ID,
                    ModifiedBy = userObj.ID,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now

                };
                dbobj.Users.Add(admin);


                dbobj.Users.Add(admin);
                dbobj.SaveChanges();

                // Generating Email Verification Link
                /* We will use encrypted password as a activation code for verifying email address*/
                var verifyUrl = "/Account/VerifyAccount/?VerificationCode=" + admin.Password;
                var activationlink = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

                var SupportEmailAddress = dbobj.SystemConfigurations.Where(x => x.Key.ToLower() == "supportemailaddress").Select(y => y.Value).FirstOrDefault();
                var EmailPassword = dbobj.SystemConfigurations.Where(x => x.Key.ToLower() == "emailpassword").Select(y => y.Value).FirstOrDefault();

                // Sending Email
                EmailVerification.SendVerificationLinkEmail(SupportEmailAddress, EmailPassword, admin, activationlink);

                var id = admin.ID;

                UserProfile adminprofile = new UserProfile
                {
                    PhoneNumberCountryCode = addadministrator.PhoneNumberCountryCode,
                    PhoneNumber = addadministrator.PhoneNumber,
                    UserID = id,
                    ProfilePicture = dbobj.SystemConfigurations.Where(x => x.Key.ToLower() == "defaultprofilepicture").Select(x => x.Value).FirstOrDefault(),
                    CreatedBy = userObj.ID,
                    CreatedDate = DateTime.Now,
                    ModifiedBy = userObj.ID,
                    ModifiedDate = DateTime.Now,
                    IsActive = true
                };

                dbobj.UserProfiles.Add(adminprofile);
                dbobj.SaveChanges();

                string storepath = Path.Combine(Server.MapPath("~/Members/"), id.ToString());
                if (!Directory.Exists(storepath))
                {
                    Directory.CreateDirectory(storepath);
                }

                TempData["success"] = userObj.FirstName + " " + userObj.LastName;
                TempData["message"] = "Administrator has been Successfully Added";
                return RedirectToAction("Administrators", "Admin");
            }
            ViewBag.PhoneNumberCountryCode = dbobj.Countries.Where(x => x.IsActive == true);
            return View();
        }

        [Route("EditAdministrator/{id}")]
        [HttpGet]
        public ActionResult EditAdministrator(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User admin = dbobj.Users.Find(id);
            UserProfile adminprofile = dbobj.UserProfiles.Where(x => x.UserID == admin.ID).FirstOrDefault();

            EditAdministrator addadministrator = new EditAdministrator
            {
                UserID = admin.ID,
                FirstName = admin.FirstName,
                LastName = admin.LastName,
                EmailID = admin.EmailID,
                PhoneNumber = adminprofile.PhoneNumber,
                IsEmailVerified = admin.IsEmailVerified
            };

            if (admin == null)
            {
                return RedirectToAction("Error", "Home");
            }

            ViewData["PhoneNumberCountryCode"] = new SelectList(dbobj.Countries.Distinct().Where(x => x.IsActive == true), "CountryCode", "CountryCode", adminprofile.PhoneNumberCountryCode);
            return View(addadministrator);
        }

        [Route("EditAdministrator/{id}")]
        [HttpPost]
        public ActionResult EditAdministrator(int? id, EditAdministrator addadministrator)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                User admin = dbobj.Users.Find(id);
                UserProfile adminprofile = dbobj.UserProfiles.Where(x => x.UserID == admin.ID).FirstOrDefault();
                if (admin == null)
                {
                    return RedirectToAction("Error", "Home");
                }

                var Emailid = User.Identity.Name.ToString();
                User userObj = dbobj.Users.Where(x => x.EmailID == Emailid).FirstOrDefault();

                admin.FirstName = addadministrator.FirstName;
                admin.LastName = addadministrator.LastName;
                admin.EmailID = addadministrator.EmailID;
                admin.ModifiedBy = userObj.ID;
                admin.ModifiedDate = DateTime.Now;

                dbobj.Entry(admin).State = EntityState.Modified;
                dbobj.SaveChanges();

                adminprofile.PhoneNumberCountryCode = addadministrator.PhoneNumberCountryCode;
                adminprofile.PhoneNumber = addadministrator.PhoneNumber;

                dbobj.Entry(adminprofile).State = EntityState.Modified;
                dbobj.SaveChanges();

                TempData["success"] = userObj.FirstName + " " + userObj.LastName;
                TempData["message"] = "Administrator has been Successfully edited";
                return RedirectToAction("Administrators", "Admin");
            }
            return View();
        }

        [Route("DeleteAdministrator/{id}")]
        [HttpGet]
        public ActionResult DeleteAdministrator(int? id)
        {
            var Emailid = User.Identity.Name.ToString();
            User userObj = dbobj.Users.Where(x => x.EmailID == Emailid).FirstOrDefault();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User admin = dbobj.Users.Find(id);

            if (admin == null)
            {
                return RedirectToAction("Error", "Home");
            }
            admin.IsActive = false;

            dbobj.Entry(admin).State = EntityState.Modified;
            dbobj.SaveChanges();

            TempData["success"] = userObj.FirstName + " " + userObj.LastName;
            TempData["message"] = "Administrator has been Successfully deleted";
            return RedirectToAction("Administrators", "Admin");

        }
    }
}