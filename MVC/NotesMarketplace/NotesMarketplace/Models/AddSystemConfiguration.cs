using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NotesMarketplace.Models
{
    public class AddSystemConfiguration
    {
        public string ID { get; set; }

        [Required(ErrorMessage = "Please Enter Support Email Address")]
        [EmailAddress(ErrorMessage ="Invelid Email ID")]
        [RegularExpression("^(([^<>()[\\]\\.,;:\\s@\"]+(\\.[^<>()[\\]\\.,;:\\s@\"]+)*)|(\".+\"))@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\])|(([a-zA-Z\\-0-9]+\\.)+[a-zA-Z]{2,}))$",ErrorMessage ="Envelid Email Id")]
        public string SupportEmailAddress { get; set; }

        [Required(ErrorMessage = "Please Enter Password")]
        public string EmailPassword { get; set; }

        [Required(ErrorMessage = "Please Enter Support Phone Number")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Only Number Is Allowed")]
        public string SupportPhoneNumber { get; set; }

        [Required(ErrorMessage = "Please Enter Email Address")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Please Enter Facebook URL")]
        [Url(ErrorMessage ="Invelid URL")]
        [RegularExpression("((http|https)://)(www.)?[a-zA-Z0-9@:%._\\+~#?&//=]{2,256}\\.[a-z]{2,6}\\b([-a-zA-Z0-9@:%._\\+~#?&//=]*)",ErrorMessage ="Invelid URL")]
        public string FacebookURL { get; set; }

        [Required(ErrorMessage = "Please Enter Twitter URL")]
        [Url(ErrorMessage = "Invelid URL")]
        [RegularExpression("((http|https)://)(www.)?[a-zA-Z0-9@:%._\\+~#?&//=]{2,256}\\.[a-z]{2,6}\\b([-a-zA-Z0-9@:%._\\+~#?&//=]*)", ErrorMessage = "Invelid URL")]
        public string TwitterURL { get; set; }

        [Required(ErrorMessage = "Please Enter Linkedin URL")]
        [Url(ErrorMessage = "Invelid URL")]
        [RegularExpression("((http|https)://)(www.)?[a-zA-Z0-9@:%._\\+~#?&//=]{2,256}\\.[a-z]{2,6}\\b([-a-zA-Z0-9@:%._\\+~#?&//=]*)", ErrorMessage = "Invelid URL")]
        public string LinkedinURL { get; set; }

        [Required(ErrorMessage = "Please Select Default Image For Notes")]
        public HttpPostedFileBase DefaultImageForNotes { get; set; }

        [Required(ErrorMessage = "Please Select Defaul Profile Picture")]
        public HttpPostedFileBase DefaultProfilePicture { get; set; }
    }
}