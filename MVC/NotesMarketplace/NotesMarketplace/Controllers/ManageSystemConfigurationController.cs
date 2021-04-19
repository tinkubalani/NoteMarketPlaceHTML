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
    [Authorize(Roles = "SuperAdmin")]
    [RoutePrefix("Admin")]
    public class ManageSystemConfigurationController : Controller
    {
        private readonly NoteMarketplaceEntities dbobj = new NoteMarketplaceEntities();

        [Route("ManageSystemConfiguration")]
        public ActionResult ManageSystemConfiguration()
        {
            AddSystemConfiguration systemConfiguration = new AddSystemConfiguration();

            var Records = dbobj.SystemConfigurations.Count();
            if (Records > 0)
            {
                AddSystemConfiguration SA = new AddSystemConfiguration
                {
                    SupportEmailAddress = dbobj.SystemConfigurations.Where(x => x.Key.ToLower() == "supportemailaddress").Select(y => y.Value).FirstOrDefault(),
                    EmailPassword = dbobj.SystemConfigurations.Where(x => x.Key.ToLower() == "emailpassword").Select(y => y.Value).FirstOrDefault(),
                    SupportPhoneNumber = dbobj.SystemConfigurations.Where(x => x.Key.ToLower() == "supportphonenumber").Select(y => y.Value).FirstOrDefault(),
                    EmailAddress = dbobj.SystemConfigurations.Where(x => x.Key.ToLower() == "emailaddress").Select(y => y.Value).FirstOrDefault(),
                    FacebookURL = dbobj.SystemConfigurations.Where(x => x.Key.ToLower() == "facebookurl").Select(y => y.Value).FirstOrDefault(),
                    LinkedinURL = dbobj.SystemConfigurations.Where(x => x.Key.ToLower() == "linkedinurl").Select(y => y.Value).FirstOrDefault(),
                    TwitterURL = dbobj.SystemConfigurations.Where(x => x.Key.ToLower() == "twitterurl").Select(y => y.Value).FirstOrDefault(),
                };
                return View(SA);
            }
            return View();
        }

        [Route("ManageSystemConfiguration")]
        [HttpPost]
        public ActionResult ManageSystemConfiguration(AddSystemConfiguration addSystemConfiguration)
        {
            if (ModelState.IsValid)
            {
                var EmailID = User.Identity.Name.ToString();
                User userObj = dbobj.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();

                var Records = dbobj.SystemConfigurations.Count();
                if (Records > 0)
                {
                    SystemConfiguration updatedetail1 = dbobj.SystemConfigurations.Where(x => x.Key.ToLower() == "supportemailaddress").FirstOrDefault();
                    updatedetail1.Value = addSystemConfiguration.SupportEmailAddress;
                    updatedetail1.ModifiedBy = userObj.ID;
                    updatedetail1.ModifiedDate = DateTime.Now;
                    dbobj.Entry(updatedetail1).State = EntityState.Modified;
                    dbobj.SaveChanges();

                    SystemConfiguration updatedetail10 = dbobj.SystemConfigurations.Where(x => x.Key.ToLower() == "emailpassword").FirstOrDefault();
                    updatedetail10.Value = addSystemConfiguration.EmailPassword;
                    updatedetail10.ModifiedBy = userObj.ID;
                    updatedetail10.ModifiedDate = DateTime.Now;
                    dbobj.Entry(updatedetail10).State = EntityState.Modified;
                    dbobj.SaveChanges();

                    SystemConfiguration updatedetail2 = dbobj.SystemConfigurations.Where(x => x.Key.ToLower() == "SupportPhoneNumber").FirstOrDefault();
                    updatedetail2.Value = addSystemConfiguration.SupportPhoneNumber;
                    updatedetail2.ModifiedBy = userObj.ID;
                    updatedetail2.ModifiedDate = DateTime.Now;
                    dbobj.Entry(updatedetail2).State = EntityState.Modified;
                    dbobj.SaveChanges();

                    SystemConfiguration updatedetail3 = dbobj.SystemConfigurations.Where(x => x.Key.ToLower() == "emailaddress").FirstOrDefault();
                    updatedetail3.Value = addSystemConfiguration.EmailAddress;
                    updatedetail3.ModifiedBy = userObj.ID;
                    updatedetail3.ModifiedDate = DateTime.Now;
                    dbobj.Entry(updatedetail3).State = EntityState.Modified;
                    dbobj.SaveChanges();

                    SystemConfiguration updatedetail4 = dbobj.SystemConfigurations.Where(x => x.Key.ToLower() == "facebookurl").FirstOrDefault();
                    updatedetail4.Value = addSystemConfiguration.FacebookURL;
                    updatedetail4.ModifiedBy = userObj.ID;
                    updatedetail4.ModifiedDate = DateTime.Now;
                    dbobj.Entry(updatedetail4).State = EntityState.Modified;
                    dbobj.SaveChanges();

                    SystemConfiguration updatedetail5 = dbobj.SystemConfigurations.Where(x => x.Key.ToLower() == "linkedinurl").FirstOrDefault();
                    updatedetail5.Value = addSystemConfiguration.LinkedinURL;
                    updatedetail5.ModifiedBy = userObj.ID;
                    updatedetail5.ModifiedDate = DateTime.Now;
                    dbobj.Entry(updatedetail5).State = EntityState.Modified;
                    dbobj.SaveChanges();

                    SystemConfiguration updatedetail6 = dbobj.SystemConfigurations.Where(x => x.Key.ToLower() == "twitterurl").FirstOrDefault();
                    updatedetail6.Value = addSystemConfiguration.TwitterURL;
                    updatedetail6.ModifiedBy = userObj.ID;
                    updatedetail6.ModifiedDate = DateTime.Now;
                    dbobj.Entry(updatedetail6).State = EntityState.Modified;
                    dbobj.SaveChanges();

                    SystemConfiguration updatedetail7 = dbobj.SystemConfigurations.Where(x => x.Key.ToLower() == "defaultimagefornotes").FirstOrDefault();
                    updatedetail7.ModifiedBy = userObj.ID;
                    updatedetail7.ModifiedDate = DateTime.Now;
                    string storepath = Path.Combine(Server.MapPath("~/SystemConfigurations/"), "DefaultImages");
                    System.IO.DirectoryInfo di = new DirectoryInfo(storepath);

                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                    // Check for Directory, If not exist, then create it  
                    if (!Directory.Exists(storepath))
                    {
                        Directory.CreateDirectory(storepath);
                    }

                    if (addSystemConfiguration.DefaultImageForNotes != null && addSystemConfiguration.DefaultImageForNotes.ContentLength > 0)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(addSystemConfiguration.DefaultImageForNotes.FileName);
                        string extension = Path.GetExtension(addSystemConfiguration.DefaultImageForNotes.FileName);
                        fileName = "DefaultNoteImage" + extension;
                        string finalpath = Path.Combine(storepath, fileName);
                        addSystemConfiguration.DefaultImageForNotes.SaveAs(finalpath);

                        updatedetail7.Value = Path.Combine("/SystemConfigurations/DefaultImages/", fileName);
                    }
                    dbobj.Entry(updatedetail7).State = EntityState.Modified;
                    dbobj.SaveChanges();

                    SystemConfiguration updatedetail8 = dbobj.SystemConfigurations.Where(x => x.Key.ToLower() == "defaultprofilepicture").FirstOrDefault();
                    updatedetail8.ModifiedBy = userObj.ID;
                    updatedetail8.ModifiedDate = DateTime.Now;
                    if (addSystemConfiguration.DefaultProfilePicture != null && addSystemConfiguration.DefaultProfilePicture.ContentLength > 0)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(addSystemConfiguration.DefaultProfilePicture.FileName);
                        string extension = Path.GetExtension(addSystemConfiguration.DefaultProfilePicture.FileName);
                        fileName = "DefaultUserImage" + extension;
                        string finalpath = Path.Combine(storepath, fileName);
                        addSystemConfiguration.DefaultProfilePicture.SaveAs(finalpath);

                        updatedetail8.Value = Path.Combine("/SystemConfigurations/DefaultImages/", fileName);
                    }
                    dbobj.Entry(updatedetail8).State = EntityState.Modified;
                    dbobj.SaveChanges();

                }
                else
                {

                    SystemConfiguration obj1 = new SystemConfiguration
                    {
                        Key = "SupportEmailAddress",
                        Value = addSystemConfiguration.SupportEmailAddress,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        CreatedBy = userObj.ID,
                        ModifiedBy = userObj.ID,
                        IsActive = true
                    };
                    dbobj.SystemConfigurations.Add(obj1);
                    dbobj.SaveChanges();

                    SystemConfiguration obj9 = new SystemConfiguration
                    {
                        Key = "EmailPassword",
                        Value = addSystemConfiguration.EmailPassword,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        CreatedBy = userObj.ID,
                        ModifiedBy = userObj.ID,
                        IsActive = true
                    };
                    dbobj.SystemConfigurations.Add(obj9);
                    dbobj.SaveChanges();

                    SystemConfiguration obj2 = new SystemConfiguration
                    {
                        Key = "SupportPhoneNumber",
                        Value = addSystemConfiguration.SupportPhoneNumber,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        CreatedBy = userObj.ID,
                        ModifiedBy = userObj.ID,
                        IsActive = true
                    };
                    dbobj.SystemConfigurations.Add(obj2);
                    dbobj.SaveChanges();

                    SystemConfiguration obj3 = new SystemConfiguration
                    {
                        Key = "EmailAddress",
                        Value = addSystemConfiguration.EmailAddress,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        CreatedBy = userObj.ID,
                        ModifiedBy = userObj.ID,
                        IsActive = true
                    };
                    dbobj.SystemConfigurations.Add(obj3);
                    dbobj.SaveChanges();

                    SystemConfiguration obj4 = new SystemConfiguration
                    {
                        Key = "FacebookURL",
                        Value = addSystemConfiguration.FacebookURL,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        CreatedBy = userObj.ID,
                        ModifiedBy = userObj.ID,
                        IsActive = true
                    };
                    dbobj.SystemConfigurations.Add(obj4);
                    dbobj.SaveChanges();

                    SystemConfiguration obj5 = new SystemConfiguration
                    {
                        Key = "LinkedinURL",
                        Value = addSystemConfiguration.LinkedinURL,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        CreatedBy = userObj.ID,
                        ModifiedBy = userObj.ID,
                        IsActive = true
                    };
                    dbobj.SystemConfigurations.Add(obj5);
                    dbobj.SaveChanges();

                    SystemConfiguration obj6 = new SystemConfiguration
                    {
                        Key = "TwitterURL",
                        Value = addSystemConfiguration.TwitterURL,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        CreatedBy = userObj.ID,
                        ModifiedBy = userObj.ID,
                        IsActive = true
                    };
                    dbobj.SystemConfigurations.Add(obj6);
                    dbobj.SaveChanges();


                    string storepath = Path.Combine(Server.MapPath("~/SystemConfigurations/"), "DefaultImages");

                    // Check for Directory, If not exist, then create it  
                    if (!Directory.Exists(storepath))
                    {
                        Directory.CreateDirectory(storepath);
                    }

                    SystemConfiguration obj7 = new SystemConfiguration
                    {
                        Key = "DefaultImageForNotes",
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        CreatedBy = userObj.ID,
                        ModifiedBy = userObj.ID,
                        IsActive = true
                    };

                    if (addSystemConfiguration.DefaultImageForNotes != null && addSystemConfiguration.DefaultImageForNotes.ContentLength > 0)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(addSystemConfiguration.DefaultImageForNotes.FileName);
                        string extension = Path.GetExtension(addSystemConfiguration.DefaultImageForNotes.FileName);
                        fileName = "DefaultNoteImage" + extension;
                        string finalpath = Path.Combine(storepath, fileName);
                        addSystemConfiguration.DefaultImageForNotes.SaveAs(finalpath);

                        obj7.Value = Path.Combine("/SystemConfigurations/DefaultImages/", fileName);
                    }

                    dbobj.SystemConfigurations.Add(obj7);
                    dbobj.SaveChanges();

                    SystemConfiguration obj8 = new SystemConfiguration
                    {
                        Key = "DefaultProfilePicture",
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        CreatedBy = userObj.ID,
                        ModifiedBy = userObj.ID,
                        IsActive = true
                    };

                    if (addSystemConfiguration.DefaultProfilePicture != null && addSystemConfiguration.DefaultProfilePicture.ContentLength > 0)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(addSystemConfiguration.DefaultProfilePicture.FileName);
                        string extension = Path.GetExtension(addSystemConfiguration.DefaultProfilePicture.FileName);
                        fileName = "DefaultUserImage" + extension;
                        string finalpath = Path.Combine(storepath, fileName);
                        addSystemConfiguration.DefaultProfilePicture.SaveAs(finalpath);

                        obj8.Value = Path.Combine("/SystemConfigurations/DefaultImages/", fileName);
                    }

                    dbobj.SystemConfigurations.Add(obj8);
                    dbobj.SaveChanges();

                }

                TempData["success"] = userObj.FirstName + " " + userObj.LastName;
                TempData["message"] = "System Configuration updated";
                return RedirectToAction("Index", "Admin");
            }
            return View();
        }
    }
}