using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Web.SessionState;
using System.Net;
using System.Data.Entity;
using NoteMarketPlace.EmailTemplates;
using System.IO;

namespace NoteMarketPlace.Controllers
{
    [Authorize]
    [RoutePrefix("User")]
    public class UserController : Controller
    {

        private readonly NoteMarketplaceEntities dbobj = new NoteMarketplaceEntities();

        [Route("UserProfile")]
        public ActionResult UserProfile()
        {
            var Emailid = User.Identity.Name.ToString();
            User user = dbobj.Users.Where(x => x.EmailID == Emailid).FirstOrDefault();

            if (User.Identity.IsAuthenticated)
            {
                UserProfile Profilepic = dbobj.UserProfiles.Where(x => x.UserID == user.ID).FirstOrDefault();
                if (Profilepic != null)
                {
                    TempData["ProfilePicture"] = Profilepic.ProfilePicture;
                }
                else
                {
                    TempData["ProfilePicture"] = Path.Combine("/SystemConfigurations/DefaultImages/", "DefaultUserImage.jpg");
                }
            }

            if (dbobj.UserProfiles.Any(x => x.UserID == user.ID))
            {
                UserProfile userProfile = dbobj.UserProfiles.Where(x => x.UserID == user.ID).FirstOrDefault();

                UserProfileModel userProfile1 = new UserProfileModel
                {
                    ID = user.ID,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    EmailID = user.EmailID,
                    DOB = userProfile.DOB,
                    Gender = userProfile.Gender,
                    PhoneNumberCountryCode = userProfile.PhoneNumberCountryCode,
                    PhoneNumber = userProfile.PhoneNumber,
                    AddressLine1 = userProfile.AddressLine1,
                    AddressLine2 = userProfile.AddressLine2,
                    City = userProfile.City,
                    State = userProfile.State,
                    ZipCode = userProfile.ZipCode,
                    Country = userProfile.Country,
                    University = userProfile.University,
                    College = userProfile.College

                };

                ViewBag.ProfilePicture = userProfile.ProfilePicture;
                ViewBag.Country = new SelectList(dbobj.Countries.Distinct().Where(x => x.IsActive == true), "Name", "Name", userProfile1.Country);
                ViewBag.PhoneNumberCountryCode = new SelectList(dbobj.Countries.Distinct().Where(x => x.IsActive == true), "CountryCode", "CountryCode", userProfile1.PhoneNumberCountryCode);
                ViewBag.Gender = new SelectList(dbobj.ReferenceDatas.Where(x => x.RefCategory == "Gender" && x.IsActive == true), "ID", "Value", userProfile1.Gender).ToList();
                return View(userProfile1);
            }
            else
            {
                UserProfileModel userProfile = new UserProfileModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    EmailID = user.EmailID
                };
                ViewBag.Country = new SelectList(dbobj.Countries.Distinct().Where(x => x.IsActive == true), "Name", "Name");
                ViewBag.PhoneNumberCountryCode = new SelectList(dbobj.Countries.Distinct().Where(x => x.IsActive == true), "CountryCode", "CountryCode");
                ViewBag.Gender = new SelectList(dbobj.ReferenceDatas.Where(x => x.RefCategory == "Gender" && x.IsActive == true), "ID", "Value").ToList();
                return View(userProfile);
            }
        }

