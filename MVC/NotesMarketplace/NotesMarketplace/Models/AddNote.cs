using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.IO;

namespace NotesMarketplace.Models
{
    public class AddNote
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Please Enter Note Title")]
        [StringLength(60, MinimumLength = 3)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please Select Note Category")]
        public int Category { get; set; }

        public HttpPostedFileBase DisplayPicture { get; set; }

        [Required(ErrorMessage = "Please Select A file to upload")]
        public List<HttpPostedFileBase> UploadNotes { get; set; }

        public Nullable<int> NoteType { get; set; }
        public Nullable<int> NumberofPages { get; set; }

        [Required(ErrorMessage  = "Please Enter Note Description")]
        public string Description { get; set; }
        public Nullable<int> Country { get; set; }
        public string UniversityName { get; set; }
        public string Course { get; set; }
        public string CourseCode { get; set; }
        public string Professor { get; set; }

        [Required(ErrorMessage = "Please selecet Note Type")]
        public bool IsPaid { get; set; }

        public Nullable<decimal> SellingPrice { get; set; }

        public HttpPostedFileBase NotesPreview { get; set; }
    }
}