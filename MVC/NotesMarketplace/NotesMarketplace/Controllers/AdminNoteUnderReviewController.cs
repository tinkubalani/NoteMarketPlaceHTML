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
    public class AdminNoteUnderReviewController : Controller
    {
        private readonly NoteMarketplaceEntities dbobj = new NoteMarketplaceEntities();

        [Route("NotesUnderReview")]
        // GET: AdminNoteUnderReview
        public ActionResult NotesUnderReview(string SearchUnderReview, int? NotesUnderReviewspage, string SortOrderUnderReview, string SellerName)
        {
            var EmailID = User.Identity.Name.ToString();
            User userObj = dbobj.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();


            List<SellerNote> NoteTitlePublished = dbobj.SellerNotes.Where(x => x.IsActive == true && (x.Title.Contains(SearchUnderReview)|| x.NoteCategory.Name.Contains(SearchUnderReview) || x.User.FirstName.Contains(SearchUnderReview) || x.ReferenceData.Value.Contains(SearchUnderReview) || x.User.LastName.Contains(SearchUnderReview) || (x.ModifiedDate.Value.Day + "-" + x.ModifiedDate.Value.Month + "-" + x.ModifiedDate.Value.Year).Contains(SearchUnderReview) || SearchUnderReview == null)).ToList();
            List<NoteCategory> CategoryNamePublished = dbobj.NoteCategories.ToList();
            List<ReferenceData> StatusNamePublished = dbobj.ReferenceDatas.Where(x => x.RefCategory == "Notes Status" && x.Value != "Rejected" && x.Value != "Removed" && x.IsActive == true).ToList();
            List<User> UserDetails = dbobj.Users.ToList();


            ViewBag.DateSortParamPublish = string.IsNullOrEmpty(SortOrderUnderReview) ? "ModifiedDate_asc" : "";
            ViewBag.TitleSortParamPublish = SortOrderUnderReview == "Title" ? "Title_desc" : "Title";
            ViewBag.CategorySortParamPublish = SortOrderUnderReview == "Category" ? "Category_desc" : "Category";
            ViewBag.SellerSortParamPublish = SortOrderUnderReview == "Seller" ? "Seller_desc" : "Seller";
            ViewBag.StatusSortParamPublish = SortOrderUnderReview == "Status" ? "Status_desc" : "Status";

            var NotesUnderReview = (from nt in NoteTitlePublished
                                    join cn in CategoryNamePublished on nt.Category equals cn.ID into table1
                                    from cn in table1.ToList()
                                    join sn in StatusNamePublished on nt.Status equals sn.ID into table2
                                    from sn in table2.ToList()
                                    join us in UserDetails on nt.SellerID equals us.ID into table3
                                    from us in table3.ToList()
                                    where ((sn.Value == "Submitted For Review" || sn.Value == "In Review") && ((us.FirstName + us.LastName) == SellerName || string.IsNullOrEmpty(SellerName)))
                                    select new UnderReviewsNote
                                    {
                                        NoteDetails = nt,
                                        Category = cn,
                                        Status = sn,
                                        User = us,

                                    }).AsQueryable();

            switch (SortOrderUnderReview)
            {
                case "ModifiedDate_asc":
                    NotesUnderReview = NotesUnderReview.OrderBy(x => x.NoteDetails.ModifiedDate);
                    break;
                case "Title_desc":
                    NotesUnderReview = NotesUnderReview.OrderByDescending(x => x.NoteDetails.Title);
                    break;
                case "Title":
                    NotesUnderReview = NotesUnderReview.OrderBy(x => x.NoteDetails.Title);
                    break;
                case "Category_desc":
                    NotesUnderReview = NotesUnderReview.OrderByDescending(x => x.Category.Name);
                    break;
                case "Category":
                    NotesUnderReview = NotesUnderReview.OrderBy(x => x.Category.Name);
                    break;

                case "Seller_desc":
                    NotesUnderReview = NotesUnderReview.OrderByDescending(x => x.User.FirstName);
                    break;
                case "Seller":
                    NotesUnderReview = NotesUnderReview.OrderBy(x => x.User.FirstName);
                    break;
                case "Status_desc":
                    NotesUnderReview = NotesUnderReview.OrderByDescending(x => x.Status.Value);
                    break;
                case "Status":
                    NotesUnderReview = NotesUnderReview.OrderBy(x => x.Status.Value);
                    break;
                default:
                    NotesUnderReview = NotesUnderReview.OrderByDescending(x => x.NoteDetails.ModifiedDate);
                    break;
            }

            var Seller = dbobj.Users.Where(x => x.IsEmailVerified == true && x.RoleID == dbobj.UserRoles.Where(r=>r.Name.ToLower() == "member").Select(r=>r.ID).FirstOrDefault() && x.IsActive == true)
                        .Select(s => new
                        {
                            Text = s.FirstName + "" + s.LastName,
                        }).Distinct().ToList();

            ViewBag.SellerName = new SelectList(Seller, "Text", "Text");
            ViewBag.NotesUnderReview = NotesUnderReview.ToList().ToPagedList(NotesUnderReviewspage ?? 1, 5);
            return View();
        }

        [Route("Approve/{id}")]
        [HttpGet]
        public ActionResult Approve(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var Emailid = User.Identity.Name.ToString();
            User user = dbobj.Users.Where(x => x.EmailID == Emailid).FirstOrDefault();

            SellerNote note = dbobj.SellerNotes.Find(id);

            if (note == null)
            {
                return RedirectToAction("Error", "Home");
            }

            note.Status = dbobj.ReferenceDatas.Where(x => x.RefCategory == "Notes Status" && x.Value == "Published").Select(x => x.ID).FirstOrDefault();
            note.ModifiedBy = user.ID;
            note.ModifiedDate = DateTime.Now;
            note.ActionedBy = user.ID;
            note.PubilshedDate = DateTime.Now;
            dbobj.Entry(note).State = EntityState.Modified;
            dbobj.SaveChanges();

            TempData["success"] = user.FirstName + " " + user.LastName;
            TempData["message"] = "Note has been Approved";
            return RedirectToAction("NotesUnderReview", "Admin");
        }

        [Route("InReview/{id}")]
        [HttpGet]
        public ActionResult InReview(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var Emailid = User.Identity.Name.ToString();
            User user = dbobj.Users.Where(x => x.EmailID == Emailid).FirstOrDefault();

            SellerNote note = dbobj.SellerNotes.Find(id);

            if (note == null)
            {
                return RedirectToAction("Error", "Home");
            }

            note.Status = dbobj.ReferenceDatas.Where(x => x.RefCategory == "Notes Status" && x.Value == "In Review").Select(x => x.ID).FirstOrDefault();
            note.ModifiedBy = user.ID;
            note.ModifiedDate = DateTime.Now;
            note.ActionedBy = user.ID;
            dbobj.Entry(note).State = EntityState.Modified;
            dbobj.SaveChanges();

            TempData["success"] = user.FirstName + " " + user.LastName;
            TempData["message"] = "Note Status has been changed";
            return RedirectToAction("NotesUnderReview", "Admin");
        }

        [Route("RejectedNote/{id}")]
        [HttpGet]
        public ActionResult RejectedNote(int? id, AdminRejectRemark adminremark)
        {
            var Emailid = User.Identity.Name.ToString();
            User user = dbobj.Users.Where(x => x.EmailID == Emailid).FirstOrDefault();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SellerNote note = dbobj.SellerNotes.Find(id);

            if (note == null)
            {
                return RedirectToAction("Error", "Home");
            }

            note.Status = dbobj.ReferenceDatas.Where(x => x.RefCategory == "Notes Status" && x.Value.ToLower() == "rejected").Select(x => x.ID).FirstOrDefault();
            note.ModifiedBy = user.ID;
            note.ModifiedDate = DateTime.Now;
            note.AdminRemarks = adminremark.Remarks;
            note.ActionedBy = user.ID;
            dbobj.Entry(note).State = EntityState.Modified;
            dbobj.SaveChanges();


            TempData["success"] = user.FirstName + " " + user.LastName;
            TempData["message"] = "Note has been Rejected";
            return RedirectToAction("NotesUnderReview", "Admin");
        }
    }
}