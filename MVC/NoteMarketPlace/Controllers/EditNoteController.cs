using NoteMarketPlace.EmailTemplates;
using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace NoteMarketPlace.Controllers
{
    [Authorize]
    [RoutePrefix("EditNote")]
    public class EditNoteController : Controller
    {
        private readonly NoteMarketplaceEntities dbobj = new NoteMarketplaceEntities();

        [Route("EditNote/{id}")]
        // GET: EditNote
        public ActionResult EditNote(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SellerNote sellerNote = dbobj.SellerNotes.Find(id);
            SellerNotesAttachement attachment = dbobj.SellerNotesAttachements.Where(x => x.NoteID == sellerNote.ID).FirstOrDefault();

            AddNote sellerNote1 = new AddNote()
            {
                ID = sellerNote.ID,
                Title = sellerNote.Title,
                Category = sellerNote.Category,
                Description = sellerNote.Description,
                IsPaid = sellerNote.IsPaid,
                NoteType = sellerNote.NoteType,
                NumberofPages = sellerNote.NumberofPages,
                UniversityName = sellerNote.UniversityName,
                Country = sellerNote.Country,
                Course = sellerNote.Course,
                CourseCode = sellerNote.CourseCode,
                Professor = sellerNote.Professor,
                SellingPrice = sellerNote.SellingPrice
            };

            if (sellerNote == null)
            {
                return HttpNotFound();
            }

            ViewBag.NotePriview = sellerNote.NotesPreview;
            ViewBag.AttechmentPath = attachment.FilePath;
            ViewBag.DP = sellerNote.DisplayPicture;
            ViewBag.Country = new SelectList(dbobj.Countries.Where(x => x.IsActive == true), "ID", "Name", sellerNote.Country);
            ViewBag.Category = new SelectList(dbobj.NoteCategories.Where(x => x.IsActive == true), "ID", "Name", sellerNote.Category);
            ViewBag.NoteType = new SelectList(dbobj.NoteTypes.Where(x => x.IsActive == true), "ID", "Name", sellerNote.NoteType);
            return View(sellerNote1);
        }

        [Route("EditNote/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditNote(int? id, AddNote note, string submit)
        {
            if (ModelState.IsValid)
            {

                //Check UploadNote Is Selected Or Not
                if (note.UploadNotes[0] == null)
                {
                    TempData["notice"] = "Select File to upload";
                    ViewBag.Country = new SelectList(dbobj.Countries.Where(x => x.IsActive == true), "ID", "Name", note.Country);
                    ViewBag.Category = new SelectList(dbobj.NoteCategories.Where(x => x.IsActive == true), "ID", "Name", note.Category);
                    ViewBag.NoteType = new SelectList(dbobj.NoteTypes.Where(x => x.IsActive == true), "ID", "Name", note.NoteType);
                    return View(note);
                }

                //Check SellingPrice is included for paid note or not
                if (note.IsPaid == true && note.SellingPrice == null)
                {
                    TempData["noticeprice"] = "Enter The Price";
                    ViewBag.Country = new SelectList(dbobj.Countries.Where(x => x.IsActive == true), "ID", "Name", note.Country);
                    ViewBag.Category = new SelectList(dbobj.NoteCategories.Where(x => x.IsActive == true), "ID", "Name", note.Category);
                    ViewBag.NoteType = new SelectList(dbobj.NoteTypes.Where(x => x.IsActive == true), "ID", "Name", note.NoteType);
                    return View(note);
                }


                //Check NotePreView is included for paid note or not
                if (note.IsPaid == true && note.NotesPreview == null)
                {
                    TempData["noticePreview"] = "Note Preview Is Required For Paid Notes";
                    ViewBag.Country = new SelectList(dbobj.Countries.Where(x => x.IsActive == true), "ID", "Name", note.Country);
                    ViewBag.Category = new SelectList(dbobj.NoteCategories.Where(x => x.IsActive == true), "ID", "Name", note.Category);
                    ViewBag.NoteType = new SelectList(dbobj.NoteTypes.Where(x => x.IsActive == true), "ID", "Name", note.NoteType);
                    return View(note);
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
                var noteDataValue = "";

                if (submit == "Publish")
                {
                    noteDataValue = "Submitted For Review";

                    //Notify Admin If Seller Publish The Book
                    SellerPublishedNoteEmail.SellerPublishedNoteNotifyEmail(userObj, note.Title);
                }
                else if (submit == "Save")
                {
                    noteDataValue = "Draft";
                }


                // get Note Status based on user clicked on Save or Publish
                ReferenceData referenceData = dbobj.ReferenceDatas.Where(x => x.RefCategory == "Notes Status" && x.DataValue == noteDataValue && x.IsActive == true).FirstOrDefault();

                //Save Book Details
                SellerNote objSellerNote = dbobj.SellerNotes.Where(x => x.ID == note.ID).FirstOrDefault();
                objSellerNote.Status = referenceData.ID;
                objSellerNote.Title = note.Title;
                objSellerNote.Category = note.Category;
                objSellerNote.Description = note.Description;
                objSellerNote.IsPaid = note.IsPaid;
                objSellerNote.NoteType = note.NoteType;
                objSellerNote.NumberofPages = note.NumberofPages;
                objSellerNote.UniversityName = note.UniversityName;
                objSellerNote.Country = note.Country;
                objSellerNote.Course = note.Course;
                objSellerNote.CourseCode = note.CourseCode;
                objSellerNote.Professor = note.Professor;
                objSellerNote.ModifiedDate = DateTime.Now;
                objSellerNote.ModifiedBy = userObj.ID;
                objSellerNote.SellingPrice = note.SellingPrice;
                objSellerNote.CreatedBy = userObj.ID;
                objSellerNote.IsActive = true;

                //Save Note To Database
                dbobj.Entry(objSellerNote).State = EntityState.Modified;
                dbobj.SaveChanges();

                //Get Saved Notes ID
                var noteID = objSellerNote.ID;

                //Generate Path To Store Image
                string storepath = Path.Combine(Server.MapPath("~/Members/" + userObj.ID), noteID.ToString());

                System.IO.DirectoryInfo di = new DirectoryInfo(storepath);

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }

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
                    objSellerNote.DisplayPicture = "/SystemConfigurations/DefaultImages/DefaultNoteImage.jpg";
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
                else
                {
                    objSellerNote.NotesPreview = null;
                }

                //Create Path To Store Attachement
                string attachementsstorepath = Path.Combine(storepath, "Attachements");

                // Check for Directory, If not exist, then create it  
                if (!Directory.Exists(attachementsstorepath))
                {
                    Directory.CreateDirectory(attachementsstorepath);
                }

                //Create Object of SellerNotesAttachement Table and Store Data
                SellerNotesAttachement sellerNotesAttachement = dbobj.SellerNotesAttachements.Where(x => x.NoteID == noteID).FirstOrDefault();
                sellerNotesAttachement.ModifiedDate = DateTime.Now;
                sellerNotesAttachement.ModifiedBy = userObj.ID;

                //Store The Attached File
                int Count = 1;
                var FilePath = "";
                var FileName = "";
                foreach (var file in note.UploadNotes)
                {
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
                sellerNotesAttachement.FileName = FileName;
                sellerNotesAttachement.FilePath = FilePath;
                dbobj.Entry(sellerNotesAttachement).State = EntityState.Modified;
                dbobj.SaveChanges();

                return RedirectToAction("Index", "User");
            }
            ViewBag.Country = new SelectList(dbobj.Countries.Where(x => x.IsActive == true), "ID", "Name", note.Country);
            ViewBag.Category = new SelectList(dbobj.NoteCategories.Where(x => x.IsActive == true), "ID", "Name", note.Category);
            ViewBag.NoteType = new SelectList(dbobj.NoteTypes.Where(x => x.IsActive == true), "ID", "Name", note.NoteType);
            return View(note);
        }

        public ActionResult DownloadPicture(int? id)
        {
            SellerNote note = dbobj.SellerNotes.Find(id);

            var displaypath = note.DisplayPicture;

            /* var allFilesPath = attechment.FilePath.Split(';');*/

            string FullPath = Path.Combine(Server.MapPath("~" + displaypath));
            string FileName = Path.GetFileName(FullPath);
            return File(FullPath, "image/*", FileName);
        }

        public ActionResult DownloadPreview(int? id)
        {
            SellerNote note = dbobj.SellerNotes.Find(id);

            var Priviewpath = note.NotesPreview;

            /* var allFilesPath = attechment.FilePath.Split(';');*/

            string FullPath = Path.Combine(Server.MapPath("~" + Priviewpath));
            string FileName = Path.GetFileName(FullPath);
            return File(FullPath, "application/pdf", FileName);
        }

        public ActionResult DownloadAttechedFile(int? id)
        {
            SellerNotesAttachement attechment = dbobj.SellerNotesAttachements.Where(x => x.NoteID == id).FirstOrDefault();

            var allFilesPath = attechment.FilePath.Split(';');
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
    }
}