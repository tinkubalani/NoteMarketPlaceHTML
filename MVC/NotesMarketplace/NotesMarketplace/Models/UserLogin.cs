using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NotesMarketplace.Models
{
    public class UserLogin
    {
        [Display(Name = "User Email")]
        [Required(ErrorMessage = "Please Enter Email Address")]
        [EmailAddress(ErrorMessage = "Enter valid email address")]
        public string EmailID { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please Enter Password")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}