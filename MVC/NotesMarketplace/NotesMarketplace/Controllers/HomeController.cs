using NotesMarketplace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Net;
using System.IO;
using System.IO.Compression;
using NotesMarketplace.EmailTemplates;
using NotesMarketplace.Password_Encryption;

namespace NotesMarketplace.Controllers
{
    public class HomeController : Controller
    {
        private readonly NoteMarketplaceEntities dbobj = new NoteMarketplaceEntities();

        public ActionResult Index()
        {
            var Records = dbobj.SystemConfigurations.Count();
            if (Records > 0)
            {
                TempData["FacebookURL"] = dbobj.SystemConfigurations.Where(x => x.Key == "FacebookURL").Select(x => x.Value).FirstOrDefault();
                TempData["LinkedInURL"] = dbobj.SystemConfigurations.Where(x => x.Key == "LinkedinURL").Select(x => x.Value).FirstOrDefault();
                TempData["TwitterURL"] = dbobj.SystemConfigurations.Where(x => x.Key == "TwitterURL").Select(x => x.Value).FirstOrDefault();
                TempData.Keep("FacebookURL");
                TempData.Keep("LinkedInURL");
                TempData.Keep("TwitterURL");
            }
            return View();
        }

        [Route("Home/Contact")]
        public ActionResult Contact()
        {

            if (User.Identity.IsAuthenticated)
            {
                var EmailID = User.Identity.Name.ToString();
                var v = dbobj.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();
                if (v != null)
                {
                    ContactUs contact = new ContactUs
                    {
                        FullName = v.FirstName + " " + v.LastName,
                        EmailID = v.EmailID
                    };
                    return View(contact);
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        [Route("ContactUs")]
        [HttpPost]
        public ActionResult Contact(ContactUs contactus)
        {
            if (ModelState.IsValid)
            {
                var EmailID = User.Identity.Name.ToString();

                var v = dbobj.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();

                bool internet = CheckInternet.IsConnectedToInternet();
                if (internet == true)
                {

                    ContactUs contact = new ContactUs
                    {
                        FullName = contactus.FullName,
                        EmailID = contactus.EmailID,
                        Subject = contactus.Subject,
                        Comments = contactus.Comments
                    };

                    var SupportEmailAddress = dbobj.SystemConfigurations.Where(x => x.Key.ToLower() == "supportemailaddress").Select(y => y.Value).FirstOrDefault();
                    var EmailPassword = dbobj.SystemConfigurations.Where(x => x.Key.ToLower() == "emailpassword").Select(y => y.Value).FirstOrDefault();
                    var emails = dbobj.SystemConfigurations.Where(x => x.Key.ToLower() == "emailaddress").Select(x => x.Value).FirstOrDefault();

                    ContactUsEmail.ContactEmail(SupportEmailAddress, EmailPassword, contact,emails);

                    if (v != null)
                    {
                        TempData["success"] = contactus.FullName;
                        TempData["message"] = "Thank You For Contact Us.";
                        return RedirectToAction("Index", "User");
                    }
                    else
                    {
                        TempData["success"] = contactus.FullName;
                        TempData["message"] = "Thank You For Contact Us.";
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    TempData["internetnotconnected"] = "User";
                    return View();
                }


            }
            return View();
        }

        [Route("Home/FAQ")]
        public ActionResult FAQ()
        {
            return View();
        }


        [Route("Home/SearchNotes")]
        public ActionResult SearchNotes(string search, string Country, string Category, string NoteType, string University, string Course, string Rating, int? page)
        {
            var EmailID = User.Identity.Name.ToString();
            User userObj = dbobj.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();

            var Records = dbobj.SystemConfigurations.Count();
            if (Records > 0)
            {
                TempData["FacebookURL"] = dbobj.SystemConfigurations.Where(x => x.Key == "FacebookURL").Select(x => x.Value).FirstOrDefault();
                TempData["LinkedInURL"] = dbobj.SystemConfigurations.Where(x => x.Key == "LinkedinURL").Select(x => x.Value).FirstOrDefault();
                TempData["TwitterURL"] = dbobj.SystemConfigurations.Where(x => x.Key == "TwitterURL").Select(x => x.Value).FirstOrDefault();
                TempData.Keep("FacebookURL");
                TempData.Keep("LinkedInURL");
                TempData.Keep("TwitterURL");
            }

            if (User.Identity.IsAuthenticated)
            {
                UserProfile Profilepic = dbobj.UserProfiles.Where(x => x.UserID == userObj.ID).FirstOrDefault();
                if (Profilepic != null)
                {
                    TempData["ProfilePicture"] = Profilepic.ProfilePicture;
                }
                else
                {
                    SystemConfiguration systemConfiguration = dbobj.SystemConfigurations.Where(x => x.Key.ToLower() == "defaultprofilepicture").FirstOrDefault();
                    TempData["ProfilePicture"] = systemConfiguration.Value;
                }
            }

            List<SellerNote> sellerNotes = dbobj.SellerNotes.OrderBy(x => x.Title).Where(x => x.IsActive == true && (x.Title.StartsWith(search) || x.NoteCategory.Name.Contains(search) || search == null)).ToList();
            List<NoteCategory> noteCategories = dbobj.NoteCategories.ToList();
            List<ReferenceData> referenceDatas = dbobj.ReferenceDatas.ToList();
            List<Country> countries = dbobj.Countries.ToList();
            List<SellerNotesReview> sellerNotesReviews = dbobj.SellerNotesReviews.ToList();
            List<SellerNotesReportedIssue> sellerNotesReportedIssues = dbobj.SellerNotesReportedIssues.ToList();


            var noterecordes = from sell in sellerNotes
                               join cate in noteCategories on sell.Category equals cate.ID into table1
                               from cate in table1.ToList()
                               join refe in referenceDatas on sell.Status equals refe.ID into table2
                               from refe in table2.ToList().DefaultIfEmpty()
                               join con in countries on sell.Country equals con.ID into table3
                               from con in table3.ToList().DefaultIfEmpty()
                               where (refe.Value == "Published" && ((sell.Country.ToString() == Country || string.IsNullOrEmpty(Country)) && (sell.Category.ToString() == Category
                               || string.IsNullOrEmpty(Category)) && (sell.NoteType.ToString() == NoteType || string.IsNullOrEmpty(NoteType)) && (sell.UniversityName == University
                               || string.IsNullOrEmpty(University)) && (sell.Course == Course || string.IsNullOrEmpty(Course)) && ((Math.Round(sell.SellerNotesReviews.Where(x=>x.NoteID == sell.ID).Count() == 0 ? 0 : sell.SellerNotesReviews.Where(x => x.NoteID == sell.ID).Sum(r => r.Ratings) / sell.SellerNotesReviews.Where(x => x.NoteID == sell.ID).Count())).ToString() == Rating || String.IsNullOrEmpty(Rating))))
                               select new AllPublishedNotes
                               {
                                   SellerNotes = sell,
                                   NoteCategories = cate,
                                   ReferenceDatas = refe,
                                   Countries = con
                               };

            ViewBag.TotalRecord = noterecordes.Count();
            ViewBag.NotesCategory = dbobj.NoteCategories.Distinct();
            ViewBag.NotesType = dbobj.NoteTypes.Distinct();
            ViewBag.Country = dbobj.Countries.Distinct();
            ViewBag.Course = dbobj.SellerNotes.Where(x => x.Course != null).Select(x => x.Course).Distinct();
            ViewBag.University = dbobj.SellerNotes.Where(x => x.UniversityName != null).Select(x => x.UniversityName).Distinct();

            ViewBag.Rating = Enumerable.Range(0, 6).ToList();
            return View(noterecordes.ToList().ToPagedList(page ?? 1, 9));
        }

        [Route("Home/NoteDetail/{id}")]
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

            if (User.Identity.IsAuthenticated)
            {
                var Email = User.Identity.Name.ToString();
                ViewBag.BuyerId = dbobj.Users.Where(x => x.EmailID == Email).Select(x=>x.ID).FirstOrDefault();
            }

            var EmailID = User.Identity.Name.ToString();
            ViewBag.Buyer = dbobj.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();
            ViewBag.Seller = dbobj.Users.Where(x => x.ID == sellerNotes.SellerID).FirstOrDefault();
            ViewBag.PhoneNumber = dbobj.UserProfiles.Where(x => x.UserID == sellerNotes.SellerID).Select(x => x.PhoneNumberCountryCode + " " + x.PhoneNumber).FirstOrDefault();
            return View(sellerNotes);
        }


        [Authorize(Roles = "Member")]
        [Route("Home/DownloadNote/{id}")]
        public ActionResult DownloadNote(int? id)
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

            SellerNotesAttachement sellerNotesAttachement = dbobj.SellerNotesAttachements.Where(x => x.NoteID == sellerNotes.ID).FirstOrDefault();
            NoteCategory noteCategory = dbobj.NoteCategories.Find(sellerNotes.Category);

            var EmailID = User.Identity.Name.ToString();
            User userObj = dbobj.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();

            if (sellerNotes.IsPaid == false)
            {
                Download download = new Download
                {
                    NoteID = sellerNotes.ID,
                    Seller = sellerNotes.SellerID,
                    Downloader = userObj.ID,
                    IsSellerHasAllowedDownload = true,
                    AttachmentPath = sellerNotesAttachement.FilePath,
                    IsAttachementDownloaded = true,
                    AttacmentDownloadedDate = DateTime.Now,
                    IsPaid = sellerNotes.IsPaid,
                    PurchasedPrice = sellerNotes.SellingPrice,
                    NoteTitle = sellerNotes.Title,
                    NoteCategory = noteCategory.Name,
                    CreatedDate = DateTime.Now,
                    CreatedBy = userObj.ID,
                    ModifiedDate = DateTime.Now,
                    ModifiedBy = userObj.ID
                };

                dbobj.Downloads.Add(download);
                dbobj.SaveChanges();

                var allFilesPath = sellerNotesAttachement.FilePath.Split(';');
                var allFileName = sellerNotesAttachement.FileName.Split(';');

                using (var memoryStream = new MemoryStream())
                {
                    using (var ziparchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        foreach (var FilePath in allFilesPath)
                        {
                            string FullPath = Path.Combine(Server.MapPath("~" + FilePath));
                            string FileName = Path.GetFileName(FullPath);
                            if (FileName == "")
                            {
                                continue;
                            }
                            else
                            {
                                ziparchive.CreateEntryFromFile(FullPath, FileName);
                            }
                        }
                    }
                    return File(memoryStream.ToArray(), "application/zip", "Attachments.zip");
                }
            }
            else
            {
                bool internet = CheckInternet.IsConnectedToInternet();
                if (internet == true)
                {

                    Download download = new Download
                    {
                        NoteID = sellerNotes.ID,
                        Seller = sellerNotes.SellerID,
                        Downloader = userObj.ID,
                        IsSellerHasAllowedDownload = false,
                        AttachmentPath = null,
                        IsAttachementDownloaded = false,
                        IsPaid = sellerNotes.IsPaid,
                        PurchasedPrice = sellerNotes.SellingPrice,
                        NoteTitle = sellerNotes.Title,
                        NoteCategory = noteCategory.Name,
                        CreatedDate = DateTime.Now,
                        CreatedBy = userObj.ID,
                        ModifiedDate = DateTime.Now,
                        ModifiedBy = userObj.ID
                    };

                    dbobj.Downloads.Add(download);
                    dbobj.SaveChanges();

                    User sellerRecord = dbobj.Users.Find(sellerNotes.SellerID);

                    var SupportEmailAddress = dbobj.SystemConfigurations.Where(x => x.Key.ToLower() == "supportemailaddress").Select(y => y.Value).FirstOrDefault();
                    var EmailPassword = dbobj.SystemConfigurations.Where(x => x.Key.ToLower() == "emailpassword").Select(y => y.Value).FirstOrDefault();

                    BuyerRequestNoteEmail.BuyerNotifyEmail(SupportEmailAddress, EmailPassword, userObj, sellerRecord);
                    TempData["success"] = userObj.FirstName + " " + userObj.LastName;
                    TempData["message"] = "Your Request has been Successfully sent";
                }
                else
                {
                    TempData["internetnotconnected"] = userObj.FirstName+" "+ userObj.LastName;
                    return RedirectToAction("Index", "User");
                }
            }
            return RedirectToAction("Index", "User");
        }

        public ActionResult Error()
        {
            return View();
        }

    }
}