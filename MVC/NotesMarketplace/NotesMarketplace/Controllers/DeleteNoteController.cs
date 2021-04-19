using NotesMarketplace.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace NotesMarketplace.Controllers
{
    [Authorize(Roles = "Member")]
    [RoutePrefix("DeleteNote")]
    public class DeleteNoteController : Controller
    {
        private readonly NoteMarketplaceEntities dbobj = new NoteMarketplaceEntities();

        // GET: DeleteNote
        [Route("DeleteNote/{id}")]
        public ActionResult DeleteNote(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SellerNote sellerNote = dbobj.SellerNotes.Find(id);
            if (sellerNote == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var EmailID = User.Identity.Name.ToString();
            User userObj = dbobj.Users.Where(x => x.EmailID == EmailID).FirstOrDefault();
            string path = Path.Combine(Server.MapPath("~/Members"), userObj.ID.ToString());

            string storepath = Path.Combine(Server.MapPath("~/Members/" + userObj.ID), sellerNote.ID.ToString());

            System.IO.DirectoryInfo di = new DirectoryInfo(storepath);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }

            SellerNotesAttachement sellerNotesAttachement = dbobj.SellerNotesAttachements.Where(x => x.NoteID == sellerNote.ID).FirstOrDefault();
            dbobj.SellerNotesAttachements.Remove(sellerNotesAttachement);
            dbobj.SaveChanges();

            dbobj.SellerNotes.Remove(sellerNote);
            dbobj.SaveChanges();

            return RedirectToAction("Index","User");
        }
    }
}