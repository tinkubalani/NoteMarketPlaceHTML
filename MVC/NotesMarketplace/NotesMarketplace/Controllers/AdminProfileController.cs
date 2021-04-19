using NotesMarketplace.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NotesMarketplace.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    [RoutePrefix("Admin")]
    public class AdminProfileController : Controller
    {
        private readonly NoteMarketplaceEntities dbobj = new NoteMarketplaceEntities();

        [Route("MyProfile")]
        public ActionResult MyProfile()
        {
            var Emailid = User.Identity.Name.ToString();
            User user = dbobj.Users.Where(x => x.EmailID == Emailid).FirstOrDefault();

            if (User.Identity.IsAuthenticated)
            {
                UserProfile Profilepic = dbobj.UserProfiles.Where(x => x.UserID == user.ID).FirstOrDefault();
                if (Profilepic != null)
                {
                    TempData["ProfilePicture"] = Profilepic.ProfilePicture;
                }
                else
                {
                    TempData["ProfilePicture"] = dbobj.SystemConfigurations.Where(x => x.Key.ToLower() == "defaultprofilepicture").Select(x => x.Value).FirstOrDefault();
                }
            }

            if (dbobj.UserProfiles.Any(x => x.UserID == user.ID))
            {
                UserProfile userProfile = dbobj.UserProfiles.Where(x => x.UserID == user.ID).FirstOrDefault();

                AdminProfile userProfile1 = new AdminProfile
                {
                    ID = user.ID,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    EmailID = user.EmailID,
                    PhoneNumberCountryCode = userProfile.PhoneNumberCountryCode,
                    PhoneNumber = userProfile.PhoneNumber,
                    SecondaryEmail = userProfile.SecondaryEmailAddress

                };

                ViewBag.ProfilePicture = userProfile.ProfilePicture;
                ViewBag.PhoneNumberCountryCode = new SelectList(dbobj.Countries.Distinct().Where(x => x.IsActive == true), "CountryCode", "CountryCode", userProfile1.PhoneNumberCountryCode);
                return View(userProfile1);
            }
            else
            {
                AdminProfile userProfile = new AdminProfile
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    EmailID = user.EmailID
                };
                ViewBag.PhoneNumberCountryCode = new SelectList(dbobj.Countries.Distinct().Where(x => x.IsActive == true), "CountryCode", "CountryCode");
                return View(userProfile);
            }
        }

        [Route("MyProfile")]
        [HttpPost]
        public ActionResult MyProfile(AdminProfile userProfile)
        {
            if (ModelState.IsValid)
            {
                var Emailid = User.Identity.Name.ToString();
                User user = dbobj.Users.Where(x => x.EmailID == Emailid).FirstOrDefault();

                if (dbobj.UserProfiles.Any(x => x.UserID == user.ID))
                {
                    UserProfile userProfileObj = dbobj.UserProfiles.Where(x => x.UserID == user.ID).FirstOrDefault();
                    userProfileObj.PhoneNumberCountryCode = userProfile.PhoneNumberCountryCode;
                    userProfileObj.PhoneNumber = userProfile.PhoneNumber;
                    userProfileObj.ModifiedBy = user.ID;
                    userProfileObj.ModifiedDate = DateTime.Now;
                    userProfileObj.SecondaryEmailAddress = userProfile.SecondaryEmail;

                    dbobj.Entry(userProfileObj).State = EntityState.Modified;
                    dbobj.SaveChanges();

                    user.FirstName = userProfile.FirstName;
                    user.LastName = userProfile.LastName;
                    dbobj.Entry(user).State = EntityState.Modified;
                    dbobj.SaveChanges();

                    string path = Path.Combine(Server.MapPath("~/Members"), user.ID.ToString());

                    // Check for Directory, If not exist, then create it  
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    if (userProfile.ProfilePicture != null && userProfile.ProfilePicture.ContentLength > 0)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(userProfile.ProfilePicture.FileName);
                        string extension = Path.GetExtension(userProfile.ProfilePicture.FileName);
                        fileName = "DP_" + DateTime.Now.ToString("ddMMyyyy") + extension;
                        string finalpath = Path.Combine(path, fileName);
                        userProfile.ProfilePicture.SaveAs(finalpath);

                        userProfileObj.ProfilePicture = Path.Combine(("/Members/" + user.ID + "/"), fileName);
                        dbobj.SaveChanges();
                    }
                    else
                    {
                        userProfileObj.ProfilePicture = dbobj.SystemConfigurations.Where(x => x.Key.ToLower() == "defaultprofilepicture").Select(x => x.Value).FirstOrDefault();
                        dbobj.SaveChanges();
                    }

                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    UserProfile userProfileObj = new UserProfile
                    {
                        UserID = user.ID,
                        PhoneNumberCountryCode = userProfile.PhoneNumberCountryCode,
                        PhoneNumber = userProfile.PhoneNumber,
                        CreatedDate = DateTime.Now,
                        CreatedBy = user.ID,
                        ModifiedDate = DateTime.Now,
                        ModifiedBy = user.ID,
                        SecondaryEmailAddress = userProfile.SecondaryEmail
                    };

                    dbobj.UserProfiles.Add(userProfileObj);
                    dbobj.SaveChanges();

                    string path = Path.Combine(Server.MapPath("~/Members"), user.ID.ToString());

                    // Check for Directory, If not exist, then create it  
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    if (userProfile.ProfilePicture != null && userProfile.ProfilePicture.ContentLength > 0)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(userProfile.ProfilePicture.FileName);
                        string extension = Path.GetExtension(userProfile.ProfilePicture.FileName);
                        fileName = "DP_" + DateTime.Now.ToString("ddMMyyyy") + extension;
                        string finalpath = Path.Combine(path, fileName);
                        userProfile.ProfilePicture.SaveAs(finalpath);

                        userProfileObj.ProfilePicture = Path.Combine(("/Members/" + user.ID + "/"), fileName);
                        dbobj.SaveChanges();
                    }
                    else
                    {
                        userProfileObj.ProfilePicture = dbobj.SystemConfigurations.Where(x => x.Key.ToLower() == "defaultprofilepicture").Select(x => x.Value).FirstOrDefault();
                        dbobj.SaveChanges();
                    }

                    user.FirstName = userProfile.FirstName;
                    user.LastName = userProfile.LastName;
                    dbobj.Entry(user).State = EntityState.Modified;
                    dbobj.SaveChanges();
                }

                return RedirectToAction("ManageSystemConfiguration", "Admin");
            }
            return View();
        }
    }
}