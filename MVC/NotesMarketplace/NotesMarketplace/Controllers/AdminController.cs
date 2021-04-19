using NotesMarketplace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Net;
using System.Data.Entity;
using NotesMarketplace.EmailTemplates;
using System.Data.Entity.Core.Objects;
using System.IO;
using NotesMarketplace.Password_Encryption;

namespace NotesMarketplace.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class AdminController : Controller
    {
        private readonly NoteMarketplaceEntities dbobj = new NoteMarketplaceEntities();

        // GET: Admin
        public ActionResult Index(string searchPublished, int? PublishedNotespage, string sortByForPublished, string SelectMonth)
        {
            var EmailID = User.Identity.Name.ToString();
            User userObj = dbobj.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();

            if (User.Identity.IsAuthenticated)
            {
                UserProfile Profilepic = dbobj.UserProfiles.Where(x => x.UserID == userObj.ID).FirstOrDefault();
                if (Profilepic != null)
                {
                    TempData["ProfilePicture"] = Profilepic.ProfilePicture;
                }
                else
                {
                    TempData["ProfilePicture"] = dbobj.SystemConfigurations.Where(x => x.Key.ToLower() == "defaultprofilepicture").Select(x=>x.Value).FirstOrDefault();
                }
            }

            var rangOfDate = DateTime.Now.AddDays(-7);

            ViewBag.LastDownloads = dbobj.Downloads.Where(x => x.AttacmentDownloadedDate > rangOfDate && x.IsSellerHasAllowedDownload == true && x.AttachmentPath != null && x.IsAttachementDownloaded == true).Count();
            ViewBag.Newregistration = dbobj.Users.Where(x => x.CreatedDate > rangOfDate && x.RoleID == dbobj.UserRoles.Where(u=>u.Name.ToLower() == "member").Select(u=>u.ID).FirstOrDefault()).Count();
            ViewBag.TotalInReviewNotes = dbobj.SellerNotes.Where(x => x.Status == dbobj.ReferenceDatas.Where(r => r.Value.ToLower() == "in review").Select(r => r.ID).FirstOrDefault()
                                                           || x.Status == dbobj.ReferenceDatas.Where(r => r.Value.ToLower() == "submitted for review").Select(r => r.ID).FirstOrDefault()).Count();

            ViewBag.SortCreatedDateParameterPublish = string.IsNullOrEmpty(sortByForPublished) ? "CreatedDate asc" : "";
            ViewBag.SortTitleParameterPublish = sortByForPublished == "Title" ? "Title desc" : "Title";
            ViewBag.SortCategoryParameterPublish = sortByForPublished == "Category" ? "Category desc" : "Category";
            ViewBag.SortSellTypeParameterPublish = sortByForPublished == "Type" ? "Type desc" : "Type";
            ViewBag.SortPriceParameterPublish = sortByForPublished == "Price" ? "Price desc" : "Price";
            ViewBag.SizeSortParamPublish = sortByForPublished == "Size" ? "Size_desc" : "Size";
            ViewBag.PublisherSortParamPublish = sortByForPublished == "Publisher" ? "Publisher_desc" : "Publisher";

            List<SellerNote> sellerNotesPublished = dbobj.SellerNotes.Where(x => x.IsActive == true && (x.Title.Contains(searchPublished) || x.NoteCategory.Name.Contains(searchPublished) ||x.SellingPrice.ToString().StartsWith(searchPublished)|| x.User.FirstName.Contains(searchPublished) || x.User.LastName.Contains(searchPublished)|| (x.PubilshedDate.Value.Day + "-" + x.PubilshedDate.Value.Month + "-" + x.PubilshedDate.Value.Year).Contains(searchPublished) || searchPublished == null)).ToList();
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
                                  where (r.Value == "Published" && (s.PubilshedDate.Value.ToString("MM") == SelectMonth
                                                                                           || string.IsNullOrEmpty(SelectMonth)))
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

            var now = DateTimeOffset.Now;
            var months = Enumerable.Range(1, 6).Select(i => new
            {
                A = now.AddMonths(-i + 1).ToString("MM").ToString(),
                B = now.AddMonths(-i + 1).ToString("MMMM").ToString()
            }).ToList();

            ViewBag.SelectMonth = new SelectList(months, "A", "B");

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
                return RedirectToAction("Error", "Home");
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

        public ActionResult DeleteReview(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SellerNotesReview notereview = dbobj.SellerNotesReviews.Find(id);

            if (notereview == null)
            {
                return HttpNotFound();
            }

            dbobj.SellerNotesReviews.Remove(notereview);
            dbobj.SaveChanges();

            return RedirectToAction("Index", "Admin");
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
                return RedirectToAction("Error", "Home");
            }

            User sellUser = dbobj.Users.Find(sellerNote.SellerID);

            if (sellUser == null)
            {
                return RedirectToAction("Error", "Home");
            }

            bool internet = CheckInternet.IsConnectedToInternet();
            if (internet == true)
            {

                sellerNote.Status = dbobj.ReferenceDatas.Where(x => x.RefCategory == "Notes Status" && x.Value.ToLower() == "removed").Select(x => x.ID).FirstOrDefault();
                sellerNote.ModifiedBy = userObj.ID;
                sellerNote.ModifiedDate = DateTime.Now;

                dbobj.Entry(sellerNote).State = EntityState.Modified;
                dbobj.SaveChanges();

                var SupportEmailAddress = dbobj.SystemConfigurations.Where(x => x.Key.ToLower() == "supportemailaddress").Select(y => y.Value).FirstOrDefault();
                var EmailPassword = dbobj.SystemConfigurations.Where(x => x.Key.ToLower() == "emailpassword").Select(y => y.Value).FirstOrDefault();

                NoteUnPublishedEmail.NoteUnPublishedNotifyEmail(SupportEmailAddress, EmailPassword, sellUser, sellerNote.Title, remarkObj.Remarks);
            }
            else
            {
                TempData["internetnotconnected"] = userObj.FirstName + " " + userObj.LastName;
                return RedirectToAction("Index", "Admin");
            }

            TempData["success"] = userObj.FirstName + " " + userObj.LastName;
            TempData["message"] = "Note has been unpublished";
            return RedirectToAction("Index", "Admin");
        }
    }
}