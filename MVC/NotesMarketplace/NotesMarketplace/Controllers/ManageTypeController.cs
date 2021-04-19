using NotesMarketplace.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace NotesMarketplace.Controllers
{

    [Authorize(Roles = "Admin,SuperAdmin")]
    [RoutePrefix("Admin")]
    public class ManageTypeController : Controller
    {
        private readonly NoteMarketplaceEntities dbobj = new NoteMarketplaceEntities();

        [Route("ManageType")]
        public ActionResult ManageType(int? page, string Search, string SortOrder)
        {
            ViewBag.DateSortParam = string.IsNullOrEmpty(SortOrder) ? "CreatedDate_asc" : "";
            ViewBag.NameSortParam = SortOrder == "Name" ? "Name_desc" : "Name";
            ViewBag.DescriptionSortParam = SortOrder == "Description" ? "Description_desc" : "Description";
            ViewBag.AddedBySortParam = SortOrder == "AddedBy" ? "AddedBy_desc" : "AddedBy";

            var type = dbobj.NoteTypes.Where(x => (x.Name.Contains(Search) || x.Description.Contains(Search) || x.User.FirstName.Contains(Search) || x.User.LastName.Contains(Search) || (x.ModifiedDate.Value.Day + "-" + x.ModifiedDate.Value.Month + "-" + x.ModifiedDate.Value.Year).Contains(Search) || Search == null)).AsQueryable();
            ViewBag.Users = dbobj.Users.ToList();

            switch (SortOrder)
            {
                case "CreatedDate_asc":
                    type = type.OrderBy(x => x.ModifiedDate);
                    break;
                case "Name_desc":
                    type = type.OrderByDescending(x => x.Name);
                    break;
                case "Name":
                    type = type.OrderBy(x => x.Name);
                    break;
                case "Description_desc":
                    type = type.OrderByDescending(x => x.Description);
                    break;
                case "Description":
                    type = type.OrderBy(x => x.Description);
                    break;
                case "AddedBy_desc":
                    type = type.OrderByDescending(x => x.ModifiedBy);
                    break;
                case "AddedBy":
                    type = type.OrderBy(x => x.ModifiedBy);
                    break;
                default:
                    type = type.OrderByDescending(x => x.ModifiedDate);
                    break;
            }

            return View(type.ToList().ToPagedList(page ?? 1, 5));
        }

        [Route("AddType")]
        [HttpGet]
        public ActionResult AddType()
        {
            return View();
        }

        [Route("AddType")]
        [HttpPost]
        public ActionResult AddType(AddType addtype)
        {
            if (ModelState.IsValid)
            {
                var EmailID = User.Identity.Name.ToString();
                User userObj = dbobj.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();

                NoteType notetype = new NoteType()
                {
                    Name = addtype.Name,
                    Description = addtype.Description,
                    CreatedBy = userObj.ID,
                    ModifiedBy = userObj.ID,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    IsActive = true
                };
                dbobj.NoteTypes.Add(notetype);
                dbobj.SaveChanges();

                TempData["success"] = userObj.FirstName + " " + userObj.LastName;
                TempData["message"] = "Type has been Successfully added";
                return RedirectToAction("ManageType", "Admin");
            }
            return View();
        }

        [Route("EditType/{id}")]
        [HttpGet]
        public ActionResult EditType(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            NoteType notetype = dbobj.NoteTypes.Find(id);

            if (notetype == null)
            {
                return RedirectToAction("Error", "Home");
            }

            AddType addtype = new AddType
            {
                Name = notetype.Name,
                Description = notetype.Description,
                TypeID = notetype.ID
            };

            return View(addtype);
        }

        [Route("EditType")]
        [HttpPost]
        public ActionResult EditType(AddType addtype)
        {
            if (ModelState.IsValid)
            {
                var EmailID = User.Identity.Name.ToString();
                User userObj = dbobj.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();
                NoteType notetype = dbobj.NoteTypes.Where(x => x.ID == addtype.TypeID).FirstOrDefault();
                notetype.Name = addtype.Name;
                notetype.Description = addtype.Description;
                notetype.ModifiedBy = userObj.ID;
                notetype.ModifiedDate = DateTime.Now;

                dbobj.Entry(notetype).State = EntityState.Modified;
                dbobj.SaveChanges();

                TempData["success"] = userObj.FirstName + " " + userObj.LastName;
                TempData["message"] = "Type has been Successfully edited";
                return RedirectToAction("ManageType", "Admin");
            }
            return View();
        }

        [Route("DeleteType/{id}")]
        [HttpGet]
        public ActionResult DeleteType(int? id)
        {

            var EmailID = User.Identity.Name.ToString();
            User userObj = dbobj.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NoteType notetype = dbobj.NoteTypes.Find(id);

            if (notetype == null)
            {
                return RedirectToAction("Error", "Home");
            }

            notetype.IsActive = false;

            dbobj.Entry(notetype).State = EntityState.Modified;
            dbobj.SaveChanges();

            TempData["success"] = userObj.FirstName + " " + userObj.LastName;
            TempData["message"] = "Type has been Successfully deleted";
            return RedirectToAction("ManageType", "Admin");

        }
    }
}