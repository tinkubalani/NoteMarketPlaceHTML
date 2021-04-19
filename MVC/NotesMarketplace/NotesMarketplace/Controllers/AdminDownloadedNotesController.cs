using NotesMarketplace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace NotesMarketplace.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    [RoutePrefix("Admin")]
    public class AdminDownloadedNotesController : Controller
    {
        private readonly NoteMarketplaceEntities dbobj = new NoteMarketplaceEntities();

        [Route("DownloadedNotes")]
        public ActionResult DownloadedNotes(int? page, string SellerName, string BuyerName, string Search, string AllNotes, string SortOrder)
        {
            List<Download> downloads = dbobj.Downloads.Where(x => (x.IsAttachementDownloaded==true && x.IsSellerHasAllowedDownload == true && x.AttachmentPath != null) && (x.NoteTitle.Contains(Search) || x.NoteCategory.Contains(Search) || x.PurchasedPrice.ToString().StartsWith(Search)|| x.User.LastName.Contains(Search) || x.User.FirstName.Contains(Search)|| (x.AttacmentDownloadedDate.Value.Day + "-" + x.AttacmentDownloadedDate.Value.Month + "-" + x.AttacmentDownloadedDate.Value.Year).Contains(Search) || Search == null)).ToList();
            List<User> users = dbobj.Users.Where(x => x.RoleID == dbobj.UserRoles.Where(y => y.Name.ToLower() == "member").Select(y => y.ID).FirstOrDefault() && x.IsEmailVerified == true && x.IsActive == true).ToList();

            ViewBag.DateSortParam = string.IsNullOrEmpty(SortOrder) ? "CreatedDate_asc" : "";
            ViewBag.TitleSortParam = SortOrder == "Title" ? "Title_desc" : "Title";
            ViewBag.CategorySortParam = SortOrder == "Category" ? "Category_desc" : "Category";
            ViewBag.SellerSortParam = SortOrder == "Seller" ? "Seller_desc" : "Seller";
            ViewBag.BuyerSortParam = SortOrder == "Buyer" ? "Buyer_desc" : "Buyer";
            ViewBag.SellTypeSortParam = SortOrder == "SellType" ? "SellType_desc" : "SellType";
            ViewBag.PriceSortParam = SortOrder == "Price" ? "Price_desc" : "Price";

            var downloadsnotes = (from nt in downloads
                                  join seller in users on nt.Seller equals seller.ID into table1
                                  from seller in table1.ToList()
                                  join down in users on nt.Downloader equals down.ID into table2
                                  from down in table2.ToList()
                                  where (((seller.FirstName + seller.LastName) == SellerName || string.IsNullOrEmpty(SellerName)) &&
                                  ((down.FirstName + down.LastName) == BuyerName || string.IsNullOrEmpty(BuyerName)) &&
                                  ((nt.NoteTitle) == AllNotes || string.IsNullOrEmpty(AllNotes))
                                  )
                                  select new AdminDownloadNotes
                                  {
                                      Downloads = nt,
                                      Seller = seller,
                                      Buyer = down,
                                  }).AsQueryable();

            var Seller = dbobj.Users.Where(x => x.IsEmailVerified == true && x.RoleID == 1 && x.IsActive == true)
                                        .Select(s => new
                                        {
                                            Text = s.FirstName + "" + s.LastName,
                                        }).Distinct().ToList();
            var Notes = dbobj.Downloads.Select(s => new
            {
                Text = s.NoteTitle,
            }).Distinct().ToList();

            ViewBag.SellerName = new SelectList(Seller, "Text", "Text");
            ViewBag.BuyerName = new SelectList(Seller, "Text", "Text");
            ViewBag.AllNotes = new SelectList(Notes, "Text", "Text");

            switch (SortOrder)
            {
                case "CreatedDate_asc":
                    downloadsnotes = downloadsnotes.OrderBy(x => x.Downloads.AttacmentDownloadedDate);
                    break;
                case "Title_desc":
                    downloadsnotes = downloadsnotes.OrderByDescending(x => x.Downloads.NoteTitle);
                    break;
                case "Title":
                    downloadsnotes = downloadsnotes.OrderBy(x => x.Downloads.NoteTitle);
                    break;
                case "Category_desc":
                    downloadsnotes = downloadsnotes.OrderByDescending(x => x.Downloads.NoteCategory);
                    break;
                case "Category":
                    downloadsnotes = downloadsnotes.OrderBy(x => x.Downloads.NoteCategory);
                    break;
                case "Seller_desc":
                    downloadsnotes = downloadsnotes.OrderByDescending(x => x.Seller.FirstName);
                    break;
                case "Seller":
                    downloadsnotes = downloadsnotes.OrderBy(x => x.Seller.FirstName);
                    break;
                case "Buyer_desc":
                    downloadsnotes = downloadsnotes.OrderByDescending(x => x.Buyer.FirstName);
                    break;
                case "Buyer":
                    downloadsnotes = downloadsnotes.OrderBy(x => x.Buyer.FirstName);
                    break;
                case "SellType_desc":
                    downloadsnotes = downloadsnotes.OrderByDescending(x => x.Downloads.IsPaid);
                    break;
                case "SellType":
                    downloadsnotes = downloadsnotes.OrderBy(x => x.Downloads.IsPaid);
                    break;
                case "Price_desc":
                    downloadsnotes = downloadsnotes.OrderByDescending(x => x.Downloads.PurchasedPrice);
                    break;
                case "Price":
                    downloadsnotes = downloadsnotes.OrderBy(x => x.Downloads.PurchasedPrice);
                    break;
                default:
                    downloadsnotes = downloadsnotes.OrderByDescending(x => x.Downloads.AttacmentDownloadedDate);
                    break;
            }

            ViewBag.downloadsnotes = downloadsnotes.ToList().ToPagedList(page ?? 1, 5);
            return View();
        }
    }
}