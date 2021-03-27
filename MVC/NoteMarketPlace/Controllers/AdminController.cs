using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Net;
using System.Data.Entity;
using NoteMarketPlace.EmailTemplates;

namespace NoteMarketPlace.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly NoteMarketplaceEntities dbobj = new NoteMarketplaceEntities();

        // GET: Admin
        public ActionResult Index(string searchPublished, int? PublishedNotespage, string sortByForPublished)
        {
            var EmailID = User.Identity.Name.ToString();
            User userObj = dbobj.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();

            ViewBag.SortCreatedDateParameterPublish = string.IsNullOrEmpty(sortByForPublished) ? "CreatedDate asc" : "";
            ViewBag.SortTitleParameterPublish = sortByForPublished == "Title" ? "Title desc" : "Title";
            ViewBag.SortCategoryParameterPublish = sortByForPublished == "Category" ? "Category desc" : "Category";
            ViewBag.SortSellTypeParameterPublish = sortByForPublished == "Type" ? "Type desc" : "Type";
            ViewBag.SortPriceParameterPublish = sortByForPublished == "Price" ? "Price desc" : "Price";
            ViewBag.SizeSortParamPublish = sortByForPublished == "Size" ? "Size_desc" : "Size";
            ViewBag.PublisherSortParamPublish = sortByForPublished == "Publisher" ? "Publisher_desc" : "Publisher";

            List<SellerNote> sellerNotesPublished = dbobj.SellerNotes.Where(x => x.IsActive == true && (x.Title.Contains(searchPublished) || searchPublished == null)).ToList();
            List<NoteCategory> noteCategoriesPublished = dbobj.NoteCategories.ToList();
            List<ReferenceData> referenceDatasPublished = dbobj.ReferenceDatas.ToList();
            List<Download> DownloadNotes = dbobj.Downloads.ToList();
            List<User> UserData = dbobj.Users.ToList();
            List<SellerNotesAttachement> attachmentDetails = dbobj.SellerNotesAttachements.ToList();

            var PublishedNotes = (from s in sellerNotesPublished
                                  join c in noteCategoriesPublished on s.Category equals c.ID into table1
                                  from c in table1.ToList()
                                  join r in referenceDatasPublished on s.Status equals r.ID into table2
                                  from r in table2.ToList()
                                  join userdata in UserData on s.SellerID equals userdata.ID into table3
                                  from userdata in table3.ToList()
                                  join ad in attachmentDetails on s.ID equals ad.NoteID into table4
                                  from ad in table4.ToList()
                                  where (r.Value == "Published")
                                  select new AllPublishedNoteViewModel
                                  {
                                      SellerNotes = s,
                                      NoteCategories = c,
                                      ReferenceDatas = r,
                                      UserData = userdata,
                                      Attachment = ad

                                  }).AsQueryable();

            switch (sortByForPublished)
            {
                case "CreatedDate asc":
                    PublishedNotes = PublishedNotes.OrderBy(x => x.SellerNotes.PubilshedDate);
                    break;
                case "Title desc":
                    PublishedNotes = PublishedNotes.OrderByDescending(x => x.SellerNotes.Title);
                    break;
                case "Title":
                    PublishedNotes = PublishedNotes.OrderBy(x => x.SellerNotes.Title);
                    break;
                case "Category desc":
                    PublishedNotes = PublishedNotes.OrderByDescending(x => x.NoteCategories.Name);
                    break;
                case "Category":
                    PublishedNotes = PublishedNotes.OrderBy(x => x.NoteCategories.Name);
                    break;
                case "Size_desc":
                    PublishedNotes = PublishedNotes.OrderByDescending(x => x.Attachment.AttachementSize);
                    break;
                case "Size":
                    PublishedNotes = PublishedNotes.OrderBy(x => x.Attachment.AttachementSize);
                    break;
                case "Type desc":
                    PublishedNotes = PublishedNotes.OrderByDescending(x => x.SellerNotes.IsPaid);
                    break;
                case "Type":
                    PublishedNotes = PublishedNotes.OrderBy(x => x.SellerNotes.IsPaid);
                    break;
                case "Price desc":
                    PublishedNotes = PublishedNotes.OrderByDescending(x => x.SellerNotes.SellingPrice);
                    break;
                case "Price":
                    PublishedNotes = PublishedNotes.OrderBy(x => x.SellerNotes.SellingPrice);
                    break;
                case "Publisher_desc":
                    PublishedNotes = PublishedNotes.OrderByDescending(x => x.UserData.FirstName);
                    break;
                case "Publisher":
                    PublishedNotes = PublishedNotes.OrderBy(x => x.UserData.FirstName);
                    break;
                default:
                    PublishedNotes = PublishedNotes.OrderByDescending(x => x.SellerNotes.PubilshedDate);
                    break;
            }

            ViewBag.PublishedNotes = PublishedNotes.ToList().ToPagedList(PublishedNotespage ?? 1, 5);
            return View();
        }


        public ActionResult NoteDetail(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SellerNote sellerNotes = dbobj.SellerNotes.Find(id);
            if (sellerNotes == null)
            {
                return HttpNotFound();
            }

            NoteCategory noteCategory = dbobj.NoteCategories.Find(sellerNotes.Category);
            ViewBag.Category = noteCategory.Name;
            if (sellerNotes.Country == null)
            {
                ViewBag.Country = null;
            }
            else
            {
                Country country = dbobj.Countries.Find(sellerNotes.Country);
                ViewBag.Country = country.Name;
            }
            return View(sellerNotes);
        }

        [HttpPost]
        public ActionResult UnPublishNote(int? id, AdminUnPublishNote remarkObj)
        {
            var EmailID = User.Identity.Name.ToString();
            User userObj = dbobj.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SellerNote sellerNote = dbobj.SellerNotes.Find(id);

            if (sellerNote == null)
            {
                return HttpNotFound();
            }

            User sellUser = dbobj.Users.Find(sellerNote.SellerID);

            if (sellUser == null)
            {
                return HttpNotFound();
            }

            sellerNote.Status = dbobj.ReferenceDatas.Where(x => x.RefCategory == "Notes Status" && x.Value.ToLower() == "removed").Select(x => x.ID).FirstOrDefault();
            sellerNote.ModifiedBy = userObj.ID;
            sellerNote.ModifiedDate = DateTime.Now;

            dbobj.Entry(sellerNote).State = EntityState.Modified;
            dbobj.SaveChanges();

            NoteUnPublishedEmail.NoteUnPublishedNotifyEmail(sellUser,sellerNote.Title,remarkObj.Remarks);

            return RedirectToAction("Index", "Admin");
        }
    }
}