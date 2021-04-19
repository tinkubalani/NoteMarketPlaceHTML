using NotesMarketplace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Data.Entity;

namespace NotesMarketplace.Controllers
{
    [RoutePrefix("AdminMember")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class AdminMemberDetailsController : Controller
    {
        private readonly NoteMarketplaceEntities dbobj = new NoteMarketplaceEntities();

        [Route("MemberList")]
        public ActionResult MemberList(int? page,string search,string sortBy)
        {
            var users = dbobj.Users.Where(x => x.IsActive == true && x.IsEmailVerified == true && x.RoleID == dbobj.UserRoles.Where(r=>r.Name.ToLower() == "member").Select(r=>r.ID).FirstOrDefault() 
                                            && ((x.FirstName.Contains(search) || x.LastName.Contains(search) || x.EmailID.Contains(search) || (x.CreatedDate.Value.Day + "-" + x.CreatedDate.Value.Month + "-" + x.CreatedDate.Value.Year).Contains(search) || x.Downloads1.Where(s => s.Seller == x.ID && s.IsSellerHasAllowedDownload == true && s.AttachmentPath != null).Sum(s => s.PurchasedPrice).ToString().StartsWith(search) || x.Downloads.Where(s=>s.Downloader == x.ID && s.IsSellerHasAllowedDownload == true && s.AttachmentPath != null).Sum(s=>s.PurchasedPrice).ToString().StartsWith(search) || search==null))).ToList().AsQueryable();

            ViewBag.Download = dbobj.Downloads.Where(x => x.IsSellerHasAllowedDownload == true && x.AttachmentPath != null).ToList();
            ViewBag.SellerNotes = dbobj.SellerNotes.Where(x => x.IsActive == true).ToList();
            ViewBag.ReferenceData = dbobj.ReferenceDatas.Where(x => x.IsActive == true);

            ViewBag.SortJoinDateParameter = string.IsNullOrEmpty(sortBy) ? "JoinDate asc" : "";
            ViewBag.SortFirstNameParameter = sortBy == "FName" ? "FName desc" : "FName";
            ViewBag.SortLastNameParameter = sortBy == "LName" ? "LName desc" : "LName";
            ViewBag.SortEmailParameter = sortBy == "EmailID" ? "EmailID desc" : "EmailID";
            ViewBag.SortEarningParameter = sortBy == "Earning" ? "Earning desc" : "Earning";
            ViewBag.SortExpenceParameter = sortBy == "Expence" ? "Expence desc" : "Expence";

            switch (sortBy)
            {
                case "JoinDate asc":
                    users = users.OrderBy(x => x.CreatedDate);
                    break;
                case "FName desc":
                    users = users.OrderByDescending(x => x.FirstName);
                    break;
                case "FName":
                    users = users.OrderBy(x => x.FirstName);
                    break;
                case "LName desc":
                    users = users.OrderByDescending(x => x.LastName);
                    break;
                case "LName":
                    users = users.OrderBy(x => x.LastName);
                    break;
                case "EmailID desc":
                    users = users.OrderByDescending(x => x.EmailID);
                    break;
                case "EmailID":
                    users = users.OrderBy(x => x.EmailID);
                    break;
                default:
                    users = users.OrderByDescending(x => x.CreatedDate);
                    break;
            }

            return View(users.ToList().ToPagedList(page ?? 1, 5));
        }

        [Route("MemberDetail/{id}")]
        public ActionResult MemberDetail(int? id, string sortBy, int? Page, string SearchPublished)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User userObj  = dbobj.Users.Find(id);

            if (userObj == null)
            {
                return RedirectToAction("Error", "Home");
            }

            UserProfile userProfile = dbobj.UserProfiles.Where(x=>x.UserID == userObj.ID).FirstOrDefault();
            if (userProfile == null)
            {
                @TempData["ProfileNotFound"] = dbobj.Users.Where(x => x.ID == userObj.ID).Select(x => x.FirstName + " " + x.LastName).FirstOrDefault();
                return RedirectToAction("Index", "Admin");
            }

            ViewBag.userObj = userObj;
            ViewBag.userProfileObj = userProfile;

            ViewBag.SortDateAddedParameter = string.IsNullOrEmpty(sortBy) ? "DateAdded asc" : "";
            ViewBag.SortTitleParameter = sortBy == "Title" ? "Title desc" : "Title";
            ViewBag.SortCategoryParameter = sortBy == "Category" ? "Category desc" : "Category";
            ViewBag.SortStatusParameter = sortBy == "Status" ? "Status desc" : "Status";
            ViewBag.SortPublishedDateParameter = sortBy == "PublishedDate" ? "PublishedDate desc" : "PublishedDate";

            List<SellerNote> sellerNotes = dbobj.SellerNotes.Where(x => x.SellerID == userObj.ID && x.IsActive == true && (x.Title.Contains(SearchPublished) || x.NoteCategory.Name.Contains(SearchPublished) || x.ReferenceData.Value.Contains(SearchPublished)
             || (x.PubilshedDate.Value.Day + "-" + x.PubilshedDate.Value.Month + "-" + x.PubilshedDate.Value.Year).Contains(SearchPublished)
              || (x.ModifiedDate.Value.Day + "-" + x.ModifiedDate.Value.Month + "-" + x.ModifiedDate.Value.Year).Contains(SearchPublished)
            || SearchPublished == null)).ToList();
            List<NoteCategory> noteCategories = dbobj.NoteCategories.ToList();
            List<ReferenceData> referenceDatas = dbobj.ReferenceDatas.Where(x => x.RefCategory == "Notes Status" && x.Value.ToLower() != "draft").ToList();
            List<Download> downloads = dbobj.Downloads.ToList();

            var AllNotes = (from sell in sellerNotes
                                  join cate in noteCategories on sell.Category equals cate.ID into table1
                                  from cate in table1.ToList()
                                  join refe in referenceDatas on sell.Status equals refe.ID into table2
                                  from refe in table2.ToList()
                                  select new AllNotesOfMember
                                  {
                                      SellerNotes = sell,
                                      NoteCategories = cate,
                                      ReferenceDatas = refe
                                  }).AsQueryable();

            switch (sortBy)
            {
                case "DateAdded asc":
                    AllNotes = AllNotes.OrderBy(x => x.SellerNotes.ModifiedDate);
                    break;
                case "Title desc":
                    AllNotes = AllNotes.OrderByDescending(x => x.SellerNotes.Title);
                    break;
                case "Title":
                    AllNotes = AllNotes.OrderBy(x => x.SellerNotes.Title);
                    break;
                case "Category desc":
                    AllNotes = AllNotes.OrderByDescending(x => x.NoteCategories.Name);
                    break;
                case "Category":
                    AllNotes = AllNotes.OrderBy(x => x.NoteCategories.Name);
                    break;
                case "Status desc":
                    AllNotes = AllNotes.OrderByDescending(x => x.ReferenceDatas.Value);
                    break;
                case "Status":
                    AllNotes = AllNotes.OrderBy(x => x.ReferenceDatas.Value);
                    break;
                case "PublishedDate desc":
                    AllNotes = AllNotes.OrderByDescending(x => x.SellerNotes.PubilshedDate);
                    break;
                case "PublishedDate":
                    AllNotes = AllNotes.OrderBy(x => x.SellerNotes.PubilshedDate);
                    break;
                default:
                    AllNotes = AllNotes.OrderByDescending(x => x.SellerNotes.ModifiedDate);
                    break;
            }

            ViewBag.AllNotes = AllNotes.ToList().ToPagedList(Page ?? 1, 5);

            return View();
        }

        [Route("DeactivateMember/{id}")]
        public ActionResult DeactivateMember(int? id)
        {
            var Emailid = User.Identity.Name.ToString();
            User user = dbobj.Users.Where(x => x.EmailID == Emailid).FirstOrDefault();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User users = dbobj.Users.Find(id);

            users.IsActive = false;
            dbobj.Entry(users).State = EntityState.Modified;
            dbobj.SaveChanges();

            var userid = users.ID;

            var notes = dbobj.SellerNotes.Where(x => x.SellerID == userid);
            foreach (var item in notes)
            {
                item.Status = dbobj.ReferenceDatas.Where(x => x.RefCategory == "Notes Status" && x.Value.ToLower() == "removed").Select(x => x.ID).FirstOrDefault();
                item.ActionedBy = user.ID;
                item.ModifiedBy = user.ID;
                item.ModifiedDate = DateTime.Now;
                item.AdminRemarks = null;
                dbobj.Entry(item).State = EntityState.Modified;
            }
            dbobj.SaveChanges();

            TempData["success"] = user.FirstName + " " + user.LastName;
            TempData["message"] = "Member has been Deactivated";
            return RedirectToAction("MemberList", "AdminMember");

        }
    }
}