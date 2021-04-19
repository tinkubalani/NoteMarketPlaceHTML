using NotesMarketplace.Password_Encryption;
using NotesMarketplace.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NotesMarketplace.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    [RoutePrefix("Admin")]
    public class AdminChangePasswordController : Controller
    {
        private readonly NoteMarketplaceEntities dbobj = new NoteMarketplaceEntities();

        [Route("ChangePassword")]
        public ActionResult ChangePassword()
        {
            return View();
        }

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

                    User user = dbobj.Users.Where(x => x.EmailID == Emailid).FirstOrDefault();

                    var pwd = EncryptPassword.EncryptPasswordMd5(changePassword.OldPassword);

                    if (user.Password == pwd)
                    {
                        user.Password = EncryptPassword.EncryptPasswordMd5(changePassword.NewPassword);
                        user.ModifiedDate = DateTime.Now;
                        user.ModifiedBy = user.ID;

                        dbobj.Entry(user).State = EntityState.Modified;
                        dbobj.SaveChanges();

                        return RedirectToAction("Index", "Admin");
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
    }
}