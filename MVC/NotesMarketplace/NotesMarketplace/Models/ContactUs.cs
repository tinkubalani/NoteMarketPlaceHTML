using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NotesMarketplace.Models
{
    public class ContactUs
    {
        [Required(ErrorMessage = "Please Enter Your Full Name")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "only alphabet allowed !")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Please Enter Email Address")]
        [EmailAddress(ErrorMessage = "Enter valid email address")]
        public string EmailID { get; set; }

        [Required(ErrorMessage = "Please Enter Subject")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Please Enter Questions/Comments")]
        public string Comments { get; set; }
    }
}