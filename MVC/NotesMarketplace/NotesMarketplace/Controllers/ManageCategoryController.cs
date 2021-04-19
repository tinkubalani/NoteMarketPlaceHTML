using NotesMarketplace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Net;
using System.Data.Entity;

namespace NotesMarketplace.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    [RoutePrefix("Admin")]
    public class ManageCategoryController : Controller
    {

        private readonly NoteMarketplaceEntities dbobj = new NoteMarketplaceEntities();

        [Route("ManageCategory")]
        public ActionResult ManageCategory(int? page, string Search, string SortOrder)
        {
            ViewBag.DateSortParam = string.IsNullOrEmpty(SortOrder) ? "CreatedDate_asc" : "";
            ViewBag.NameSortParam = SortOrder == "Name" ? "Name_desc" : "Name";
            ViewBag.DescriptionSortParam = SortOrder == "Description" ? "Description_desc" : "Description";
            ViewBag.AddedBySortParam = SortOrder == "AddedBy" ? "AddedBy_desc" : "AddedBy";

            var category = dbobj.NoteCategories.Where(x =>(x.Name.Contains(Search) || x.Description.Contains(Search) || x.User.FirstName.Contains(Search) || x.User.LastName.Contains(Search) || (x.ModifiedDate.Value.Day + "-" + x.ModifiedDate.Value.Month + "-" + x.ModifiedDate.Value.Year).Contains(Search) || Search == null)).AsQueryable();
            ViewBag.Users = dbobj.Users.ToList();

            switch (SortOrder)
            {
                case "CreatedDate_asc":
                    category = category.OrderBy(x => x.ModifiedDate);
                    break;
                case "Name_desc":
                    category = category.OrderByDescending(x => x.Name);
                    break;
                case "Name":
                    category = category.OrderBy(x => x.Name);
                    break;
                case "Description_desc":
                    category = category.OrderByDescending(x => x.Description);
                    break;
                case "Description":
                    category = category.OrderBy(x => x.Description);
                    break;
                case "AddedBy_desc":
                    category = category.OrderByDescending(x => x.ModifiedBy);
                    break;
                case "AddedBy":
                    category = category.OrderBy(x => x.ModifiedBy);
                    break;
                default:
                    category = category.OrderByDescending(x => x.ModifiedDate);
                    break;
            }

            return View(category.ToList().ToPagedList(page ?? 1, 5));
        }

        [Route("AddCategory")]
        [HttpGet]
        public ActionResult AddCategory()
        {
            return View();
        }

        [Route("AddCategory")]
        [HttpPost]
        public ActionResult AddCategory(AddCategory addcategory)
        {
            if (ModelState.IsValid)
            {
                var EmailID = User.Identity.Name.ToString();
                User userObj = dbobj.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();

                NoteCategory notecategory = new NoteCategory()
                {
                    Name = addcategory.Name,
                    Description = addcategory.Description,
                    CreatedBy = userObj.ID,
                    ModifiedBy = userObj.ID,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    IsActive = true
                };
                dbobj.NoteCategories.Add(notecategory);
                dbobj.SaveChanges();

                TempData["success"] = userObj.FirstName + " " + userObj.LastName;
                TempData["message"] = "Category has been Successfully added";
                return RedirectToAction("ManageCategory", "Admin");
            }
            return View();
        }

        [Route("EditCategory/{id}")]
        [HttpGet]
        public ActionResult EditCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            NoteCategory notecategory = dbobj.NoteCategories.Find(id);

            if (notecategory == null)
            {
                return RedirectToAction("Error", "Home");
            }

            AddCategory addcategory = new AddCategory
            {
                Name = notecategory.Name,
                Description = notecategory.Description,
                CategoryID = notecategory.ID
            };

            return View(addcategory);
        }

        [Route("EditCategory")]
        [HttpPost]
        public ActionResult EditCategory(AddCategory addcategory)
        {
            if (ModelState.IsValid)
            {
                var EmailID = User.Identity.Name.ToString();
                User userObj = dbobj.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();
                NoteCategory notecategory = dbobj.NoteCategories.Where(x => x.ID == addcategory.CategoryID).FirstOrDefault();
                notecategory.Name = addcategory.Name;
                notecategory.Description = addcategory.Description;
                notecategory.ModifiedBy = userObj.ID;
                notecategory.ModifiedDate = DateTime.Now;

                dbobj.Entry(notecategory).State = EntityState.Modified;
                dbobj.SaveChanges();

                TempData["success"] = userObj.FirstName + " " + userObj.LastName;
                TempData["message"] = "Category has been Successfully Edited";
                return RedirectToAction("ManageCategory", "Admin");
            }
            return View();
        }

        [Route("DeleteCategory/{id}")]
        [HttpGet]
        public ActionResult DeleteCategory(int? id)
        {
            var Emailid = User.Identity.Name.ToString();
            User userObj = dbobj.Users.Where(x => x.EmailID == Emailid).FirstOrDefault();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NoteCategory notecategory = dbobj.NoteCategories.Find(id);

            if (notecategory == null)
            {
                return RedirectToAction("Error", "Home");
            }

            notecategory.IsActive = false;

            dbobj.Entry(notecategory).State = EntityState.Modified;
            dbobj.SaveChanges();

            TempData["success"] = userObj.FirstName + " " + userObj.LastName;
            TempData["message"] = "Category has been Successfully deleted";
            return RedirectToAction("ManageCategory", "Admin");

        }
    }
}