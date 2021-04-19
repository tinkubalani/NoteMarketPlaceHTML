using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NotesMarketplace.Models
{
    public class ForgotPassword
    {
        [Required(ErrorMessage = "Please Enter Email Address")]
        [EmailAddress(ErrorMessage = "Enter valid email address")]
        public string EmailID { get; set; }
    }
}