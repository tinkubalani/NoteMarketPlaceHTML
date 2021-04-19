using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NotesMarketplace.Models
{
    public class AddAdministrator
    {
        public int UserID { get; set; }

        [Required(ErrorMessage = "Please Enter Admin First Name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "only alphabet allowed !")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please Enter Admin Last Name")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "only alphabet allowed !")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please Enter Email Address")]
        [EmailAddress(ErrorMessage = "Enter valid email address")]
        [Remote("EmailExits", "Account", ErrorMessage = "Email is Already Exist")]
        [RegularExpression("^(([^<>()[\\]\\.,;:\\s@\"]+(\\.[^<>()[\\]\\.,;:\\s@\"]+)*)|(\".+\"))@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\])|(([a-zA-Z\\-0-9]+\\.)+[a-zA-Z]{2,}))$", ErrorMessage = "Envelid Email Id")]
        public string EmailID { get; set; }

        [Required(ErrorMessage = "Please select Country Code")]
        public string PhoneNumberCountryCode { get; set; }

        [Required(ErrorMessage = "Please Enter Phone Number")]
        [RegularExpression("^[0-9]{1,12}$", ErrorMessage = "Only Numerics Allowed with max 12 !")]
        public string PhoneNumber { get; set; }
    }
}