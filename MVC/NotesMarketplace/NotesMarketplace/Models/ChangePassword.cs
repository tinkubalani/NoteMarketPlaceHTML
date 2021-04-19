using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NotesMarketplace.Models
{
    public class ChangePassword
    {
        [Display(Name = "Old Password")]
        [Required(ErrorMessage = "Please Enter Old Password")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Display(Name = "New Password")]
        [Required(ErrorMessage = "Please Enter New Password")]
        [StringLength(24, ErrorMessage = "The {0} Must be at least {2} characters long.", MinimumLength = 6)]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$", ErrorMessage = "Enter 6 to 24 characters,one upper case,one lower case,one special character")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Please Enter  Confirm Password")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "New Password And Confirm Password Does not match")]
        public string ConfirmPassword { get; set; }
    }
}