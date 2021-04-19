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
    public class AdminRejectedNoteController : Controller
    {
        private readonly NoteMarketplaceEntities dbobj = new NoteMarketplaceEntities();

        [Route("RejectedNote")]
        public ActionResult RejectedNote(string Search, int? page, string SortBy, string SellerName)
        {
            List<SellerNote> NoteTitlePublished = dbobj.SellerNotes.Where(x => x.IsActive == true && (x.Title.Contains(Search) || x.NoteCategory.Name.Contains(Search) || x.User.FirstName.Contains(Search) || x.User.LastName.Contains(Search) || x.AdminRemarks.Contains(Search) || (x.ModifiedDate.Value.Day + "-" + x.ModifiedDate.Value.Month + "-" + x.ModifiedDate.Value.Year).Contains(Search) || Search == null)).ToList();
            List<NoteCategory> CategoryNamePublished = dbobj.NoteCategories.ToList();
            List<ReferenceData> StatusNamePublished = dbobj.ReferenceDatas.Where(x => x.RefCategory == "Notes Status" && x.Value.ToLower() == "rejected" && x.IsActive == true).ToList();
            List<User> UserDetails = dbobj.Users.ToList();


            ViewBag.DateSortParamPublish = string.IsNullOrEmpty(SortBy) ? "ModifiedDate_asc" : "";
            ViewBag.TitleSortParamPublish = SortBy == "Title" ? "Title_desc" : "Title";
            ViewBag.CategorySortParamPublish = SortBy == "Category" ? "Category_desc" : "Category";
            ViewBag.SellerSortParamPublish = SortBy == "Seller" ? "Seller_desc" : "Seller";
            ViewBag.AdminSortParamPublish = SortBy == "Admin" ? "Admin_desc" : "Admin";

            var NotesUnderReview = (from nt in NoteTitlePublished
                                    join cn in CategoryNamePublished on nt.Category equals cn.ID into table1
                                    from cn in table1.ToList()
                                    join sn in StatusNamePublished on nt.Status equals sn.ID into table2
                                    from sn in table2.ToList()
                                    join us in UserDetails on nt.SellerID equals us.ID into table3
                                    from us in table3.ToList()
                                    join adm in UserDetails on nt.ActionedBy equals adm.ID into table4
                                    from adm in table4.ToList()
                                    where ((us.FirstName + us.LastName) == SellerName || string.IsNullOrEmpty(SellerName))
                                    select new RejectedNoteAdmin
                                    {
                                        NoteDetails = nt,
                                        Category = cn,
                                        Status = sn,
                                        User = us,
                                        Admin = adm

                                    }).AsQueryable();

            switch (SortBy)
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
                case "Admin_desc":
                    NotesUnderReview = NotesUnderReview.OrderByDescending(x => x.Admin.FirstName);
                    break;
                case "Admin":
                    NotesUnderReview = NotesUnderReview.OrderBy(x => x.Admin.FirstName);
                    break;
                default:
                    NotesUnderReview = NotesUnderReview.OrderByDescending(x => x.NoteDetails.ModifiedDate);
                    break;
            }

            var Seller = dbobj.Users.Where(x => x.IsEmailVerified == true && x.RoleID == 1 && x.IsActive==true)
                        .Select(s => new
                        {
                            Text = s.FirstName + "" + s.LastName,
                        }).Distinct().ToList();

            ViewBag.SellerName = new SelectList(Seller, "Text", "Text");
            ViewBag.NotesUnderReview = NotesUnderReview.ToList().ToPagedList(page ?? 1, 5);
            return View();
        }

        [Route("ApproveRejected/{id}")]
        [HttpGet]
        public ActionResult ApproveRejected(int? id)
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
            note.AdminRemarks = null;
            dbobj.Entry(note).State = EntityState.Modified;
            dbobj.SaveChanges();

            TempData["success"] = user.FirstName + " " + user.LastName;
            TempData["message"] = "Note has been Approved";
            return RedirectToAction("RejectedNote", "Admin");
        }
    }
}