        [Route("UserProfile")]
        [HttpPost]
        public ActionResult UserProfile(UserProfileModel userProfile)
        {
            if (ModelState.IsValid)
            {
                var Emailid = User.Identity.Name.ToString();
                User user = dbobj.Users.Where(x => x.EmailID == Emailid).FirstOrDefault();

                if (dbobj.UserProfiles.Any(x => x.UserID == user.ID))
                {
                    UserProfile userProfileObj = dbobj.UserProfiles.Where(x => x.UserID == user.ID).FirstOrDefault();
                    userProfileObj.DOB = userProfile.DOB;
                    userProfileObj.Gender = userProfile.Gender;
                    userProfileObj.PhoneNumberCountryCode = userProfile.PhoneNumberCountryCode;
                    userProfileObj.PhoneNumber = userProfile.PhoneNumber;
                    userProfileObj.AddressLine1 = userProfile.AddressLine1;
                    userProfileObj.AddressLine2 = userProfile.AddressLine2;
                    userProfileObj.City = userProfile.City;
                    userProfileObj.State = userProfile.State;
                    userProfileObj.ZipCode = userProfile.ZipCode;
                    userProfileObj.Country = userProfile.Country;
                    userProfileObj.University = userProfile.University;
                    userProfileObj.College = userProfile.College;
                    userProfileObj.ModifiedBy = user.ID;
                    userProfileObj.ModifiedDate = DateTime.Now;

                    dbobj.Entry(userProfileObj).State = EntityState.Modified;
                    dbobj.SaveChanges();

                    user.FirstName = userProfile.FirstName;
                    user.LastName = userProfile.LastName;
                    dbobj.Entry(user).State = EntityState.Modified;
                    dbobj.SaveChanges();

                    string path = Path.Combine(Server.MapPath("~/Members"), user.ID.ToString());

                    // Check for Directory, If not exist, then create it  
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    if (userProfile.ProfilePicture != null && userProfile.ProfilePicture.ContentLength > 0)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(userProfile.ProfilePicture.FileName);
                        string extension = Path.GetExtension(userProfile.ProfilePicture.FileName);
                        fileName = "DP_" + DateTime.Now.ToString("ddMMyyyy") + extension;
                        string finalpath = Path.Combine(path, fileName);
                        userProfile.ProfilePicture.SaveAs(finalpath);

                        userProfileObj.ProfilePicture = Path.Combine(("/Members/" + user.ID + "/"), fileName);
                        dbobj.SaveChanges();
                    }
                    else
                    {
                        userProfileObj.ProfilePicture = "/SystemConfigurations/DefaultImages/DefaultUserImage.jpg";
                        dbobj.SaveChanges();
                    }

                    return RedirectToAction("SearchNotes", "Home");
                }
                else
                {
                    UserProfile userProfileObj = new UserProfile
                    {
                        UserID = user.ID,
                        DOB = userProfile.DOB,
                        Gender = userProfile.Gender,
                        PhoneNumberCountryCode = userProfile.PhoneNumberCountryCode,
                        PhoneNumber = userProfile.PhoneNumber,
                        AddressLine1 = userProfile.AddressLine1,
                        AddressLine2 = userProfile.AddressLine2,
                        City = userProfile.City,
                        State = userProfile.State,
                        ZipCode = userProfile.ZipCode,
                        Country = userProfile.Country,
                        University = userProfile.University,
                        College = userProfile.College,
                        CreatedDate = DateTime.Now,
                        CreatedBy = user.ID,
                        ModifiedDate = DateTime.Now,
                        ModifiedBy = user.ID

                    };

                    dbobj.UserProfiles.Add(userProfileObj);
                    dbobj.SaveChanges();

                    string path = Path.Combine(Server.MapPath("~/Members"), user.ID.ToString());

                    // Check for Directory, If not exist, then create it  
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    if (userProfile.ProfilePicture != null && userProfile.ProfilePicture.ContentLength > 0)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(userProfile.ProfilePicture.FileName);
                        string extension = Path.GetExtension(userProfile.ProfilePicture.FileName);
                        fileName = "DP_" + DateTime.Now.ToString("ddMMyyyy") + extension;
                        string finalpath = Path.Combine(path, fileName);
                        userProfile.ProfilePicture.SaveAs(finalpath);

                        userProfileObj.ProfilePicture = Path.Combine(("/Members/" + user.ID + "/"), fileName);
                        dbobj.SaveChanges();
                    }
                    else
                    {
                        userProfileObj.ProfilePicture = "/SystemConfigurations/DefaultImages/DefaultMemberImage.jpg";
                        dbobj.SaveChanges();
                    }

                    user.FirstName = userProfile.FirstName;
                    user.LastName = userProfile.LastName;
                    dbobj.Entry(user).State = EntityState.Modified;
                    dbobj.SaveChanges();
                }

