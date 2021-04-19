using NotesMarketplace.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NotesMarketplace.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    [RoutePrefix("Admin")]
    public class AdminDownloadNoteController : Controller
    {
        private readonly NoteMarketplaceEntities dbobj = new NoteMarketplaceEntities();

        [Route("DownloadPicture/{id}")]
        public ActionResult DownloadPicture(int? id)
        {
            UserProfile userProfile = dbobj.UserProfiles.Where(x => x.UserID == id).FirstOrDefault();

            var displaypath = userProfile.ProfilePicture;

            /* var allFilesPath = attechment.FilePath.Split(';');*/

            string FullPath = Path.Combine(Server.MapPath("~" + displaypath));

            string FileName = Path.GetFileName(FullPath);
            return File(FullPath, "image/*", FileName);
        }

        [Route("DownloadAttechedFile/{id}")]
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