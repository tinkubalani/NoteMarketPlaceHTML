using NotesMarketplace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace NotesMarketplace.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    [RoutePrefix("Admin")]
    public class AdminPublishedNotesController : Controller
    {
        private readonly NoteMarketplaceEntities dbobj = new NoteMarketplaceEntities();

        [Route("PublishedNotes")]
        public ActionResult PublishedNotes(string Search, int? page, string SortOrder, string SellerName)
        {
            List<SellerNote> NoteTitlePublished = dbobj.SellerNotes.Where(x => x.IsActive == true && (x.Title.Contains(Search) || x.NoteCategory.Name.Contains(Search) || x.SellingPrice.ToString().StartsWith(Search) || x.User.FirstName.Contains(Search) || x.User.LastName.Contains(Search) || (x.PubilshedDate.Value.Day + "-" + x.PubilshedDate.Value.Month + "-" + x.PubilshedDate.Value.Year).Contains(Search) || Search == null)).ToList();
            List<NoteCategory> CategoryNamePublished = dbobj.NoteCategories.ToList();
            List<ReferenceData> StatusNamePublished = dbobj.ReferenceDatas.Where(x => x.RefCategory == "Notes Status" && x.Value != "Rejected" && x.Value != "Removed" && x.IsActive == true).ToList();
            List<User> UserDetails = dbobj.Users.ToList();

            ViewBag.DateSortParam = string.IsNullOrEmpty(SortOrder) ? "CreatedDate_asc" : "";
            ViewBag.TitleSortParam = SortOrder == "Title" ? "Title_desc" : "Title";
            ViewBag.CategorySortParam = SortOrder == "Category" ? "Category_desc" : "Category";
            ViewBag.SellerSortParam = SortOrder == "Seller" ? "Seller_desc" : "Seller";
            ViewBag.SellTypeSortParam = SortOrder == "SellType" ? "SellType_desc" : "SellType";
            ViewBag.PriceSortParam = SortOrder == "Price" ? "Price_desc" : "Price";
            ViewBag.ApprovedBySortParam = SortOrder == "ApprovedBy" ? "ApprovedBy_desc" : "ApprovedBy";

            var NotesPublished = (from nt in NoteTitlePublished
                                  join cn in CategoryNamePublished on nt.Category equals cn.ID into table1
                                  from cn in table1.ToList()
                                  join sn in StatusNamePublished on nt.Status equals sn.ID into table2
                                  from sn in table2.ToList()
                                  join us in UserDetails on nt.SellerID equals us.ID into table3
                                  from us in table3.ToList()
                                  join ad in UserDetails on nt.ActionedBy equals ad.ID into table4
                                  from ad in table4.ToList()
                                  where ((sn.Value == "Published") && ((us.FirstName + us.LastName) == SellerName || string.IsNullOrEmpty(SellerName)))
                                  select new PublishedNoteAdmin
                                  {
                                      NoteDetails = nt,
                                      Category = cn,
                                      Status = sn,
                                      User = us,
                                      Admin = ad
                                  }).AsQueryable();

            switch (SortOrder)
            {
                case "CreatedDate_asc":
                    NotesPublished = NotesPublished.OrderBy(x => x.NoteDetails.PubilshedDate);
                    break;
                case "Title_desc":
                    NotesPublished = NotesPublished.OrderByDescending(x => x.NoteDetails.Title);
                    break;
                case "Title":
                    NotesPublished = NotesPublished.OrderBy(x => x.NoteDetails.Title);
                    break;
                case "Category_desc":
                    NotesPublished = NotesPublished.OrderByDescending(x => x.Category.Name);
                    break;
                case "Category":
                    NotesPublished = NotesPublished.OrderBy(x => x.Category.Name);
                    break;
                case "Seller_desc":
                    NotesPublished = NotesPublished.OrderByDescending(x => x.User.FirstName);
                    break;
                case "Seller":
                    NotesPublished = NotesPublished.OrderBy(x => x.User.FirstName);
                    break;
                case "SellType_desc":
                    NotesPublished = NotesPublished.OrderByDescending(x => x.NoteDetails.IsPaid);
                    break;
                case "SellType":
                    NotesPublished = NotesPublished.OrderBy(x => x.NoteDetails.IsPaid);
                    break;
                case "Price_desc":
                    NotesPublished = NotesPublished.OrderByDescending(x => x.NoteDetails.SellingPrice);
                    break;
                case "Price":
                    NotesPublished = NotesPublished.OrderBy(x => x.NoteDetails.SellingPrice);
                    break;
                case "ApprovedBy_desc":
                    NotesPublished = NotesPublished.OrderByDescending(x => x.User.FirstName);
                    break;
                case "ApprovedBy":
                    NotesPublished = NotesPublished.OrderBy(x => x.User.FirstName);
                    break;
                default:
                    NotesPublished = NotesPublished.OrderByDescending(x => x.NoteDetails.PubilshedDate);
                    break;
            }

            var Seller = dbobj.Users.Where(x => x.IsEmailVerified == true && x.RoleID == 1 && x.IsActive == true)
                                    .Select(s => new
                                    {
                                        Text = s.FirstName + "" + s.LastName,
                                    }).Distinct().ToList();

            ViewBag.SellerName = new SelectList(Seller, "Text", "Text");
            ViewBag.NotesPublished = NotesPublished.ToList().ToPagedList(page ?? 1, 5);
            return View();

        }
    }
}