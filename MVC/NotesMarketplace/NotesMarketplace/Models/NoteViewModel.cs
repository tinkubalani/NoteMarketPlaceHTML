using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotesMarketplace.Models
{
    public class UserInProgressNoteViewModel
    {
        public SellerNote SellerNotes { get; set; }
        public NoteCategory NoteCategories { get; set; }
        public ReferenceData ReferenceDatas { get; set; }
    }

    public class UserPublishedNoteViewModel
    {
        public SellerNote SellerNotes { get; set; }
        public NoteCategory NoteCategories { get; set; }
        public ReferenceData ReferenceDatas { get; set; }
    }

    public class  AllPublishedNotes
    {
        public SellerNote SellerNotes { get; set; }
        public NoteCategory NoteCategories { get; set; }
        public ReferenceData ReferenceDatas { get; set; }
        public Country Countries { get; set; }
        public SellerNotesReview SellerNotesReview { get; set; }
        public string Rating { get; set; }
    }

    public class BuyerRequestNotes
    {
        public Download DownloadNotes { get; set; }
        public User UserData { get; set; }
        public UserProfile UserProfileData { get; set; }
    }

    public class MyDownloads
    {
        public Download DownloadNotes { get; set; }
        public User UserData { get; set; }
        public User SellerData { get; set; }
    }

    public class MySoldNote
    {
        public Download DownloadNotes { get; set; }

        public User UserData { get; set; }
    }

    public class MyRejectedNote
    {
        public SellerNote SellerNote { get; set; }

        public NoteCategory Category { get; set; }
    }
}