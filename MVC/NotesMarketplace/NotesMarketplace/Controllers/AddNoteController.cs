using NotesMarketplace.Password_Encryption;
using NotesMarketplace.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace NotesMarketplace.Controllers
{
    [Authorize]
    [RoutePrefix("AddNote")]
    public class AddNoteController : Controller
    {
        private readonly NoteMarketplaceEntities dbobj = new NoteMarketplaceEntities();

        [Route("AddNote")]
        public ActionResult AddNote()
        {
            ViewBag.NotesCategory = dbobj.NoteCategories.Where(x => x.IsActive == true);
            ViewBag.NotesType = dbobj.NoteTypes.Where(x => x.IsActive == true);
            ViewBag.Country = dbobj.Countries.Where(x => x.IsActive == true);
            return View();
        }

        [Route("AddNote")]
        [HttpPost]
        public ActionResult AddNote(AddNote note)
        {
            if (ModelState.IsValid)
            {

                //Checking Uploaded Note Is Selected Or Not
                if (note.UploadNotes[0] == null)
                {
                    TempData["notice"] = "Select File to upload";
                    ViewBag.NotesCategory = dbobj.NoteCategories.Where(x => x.IsActive == true);
                    ViewBag.NotesType = dbobj.NoteTypes.Where(x => x.IsActive == true);
                    ViewBag.Country = dbobj.Countries.Where(x => x.IsActive == true);
                    return View();
                }

                //Checking SellingPrice is included for paid note or not
                if (note.IsPaid == true && note.SellingPrice == null)
                {
                    TempData["noticeprice"] = "Enter The Price";
                    ViewBag.NotesCategory = dbobj.NoteCategories.Where(x => x.IsActive == true);
                    ViewBag.NotesType = dbobj.NoteTypes.Where(x => x.IsActive == true);
                    ViewBag.Country = dbobj.Countries.Where(x => x.IsActive == true);
                    return View();
                }


                //Checking NotePreView is included for paid note or not
                if (note.IsPaid == true && note.NotesPreview == null)
                {
                    TempData["noticePreview"] = "Note Preview Is Required For Paid Notes";
                    ViewBag.NotesCategory = dbobj.NoteCategories.Where(x => x.IsActive == true);
                    ViewBag.NotesType = dbobj.NoteTypes.Where(x => x.IsActive == true);
                    ViewBag.Country = dbobj.Countries.Where(x => x.IsActive == true);
                    return View();
                }

                //get userid of logedin user 
                var EmailID = User.Identity.Name.ToString();
                User userObj = dbobj.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();
                string path = Path.Combine(Server.MapPath("~/Members"), userObj.ID.ToString());

                // Check for Directory, If not exist, then create it  
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                //Check User Submitted Publish button or Save Button
                var noteDataValue = "Draft";


                // get Note Status based on user clicked on Save or Publish
                ReferenceData referenceData = dbobj.ReferenceDatas.Where(x => x.RefCategory == "Notes Status" && x.DataValue == noteDataValue && x.IsActive == true).FirstOrDefault();

                //Save Book Details
                SellerNote objSellerNote = new SellerNote()
                {
                    SellerID = userObj.ID,
                    Status = referenceData.ID,
                    Title = note.Title,
                    Category = note.Category,
                    Description = note.Description,
                    IsPaid = note.IsPaid,
                    NoteType = note.NoteType,
                    NumberofPages = note.NumberofPages,
                    UniversityName = note.UniversityName,
                    Country = note.Country,
                    Course = note.Course,
                    CourseCode = note.CourseCode,
                    Professor = note.Professor,
                    CreatedDate = DateTime.Now,
                    SellingPrice = note.SellingPrice,
                    CreatedBy = userObj.ID,
                    IsActive = true
                };

                //Save Note To Database
                dbobj.SellerNotes.Add(objSellerNote);
                dbobj.SaveChanges();


                //Get Saved Notes ID

                var noteID = objSellerNote.ID;

                //Generate Path To Store Image
                string storepath = Path.Combine(Server.MapPath("~/Members/" + userObj.ID), noteID.ToString());

                // Check for Directory, If not exist, then create it  
                if (!Directory.Exists(storepath))
                {
                    Directory.CreateDirectory(storepath);
                }

                //Store The DisplayPicture If Uploaded
                if (note.DisplayPicture != null && note.DisplayPicture.ContentLength > 0)
                {
                    string fileName = Path.GetFileNameWithoutExtension(note.DisplayPicture.FileName);
                    string extension = Path.GetExtension(note.DisplayPicture.FileName);
                    fileName = "DP_" + DateTime.Now.ToString("ddMMyyyy") + extension;
                    string finalpath = Path.Combine(storepath, fileName);
                    note.DisplayPicture.SaveAs(finalpath);

                    objSellerNote.DisplayPicture = Path.Combine(("/Members/" + userObj.ID + "/" + noteID + "/"), fileName);
                    dbobj.SaveChanges();
                }
                else
                {
                    SystemConfiguration systemConfiguration = dbobj.SystemConfigurations.Where(x => x.Key.ToLower() == "defaultimagefornotes").FirstOrDefault();
                    objSellerNote.DisplayPicture = systemConfiguration.Value;
                    dbobj.SaveChanges();
                }

                //Store The NotesPreview If Uploaded
                if (note.NotesPreview != null && note.NotesPreview.ContentLength > 0)
                {
                    string fileName = Path.GetFileNameWithoutExtension(note.NotesPreview.FileName);
                    string extension = Path.GetExtension(note.NotesPreview.FileName);
                    fileName = "Preview_" + DateTime.Now.ToString("ddMMyyyy") + extension;
                    string finalpath = Path.Combine(storepath, fileName);
                    note.NotesPreview.SaveAs(finalpath);

                    objSellerNote.NotesPreview = Path.Combine(("/Members/" + userObj.ID + "/" + noteID + "/"), fileName);
                    dbobj.SaveChanges();
                }

                //Create Path To Store Attachement
                string attachementsstorepath = Path.Combine(storepath, "Attachements");

                // Check for Directory, If not exist, then create it  
                if (!Directory.Exists(attachementsstorepath))
                {
                    Directory.CreateDirectory(attachementsstorepath);
                }

                //Create Object of SellerNotesAttachement Table and Store Data
                SellerNotesAttachement sellerNotesAttachement = new SellerNotesAttachement
                {
                    NoteID = noteID,
                    ISActive = true,
                    CreatedBy = userObj.ID,
                    CreatedDate = DateTime.Now
                };

                //Store The Attached File
                int Count = 1;
                var FilePath = "";
                var FileName = "";
                long FileSize = 0;
                foreach (var file in note.UploadNotes)
                {
                    FileSize += ((file.ContentLength) / 1024);
                    string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    string extension = Path.GetExtension(file.FileName);
                    fileName = "Attachement" + Count + "_" + DateTime.Now.ToString("ddMMyyyy") + extension;
                    string finalpath = Path.Combine(attachementsstorepath, fileName);
                    file.SaveAs(finalpath);

                    FileName += fileName + ";";
                    FilePath += Path.Combine(("/Members/" + userObj.ID + "/" + noteID + "/Attachements/"), fileName) + ";";

                    Count++;
                }

                //Save the Detail To DataBase
                sellerNotesAttachement.AttachementSize = FileSize;
                sellerNotesAttachement.FileName = FileName;
                sellerNotesAttachement.FilePath = FilePath;
                dbobj.SellerNotesAttachements.Add(sellerNotesAttachement);
                dbobj.SaveChanges();

                TempData["success"] = userObj.FirstName+ " " + userObj.LastName;
                TempData["message"] = "Note has been added";
                return RedirectToAction("Index", "User");
            }
            ViewBag.NotesCategory = dbobj.NoteCategories.Where(x => x.IsActive == true);
            ViewBag.NotesType = dbobj.NoteTypes.Where(x => x.IsActive == true);
            ViewBag.Country = dbobj.Countries.Where(x => x.IsActive == true);
            return View();
        }
    }
}