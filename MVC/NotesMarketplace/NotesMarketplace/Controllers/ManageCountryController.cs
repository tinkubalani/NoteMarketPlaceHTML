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
    public class ManageCountryController : Controller
    {
        private readonly NoteMarketplaceEntities dbobj = new NoteMarketplaceEntities();

        [Route("Countries")]
        public ActionResult Countries(int? page, string Search, string SortOrder)
        {
            ViewBag.DateSortParam = string.IsNullOrEmpty(SortOrder) ? "CreatedDate_asc" : "";
            ViewBag.NameSortParam = SortOrder == "Name" ? "Name_desc" : "Name";
            ViewBag.CountryCodeSortParam = SortOrder == "CountryCode" ? "CountryCode_desc" : "CountryCode";
            ViewBag.AddedBySortParam = SortOrder == "AddedBy" ? "AddedBy_desc" : "AddedBy";

            var country = dbobj.Countries.Where(x =>(x.Name.Contains(Search) || x.CountryCode.Contains(Search) || x.User.FirstName.Contains(Search) || x.User.LastName.Contains(Search) || (x.ModifiedDate.Value.Day + "-" + x.ModifiedDate.Value.Month + "-" + x.ModifiedDate.Value.Year).Contains(Search) || Search == null)).AsQueryable();
            ViewBag.Users = dbobj.Users.ToList();

            switch (SortOrder)
            {
                case "CreatedDate_asc":
                    country = country.OrderBy(x => x.ModifiedDate);
                    break;
                case "Name_desc":
                    country = country.OrderByDescending(x => x.Name);
                    break;
                case "Name":
                    country = country.OrderBy(x => x.Name);
                    break;
                case "CountryCode_desc":
                    country = country.OrderByDescending(x => x.CountryCode);
                    break;
                case "CountryCode":
                    country = country.OrderBy(x => x.CountryCode);
                    break;
                case "AddedBy_desc":
                    country = country.OrderByDescending(x => x.ModifiedBy);
                    break;
                case "AddedBy":
                    country = country.OrderBy(x => x.ModifiedBy);
                    break;
                default:
                    country = country.OrderByDescending(x => x.ModifiedDate);
                    break;
            }

            return View(country.ToList().ToPagedList(page ?? 1, 5));
        }

        [Route("Addcountry")]
        [HttpGet]
        public ActionResult Addcountry()
        {
            return View();
        }

        [Route("Addcountry")]
        [HttpPost]
        public ActionResult Addcountry(AddCountry addcountry)
        {
            if (ModelState.IsValid)
            {
                var EmailID = User.Identity.Name.ToString();
                User userObj = dbobj.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();

                Country notecountry = new Country()
                {
                    Name = addcountry.Name,
                    CountryCode = addcountry.CountryCode,
                    CreatedBy = userObj.ID,
                    ModifiedBy = userObj.ID,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    IsActive = true
                };
                dbobj.Countries.Add(notecountry);
                dbobj.SaveChanges();

                TempData["success"] = userObj.FirstName + " " + userObj.LastName;
                TempData["message"] = "country has been Successfully added";
                return RedirectToAction("Countries", "Admin");
            }
            return View();
        }

        [Route("EditCountry/{id}")]
        [HttpGet]
        public ActionResult EditCountry(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Country notecountry = dbobj.Countries.Find(id);

            if (notecountry == null)
            {
                return RedirectToAction("Error", "Home");
            }

            AddCountry addcountry = new AddCountry
            {
                Name = notecountry.Name,
                CountryCode = notecountry.CountryCode,
                CoutryID = notecountry.ID
            };

            return View(addcountry);
        }

        [Route("EditCountry")]
        [HttpPost]
        public ActionResult EditCountry(AddCountry addcountry)
        {
            if (ModelState.IsValid)
            {
                var EmailID = User.Identity.Name.ToString();
                User userObj = dbobj.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();
                Country notecountry = dbobj.Countries.Where(x => x.ID == addcountry.CoutryID).FirstOrDefault();
                notecountry.Name = addcountry.Name;
                notecountry.CountryCode = addcountry.CountryCode;
                notecountry.ModifiedBy = userObj.ID;
                notecountry.ModifiedDate = DateTime.Now;

                dbobj.Entry(notecountry).State = EntityState.Modified;
                dbobj.SaveChanges();

                TempData["success"] = userObj.FirstName + " " + userObj.LastName;
                TempData["message"] = "Country has been Successfully edited";
                return RedirectToAction("Countries", "Admin");
            }
            return View();
        }

        [Route("DeleteCountry/{id}")]
        [HttpGet]
        public ActionResult DeleteCountry(int? id)
        {

            var EmailID = User.Identity.Name.ToString();
            User userObj = dbobj.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Country notecountry = dbobj.Countries.Find(id);

            if (notecountry == null)
            {
                return RedirectToAction("Error", "Home");
            }

            notecountry.IsActive = false;

            dbobj.Entry(notecountry).State = EntityState.Modified;
            dbobj.SaveChanges();

            TempData["success"] = userObj.FirstName + " " + userObj.LastName;
            TempData["message"] = "Country has been Successfully deleted";
            return RedirectToAction("Countries", "Admin");

        }
    }
}