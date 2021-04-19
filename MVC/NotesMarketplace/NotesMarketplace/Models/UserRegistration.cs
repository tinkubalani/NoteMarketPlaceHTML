using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NotesMarketplace.Models
{
    public class UserRegistration
    {
        [Required(ErrorMessage = "Please Enter Your First Name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please Enter Your First Name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please Enter Email Address")]
        [EmailAddress(ErrorMessage = "Enter valid email address")]
        [RegularExpression("^(([^<>()[\\]\\.,;:\\s@\"]+(\\.[^<>()[\\]\\.,;:\\s@\"]+)*)|(\".+\"))@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\])|(([a-zA-Z\\-0-9]+\\.)+[a-zA-Z]{2,}))$", ErrorMessage = "Invalid Email Address")]
        [Remote("EmailExits","Account",ErrorMessage ="Email is Already Exist")]
        public string EmailID { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please Enter Password")]
        [StringLength(24, ErrorMessage = "The {0} Must be at least {2} characters long.", MinimumLength = 6)]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$", ErrorMessage = "Enter 6 to 24 characters,one upper case,one lower case,one special character")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Please Enter Confirm Password")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Password And Confirm Password Does not match")]
        public string ConfirmPassword { get; set; }
    }
}