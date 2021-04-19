using NotesMarketplace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace NotesMarketplace.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    [RoutePrefix("Admin")]
    public class AdminSpamReportsController : Controller
    {
        private readonly NoteMarketplaceEntities dbobj = new NoteMarketplaceEntities();

        [Route("SpamReports")]
        // GET: SpamReportsAdmin
        public ActionResult SpamReports(string Search, int? page, string SortOrder)
        {
            List<SellerNotesReportedIssue> reports = dbobj.SellerNotesReportedIssues.Where(x => x.SellerNote.Title.Contains(Search) || x.Remarks.Contains(Search) || x.SellerNote.NoteCategory.Name.Contains(Search) || x.User.FirstName.Contains(Search) || x.User.LastName.Contains(Search) || (x.ModifiedDate.Value.Day + "-" + x.ModifiedDate.Value.Month + "-" + x.ModifiedDate.Value.Year).Contains(Search) || Search == null).ToList();
            List<SellerNote> NoteTitlePublished = dbobj.SellerNotes.ToList();
            List<NoteCategory> CategoryNamePublished = dbobj.NoteCategories.ToList();
            List<User> UserDetails = dbobj.Users.ToList();

            ViewBag.DateSortParam = string.IsNullOrEmpty(SortOrder) ? "CreatedDate_asc" : "";
            ViewBag.TitleSortParam = SortOrder == "Title" ? "Title_desc" : "Title";
            ViewBag.CategorySortParam = SortOrder == "Category" ? "Category_desc" : "Category";
            ViewBag.ReportedBySortParam = SortOrder == "ReportedBy" ? "ReportedBy_desc" : "ReportedBy";

            var NotesSpamReport = (from sr in reports
                                   join nt in NoteTitlePublished on sr.NoteID equals nt.ID into table1
                                   from nt in table1.ToList()
                                   join cn in CategoryNamePublished on nt.Category equals cn.ID into table2
                                   from cn in table2.ToList()
                                   join us in UserDetails on sr.ReportedByID equals us.ID into table3
                                   from us in table3.ToList()
                                   select new SpamReportedAdmin
                                   {
                                       Reports = sr,
                                       NoteDetails = nt,
                                       Category = cn,
                                       user = us
                                   }).AsQueryable();

            switch (SortOrder)
            {
                case "CreatedDate_asc":
                    NotesSpamReport = NotesSpamReport.OrderBy(x => x.Reports.ModifiedDate);
                    break;
                case "Title_desc":
                    NotesSpamReport = NotesSpamReport.OrderByDescending(x => x.NoteDetails.Title);
                    break;
                case "Title":
                    NotesSpamReport = NotesSpamReport.OrderBy(x => x.NoteDetails.Title);
                    break;
                case "Category_desc":
                    NotesSpamReport = NotesSpamReport.OrderByDescending(x => x.Category.Name);
                    break;
                case "Category":
                    NotesSpamReport = NotesSpamReport.OrderBy(x => x.Category.Name);
                    break;
                case "ReportedBy_desc":
                    NotesSpamReport = NotesSpamReport.OrderByDescending(x => x.user.FirstName);
                    break;
                case "ReportedBy":
                    NotesSpamReport = NotesSpamReport.OrderBy(x => x.user.FirstName);
                    break;
                default:
                    NotesSpamReport = NotesSpamReport.OrderByDescending(x => x.Reports.ModifiedDate);
                    break;
            }

            ViewBag.NotesSpamReport = NotesSpamReport.ToList().ToPagedList(page ?? 1, 3);
            return View();
        }

        [Route("DeleteSpamReport/{id}")]
        public ActionResult DeleteSpamReport(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SellerNotesReportedIssue report = dbobj.SellerNotesReportedIssues.Find(id);
            if (report == null)
            {
                return RedirectToAction("Error", "Home");
            }
            dbobj.SellerNotesReportedIssues.Remove(report);
            dbobj.SaveChanges();

            TempData["success"] = "Admin";
            TempData["message"] = "Report has been deleted";
            return RedirectToAction("SpamReports", "Admin");
        }
    }
}