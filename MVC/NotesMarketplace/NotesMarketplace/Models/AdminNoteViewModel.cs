using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotesMarketplace.Models
{
    public class AllPublishedNoteViewModel
    {
        public SellerNote SellerNotes { get; set; }
        public NoteCategory NoteCategories { get; set; }
        public ReferenceData ReferenceDatas { get; set; }
        public Download DownloadNotes { get; set; }
        public User UserData { get; set; }
        public SellerNotesAttachement Attachment { get; set; }
    }

    public class UnderReviewsNote
    {
        public SellerNote NoteDetails { get; set; }

        public NoteCategory Category { get; set; }

        public ReferenceData Status { get; set; }
        public User User { get; set; }

    }

    public class PublishedNoteAdmin
    {
        public SellerNote NoteDetails { get; set; }
        public NoteCategory Category { get; set; }
        public ReferenceData Status { get; set; }
        public User User { get; set; }
        public User Admin { get; set; }

    }

    public class RejectedNoteAdmin
    {
        public SellerNote NoteDetails { get; set; }

        public NoteCategory Category { get; set; }

        public ReferenceData Status { get; set; }
        public User User { get; set; }
        public User Admin { get; set; }

    }

    public class AllNotesOfMember
    {
        public SellerNote SellerNotes { get; set; }
        public NoteCategory NoteCategories { get; set; }
        public ReferenceData ReferenceDatas { get; set; }
        public Download Downloads { get; set; }

    }

    public class AdminDownloadNotes
    {
        public Download Downloads { get; set; }
        public User Seller { get; set; }
        public User Buyer { get; set; }

    }

    public class SpamReportedAdmin
    {
        public SellerNotesReportedIssue Reports { get; set; }
        public SellerNote NoteDetails { get; set; }
        public NoteCategory Category { get; set; }
        public User user { get; set; }
    }
}