                return RedirectToAction("SearchNotes", "Home");
            }
            return View();
        }

        //To Download User Profile Picture
        public ActionResult DownloadPicture(int? id)
        {
            UserProfile userProfile = dbobj.UserProfiles.Where(x => x.UserID == id).FirstOrDefault();

            var displaypath = userProfile.ProfilePicture;

            /* var allFilesPath = attechment.FilePath.Split(';');*/

            string FullPath = Path.Combine(Server.MapPath("~" + displaypath));

            string FileName = Path.GetFileName(FullPath);
            return File(FullPath, "image/*", FileName);
        }

        [Route("Index")]
        // GET: User
        public ActionResult Index(string searchInProgress, string searchPublished, int? PublishedNotespage, int? ProgressNotespage, string sortByForProgress, string sortByForPublished)
        {
            var EmailID = User.Identity.Name.ToString();
            User userObj = dbobj.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();

            List<Download> DownloadNotes = dbobj.Downloads.Where(x => x.Seller == userObj.ID && x.IsSellerHasAllowedDownload == false && x.IsPaid == true).ToList();
            ViewBag.TotlaBuyerRequest = DownloadNotes.Count();
            List<Download> MyDownloadNotes = dbobj.Downloads.Where(x => x.Downloader == userObj.ID && x.IsSellerHasAllowedDownload == true && x.AttachmentPath != "").ToList();
            ViewBag.MyDownloadNotes = MyDownloadNotes.Count();
            List<Download> MySoldNotes = dbobj.Downloads.Where(x => x.Seller == userObj.ID && x.IsSellerHasAllowedDownload == true && x.AttachmentPath != "").ToList();
            ViewBag.MySoldNotes = MySoldNotes.Count();
            ViewBag.MyTotalEarning = MySoldNotes.Sum(x => x.PurchasedPrice);
            var Status = dbobj.ReferenceDatas.Where(x => x.RefCategory == "Notes Status" && x.Value == "Rejected" && x.IsActive == true).Select(x => x.ID).FirstOrDefault();
            List<SellerNote> rejectedNote = dbobj.SellerNotes.Where(x => x.SellerID == userObj.ID && x.Status == Status && x.IsActive == true ).ToList();
            ViewBag.MyRejectedNotes = rejectedNote.Count();

            ViewBag.SortCreatedDateParameter = string.IsNullOrEmpty(sortByForProgress) ? "CreatedDate asc" : "";
            ViewBag.SortTitleParameter = sortByForProgress == "Title" ? "Title desc" : "Title";
            ViewBag.SortCategoryParameter = sortByForProgress == "Category" ? "Category desc" : "Category";
            ViewBag.SortStatusParameter = sortByForProgress == "Status" ? "Status desc" : "Status";

            List<SellerNote> sellerNotes = dbobj.SellerNotes.OrderByDescending(x => x.CreatedDate).Where(x => x.SellerID == userObj.ID && x.IsActive == true && (x.Title.Contains(searchInProgress) || searchInProgress == null)).ToList();
            List<NoteCategory> noteCategories = dbobj.NoteCategories.ToList();
            List<ReferenceData> referenceDatas = dbobj.ReferenceDatas.Where(x => x.RefCategory == "Notes Status" && x.Value != "Rejected" && x.Value != "Removed").ToList();

            var ProgressNotes = (from s in sellerNotes
                                 join c in noteCategories on s.Category equals c.ID into table1
                                 from c in table1.ToList()
                                 join r in referenceDatas on s.Status equals r.ID into table2
                                 from r in table2.ToList()
                                 where (r.Value != "Published")
                                 select new UserInProgressNoteViewModel
                                 {
                                     SellerNotes = s,
                                     NoteCategories = c,
                                     ReferenceDatas = r
                                 }).AsQueryable();

            switch (sortByForProgress)
            {
                case "CreatedDate asc":
                    ProgressNotes = ProgressNotes.OrderBy(x => x.SellerNotes.CreatedDate);
                    break;
                case "Title desc":
                    ProgressNotes = ProgressNotes.OrderByDescending(x => x.SellerNotes.Title);
                    break;
                case "Title":
                    ProgressNotes = ProgressNotes.OrderBy(x => x.SellerNotes.Title);
                    break;
                case "Category desc":
                    ProgressNotes = ProgressNotes.OrderByDescending(x => x.NoteCategories.Name);
                    break;
                case "Category":
                    ProgressNotes = ProgressNotes.OrderBy(x => x.NoteCategories.Name);
                    break;
                case "Status desc":
                    ProgressNotes = ProgressNotes.OrderByDescending(x => x.ReferenceDatas.Value);
                    break;
                case "Status":
                    ProgressNotes = ProgressNotes.OrderBy(x => x.ReferenceDatas.Value);
                    break;
                default:
                    ProgressNotes = ProgressNotes.OrderByDescending(x => x.SellerNotes.CreatedDate);
                    break;
            }

            ViewBag.ProgressNotes = ProgressNotes.ToList().ToPagedList(ProgressNotespage ?? 1, 5);


            ViewBag.SortCreatedDateParameterPublish = string.IsNullOrEmpty(sortByForPublished) ? "CreatedDate asc" : "";
            ViewBag.SortTitleParameterPublish = sortByForPublished == "Title" ? "Title desc" : "Title";
            ViewBag.SortCategoryParameterPublish = sortByForPublished == "Category" ? "Category desc" : "Category";
            ViewBag.SortSellTypeParameterPublish = sortByForPublished == "Type" ? "Type desc" : "Type";
            ViewBag.SortPriceParameterPublish = sortByForPublished == "Price" ? "Price desc" : "Price";


            List<SellerNote> sellerNotesPublished = dbobj.SellerNotes.OrderByDescending(x => x.CreatedDate).Where(x => x.SellerID == userObj.ID && x.IsActive == true && (x.Title.Contains(searchPublished) || searchPublished == null)).ToList();
            List<NoteCategory> noteCategoriesPublished = dbobj.NoteCategories.ToList();
            List<ReferenceData> referenceDatasPublished = dbobj.ReferenceDatas.Where(x => x.RefCategory == "Notes Status" && x.Value != "Rejected" && x.Value != "Removed").ToList();

            var PublishedNotes = (from s in sellerNotesPublished
                                  join c in noteCategoriesPublished on s.Category equals c.ID into table1
                                  from c in table1.ToList()
                                  join r in referenceDatasPublished on s.Status equals r.ID into table2
                                  from r in table2.ToList()
                                  where (r.Value == "Published")
                                  select new UserPublishedNoteViewModel
                                  {
                                      SellerNotes = s,
                                      NoteCategories = c,
                                      ReferenceDatas = r
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
                default:
                    PublishedNotes = PublishedNotes.OrderByDescending(x => x.SellerNotes.PubilshedDate);
                    break;
            }

            ViewBag.PublishedNotes = PublishedNotes.ToList().ToPagedList(PublishedNotespage ?? 1, 5);

            return View();
        }

        [Route("BuyerRequests")]
        public ActionResult BuyerRequests(int? page, string Searchkeyword, string sortBy)
        {
            var EmailID = User.Identity.Name.ToString();
            User userObj = dbobj.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();

            ViewBag.SortCreatedDateParameter = string.IsNullOrEmpty(sortBy) ? "CreatedDate asc" : "";
            ViewBag.SortTitleParameter = sortBy == "Title" ? "Title desc" : "Title";
            ViewBag.SortCategoryParameter = sortBy == "Category" ? "Category desc" : "Category";
            ViewBag.SortPriceParameter = sortBy == "Price" ? "Price desc" : "Price";

            List<Download> DownloadNotes = dbobj.Downloads.Where(x => x.Seller == userObj.ID && x.IsSellerHasAllowedDownload == false && x.IsPaid == true && (x.NoteTitle.Contains(Searchkeyword) || x.NoteCategory.Contains(Searchkeyword) || x.PurchasedPrice.ToString().StartsWith(Searchkeyword) || Searchkeyword == null)).ToList();
            List<User> UserData = dbobj.Users.ToList();
            List<UserProfile> UserProfileData = dbobj.UserProfiles.ToList();

            var buyerRequestNotes = (from downloadnotes in DownloadNotes
                                     join userdata in UserData on downloadnotes.Downloader equals userdata.ID into table1
                                     from userdata in table1.ToList()
                                     join userprofiledata in UserProfileData on downloadnotes.Downloader equals userprofiledata.UserID into table2
                                     from userprofiledata in table2.ToList().DefaultIfEmpty()
                                     select new BuyerRequestNotes
                                     {
                                         DownloadNotes = downloadnotes,
                                         UserData = userdata,
                                         UserProfileData = userprofiledata,
                                     }).AsQueryable();

            switch (sortBy)
            {
                case "CreatedDate asc":
                    buyerRequestNotes = buyerRequestNotes.OrderBy(x => x.DownloadNotes.CreatedDate);
                    break;
                case "Title desc":
                    buyerRequestNotes = buyerRequestNotes.OrderByDescending(x => x.DownloadNotes.NoteTitle);
                    break;
                case "Title":
                    buyerRequestNotes = buyerRequestNotes.OrderBy(x => x.DownloadNotes.NoteTitle);
                    break;
                case "Category desc":
                    buyerRequestNotes = buyerRequestNotes.OrderByDescending(x => x.DownloadNotes.NoteCategory);
                    break;
                case "Category":
                    buyerRequestNotes = buyerRequestNotes.OrderBy(x => x.DownloadNotes.NoteCategory);
                    break;
                case "Price desc":
                    buyerRequestNotes = buyerRequestNotes.OrderByDescending(x => x.DownloadNotes.PurchasedPrice);
                    break;
                case "Price":
                    buyerRequestNotes = buyerRequestNotes.OrderBy(x => x.DownloadNotes.PurchasedPrice);
                    break;
                default:
                    buyerRequestNotes = buyerRequestNotes.OrderByDescending(x => x.DownloadNotes.CreatedDate);
                    break;
            }

            return View(buyerRequestNotes.ToList().ToPagedList(page ?? 1, 5));
        }

        [Route("AllowDownload/{id}")]
        public ActionResult AllowDownload(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Download allowdownload = dbobj.Downloads.Find(id);

            User sellerUser = dbobj.Users.Find(allowdownload.Seller);
            User buyerUser = dbobj.Users.Find(allowdownload.Downloader);

            if (allowdownload == null)
            {
                return HttpNotFound();
            }

            SellerNotesAttachement sellerNotesAttachement = dbobj.SellerNotesAttachements.Where(x => x.NoteID == allowdownload.NoteID).FirstOrDefault();

            allowdownload.IsSellerHasAllowedDownload = true;
            allowdownload.AttachmentPath = sellerNotesAttachement.FilePath;
            allowdownload.CreatedDate = DateTime.Now;
            allowdownload.ModifiedDate = DateTime.Now;
            allowdownload.ModifiedBy = allowdownload.Seller;

            dbobj.Entry(allowdownload).State = EntityState.Modified;
            dbobj.SaveChanges();

            DownloadAllowedEmail.DoenloadAllowedNotifyEmail(buyerUser, sellerUser);

            return RedirectToAction("BuyerRequests", "User");
        }

        [Route("MyDownloads")]
        public ActionResult MyDownloads(int? page, string Searchkeyword, string sortBy)
        {
            var EmailID = User.Identity.Name.ToString();
            User userObj = dbobj.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();

            ViewBag.SortCreatedDateParameter = string.IsNullOrEmpty(sortBy) ? "CreatedDate asc" : "";
            ViewBag.SortTitleParameter = sortBy == "Title" ? "Title desc" : "Title";
            ViewBag.SortCategoryParameter = sortBy == "Category" ? "Category desc" : "Category";
            ViewBag.SortSellTypeParameter = sortBy == "SellType" ? "SellType desc" : "SellType";
            ViewBag.SortPriceParameter = sortBy == "Price" ? "Price desc" : "Price";

            List<Download> DownloadNotes = dbobj.Downloads.Where(x => x.Downloader == userObj.ID && x.IsSellerHasAllowedDownload == true && x.AttachmentPath != "" && (x.NoteTitle.Contains(Searchkeyword) || x.NoteCategory.Contains(Searchkeyword) || x.PurchasedPrice.ToString().StartsWith(Searchkeyword) || Searchkeyword == null)).ToList();
            List<User> UserData = dbobj.Users.ToList();

            var buyerRequestNotes = (from downloadnotes in DownloadNotes
                                     join userdata in UserData on downloadnotes.Downloader equals userdata.ID into table1
                                     from userdata in table1.ToList().DefaultIfEmpty()
                                     select new MyDownloads
                                     {
                                         DownloadNotes = downloadnotes,
                                         UserData = userdata,
                                     }).AsQueryable();

            switch (sortBy)
            {
                case "CreatedDate asc":
                    buyerRequestNotes = buyerRequestNotes.OrderBy(x => x.DownloadNotes.CreatedDate);
                    break;
                case "Title desc":
                    buyerRequestNotes = buyerRequestNotes.OrderByDescending(x => x.DownloadNotes.NoteTitle);
                    break;
                case "Title":
                    buyerRequestNotes = buyerRequestNotes.OrderBy(x => x.DownloadNotes.NoteTitle);
                    break;
                case "Category desc":
                    buyerRequestNotes = buyerRequestNotes.OrderByDescending(x => x.DownloadNotes.NoteCategory);
                    break;
                case "Category":
                    buyerRequestNotes = buyerRequestNotes.OrderBy(x => x.DownloadNotes.NoteCategory);
                    break;
                case "SellType desc":
                    buyerRequestNotes = buyerRequestNotes.OrderByDescending(x => x.DownloadNotes.IsPaid);
                    break;
                case "SellType":
                    buyerRequestNotes = buyerRequestNotes.OrderBy(x => x.DownloadNotes.IsPaid);
                    break;
                case "Price desc":
                    buyerRequestNotes = buyerRequestNotes.OrderByDescending(x => x.DownloadNotes.PurchasedPrice);
                    break;
                case "Price":
                    buyerRequestNotes = buyerRequestNotes.OrderBy(x => x.DownloadNotes.PurchasedPrice);
                    break;
                default:
                    buyerRequestNotes = buyerRequestNotes.OrderByDescending(x => x.DownloadNotes.CreatedDate);
                    break;
            }

            return View(buyerRequestNotes.ToList().ToPagedList(page ?? 1, 10));
        }

        [Route("MySoldNotes")]
        public ActionResult MySoldNotes(int? page, string Searchkeyword, string sortBy)
        {
            var EmailID = User.Identity.Name.ToString();
            User userObj = dbobj.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();

            ViewBag.SortCreatedDateParameter = string.IsNullOrEmpty(sortBy) ? "CreatedDate asc" : "";
            ViewBag.SortTitleParameter = sortBy == "Title" ? "Title desc" : "Title";
            ViewBag.SortCategoryParameter = sortBy == "Category" ? "Category desc" : "Category";
            ViewBag.SortSellTypeParameter = sortBy == "SellType" ? "SellType desc" : "SellType";
            ViewBag.SortPriceParameter = sortBy == "Price" ? "Price desc" : "Price";

            List<Download> DownloadNotes = dbobj.Downloads.Where(x => x.Seller == userObj.ID && x.IsSellerHasAllowedDownload == true && x.AttachmentPath != "" && (x.NoteTitle.Contains(Searchkeyword) || x.NoteCategory.Contains(Searchkeyword) || x.PurchasedPrice.ToString().StartsWith(Searchkeyword) || Searchkeyword == null)).ToList();
            List<User> UserData = dbobj.Users.ToList();

            var buyerRequestNotes = (from downloadnotes in DownloadNotes
                                     join userdata in UserData on downloadnotes.Downloader equals userdata.ID into table1
                                     from userdata in table1.ToList().DefaultIfEmpty()
                                     select new MySoldNote
                                     {
                                         DownloadNotes = downloadnotes,
                                         UserData = userdata,
                                     }).AsQueryable();

            switch (sortBy)
            {
                case "CreatedDate asc":
                    buyerRequestNotes = buyerRequestNotes.OrderBy(x => x.DownloadNotes.CreatedDate);
                    break;
                case "Title desc":
                    buyerRequestNotes = buyerRequestNotes.OrderByDescending(x => x.DownloadNotes.NoteTitle);
                    break;
                case "Title":
                    buyerRequestNotes = buyerRequestNotes.OrderBy(x => x.DownloadNotes.NoteTitle);
                    break;
                case "Category desc":
                    buyerRequestNotes = buyerRequestNotes.OrderByDescending(x => x.DownloadNotes.NoteCategory);
                    break;
                case "Category":
                    buyerRequestNotes = buyerRequestNotes.OrderBy(x => x.DownloadNotes.NoteCategory);
                    break;
                case "SellType desc":
                    buyerRequestNotes = buyerRequestNotes.OrderByDescending(x => x.DownloadNotes.IsPaid);
                    break;
                case "SellType":
                    buyerRequestNotes = buyerRequestNotes.OrderBy(x => x.DownloadNotes.IsPaid);
                    break;
                case "Price desc":
                    buyerRequestNotes = buyerRequestNotes.OrderByDescending(x => x.DownloadNotes.PurchasedPrice);
                    break;
                case "Price":
                    buyerRequestNotes = buyerRequestNotes.OrderBy(x => x.DownloadNotes.PurchasedPrice);
                    break;
                default:
                    buyerRequestNotes = buyerRequestNotes.OrderByDescending(x => x.DownloadNotes.CreatedDate);
                    break;
            }

            return View(buyerRequestNotes.ToList().ToPagedList(page ?? 1, 10));
        }

        [HttpPost]
        [Route("ReportAsInappropriate/{id}")]

        public ActionResult ReportAsInappropriate(int? id, ReportAsInappropriate remark)
        {
            var EmailID = User.Identity.Name.ToString();
            User userObj = dbobj.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Download downloadrecord = dbobj.Downloads.Find(id);

            if (downloadrecord == null)
            {
                return HttpNotFound();
            }

            SellerNote sellerNote = dbobj.SellerNotes.Find(downloadrecord.NoteID);
            if (sellerNote == null)
            {
                return HttpNotFound();
            }

            User sellerUser = dbobj.Users.Find(sellerNote.SellerID);
            if (sellerUser == null)
            {
                return HttpNotFound();
            }

            SellerNotesReportedIssue sellerNotesReportedIssue = new SellerNotesReportedIssue
            {
                NoteID = downloadrecord.NoteID,
                ReportedByID = userObj.ID,
                AgainstDownloadID = downloadrecord.ID,
                Remarks = remark.Remarks,
                CreatedDate = DateTime.Now,
                CreatedBy = userObj.ID,
                ModifiedDate = DateTime.Now,
                ModifiedBy = userObj.ID
            };

            dbobj.SellerNotesReportedIssues.Add(sellerNotesReportedIssue);
            dbobj.SaveChanges();

            ReportedSpamEmail.BuyerReportebSpamNotifyEmail(sellerUser, userObj, sellerNote.Title);

            return RedirectToAction("MyDownloads", "User");
        }

        [HttpPost]
        [Route("AddReview/{id}")]
        public ActionResult AddReview(int? id, ReportAsInappropriate review)
        {
            var EmailID = User.Identity.Name.ToString();
            User userObj = dbobj.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Download downloadrecord = dbobj.Downloads.Find(id);

            if (downloadrecord == null)
            {
                return HttpNotFound();
            }

            SellerNotesReview sellerNotesReview = new SellerNotesReview
            {
                NoteID = downloadrecord.NoteID,
                ReviewedBy = userObj.ID,
                AgainstDownloadsID = downloadrecord.ID,
                Ratings = review.Ratings,
                Comments = review.Remarks,
                CreatedDate = DateTime.Now,
                CreatedBy = userObj.ID,
                ModifiedDate = DateTime.Now,
                ModifiedBy = userObj.ID,
                Isactive = true
            };

            dbobj.SellerNotesReviews.Add(sellerNotesReview);
            dbobj.SaveChanges();

            return RedirectToAction("MyDownloads", "User");
        }

        [Route("MyRejectedNotes")]
        public ActionResult MyRejectedNotes(int? page, string Searchkeyword, string sortBy)
        {
            var EmailID = User.Identity.Name.ToString();
            User userObj = dbobj.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();

            ViewBag.SortCreatedDateParameter = string.IsNullOrEmpty(sortBy) ? "CreatedDate asc" : "";
            ViewBag.SortTitleParameter = sortBy == "Title" ? "Title desc" : "Title";
            ViewBag.SortCategoryParameter = sortBy == "Category" ? "Category desc" : "Category";

            var Status = dbobj.ReferenceDatas.Where(x => x.RefCategory == "Notes Status" && x.Value == "Rejected" && x.IsActive == true).Select(x => x.ID).FirstOrDefault();

            List<SellerNote> sellerNotesPublished = dbobj.SellerNotes.Where(x => x.SellerID == userObj.ID && x.Status == Status && x.IsActive == true && (x.Title.Contains(Searchkeyword) || Searchkeyword == null)).ToList();
            List<NoteCategory> noteCategoriesPublished = dbobj.NoteCategories.ToList();

            var MyRejectedNotes = (from sell in sellerNotesPublished
                                   join cate in noteCategoriesPublished on sell.Category equals cate.ID into table1
                                   from cate in table1.ToList().DefaultIfEmpty()
                                   select new MyRejectedNote
                                   {
                                       SellerNote = sell,
                                       Category = cate,
                                   }).AsQueryable();

            switch (sortBy)
            {
                case "CreatedDate asc":
                    MyRejectedNotes = MyRejectedNotes.OrderBy(x => x.SellerNote.ModifiedDate);
                    break;
                case "Title desc":
                    MyRejectedNotes = MyRejectedNotes.OrderByDescending(x => x.SellerNote.Title);
                    break;
                case "Title":
                    MyRejectedNotes = MyRejectedNotes.OrderBy(x => x.SellerNote.Title);
                    break;
                case "Category desc":
                    MyRejectedNotes = MyRejectedNotes.OrderByDescending(x => x.Category.Name);
                    break;
                case "Category":
                    MyRejectedNotes = MyRejectedNotes.OrderBy(x => x.Category.Name);
                    break;
                default:
                    MyRejectedNotes = MyRejectedNotes.OrderByDescending(x => x.SellerNote.ModifiedDate);
                    break;
            }

            return View(MyRejectedNotes.ToList().ToPagedList(page ?? 1, 10));
        }

        [Route("CloneNote/{id}")]
        public ActionResult CloneNote(int? id)
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

            var Status = dbobj.ReferenceDatas.Where(x => x.RefCategory == "Notes Status" && x.Value.ToLower() == "draft" && x.IsActive == true).Select(x => x.ID).FirstOrDefault();

            sellerNote.Status = Status;
            sellerNote.CreatedDate = DateTime.Now;
            sellerNote.ModifiedDate = DateTime.Now;
            dbobj.Entry(sellerNote).State = EntityState.Modified;
            dbobj.SaveChanges();

            return RedirectToAction("Index", "User");
        }
    }
}