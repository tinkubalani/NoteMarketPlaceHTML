using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace NoteMarketPlace.Controllers
{
    [RoutePrefix("AdminMember")]
    [Authorize(Roles = "Admin")]
    public class AdminMemberDetailsController : Controller
    {
        private readonly NoteMarketplaceEntities dbobj = new NoteMarketplaceEntities();

        [Route("MemberDetail/{id}")]
        public ActionResult MemberDetail(int? id, string sortBy, int? Page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User userObj  = dbobj.Users.Find(id);

            if (userObj == null)
            {
                return HttpNotFound();
            }

            UserProfile userProfile = dbobj.UserProfiles.Where(x=>x.UserID == userObj.ID).FirstOrDefault();
            if (userObj == null)
            {
                return HttpNotFound();
            }

            ViewBag.userObj = userObj;
            ViewBag.userProfileObj = userProfile;

            ViewBag.SortDateAddedParameter = string.IsNullOrEmpty(sortBy) ? "DateAdded asc" : "";
            ViewBag.SortTitleParameter = sortBy == "Title" ? "Title desc" : "Title";
            ViewBag.SortCategoryParameter = sortBy == "Category" ? "Category desc" : "Category";
            ViewBag.SortStatusParameter = sortBy == "Status" ? "Status desc" : "Status";
            ViewBag.SortPublishedDateParameter = sortBy == "PublishedDate" ? "PublishedDate desc" : "PublishedDate";


            List<SellerNote> sellerNotes = dbobj.SellerNotes.Where(x => x.SellerID == userObj.ID && x.IsActive == true).ToList();
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
    }
}