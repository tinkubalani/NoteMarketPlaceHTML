using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NotesMarketplace.Models
{
    public class AdminProfile
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Please Enter Your First Name")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only please")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please Enter Your First Name")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only please")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please Enter Email Address")]
        [EmailAddress(ErrorMessage = "Enter valid email address")]
        [RegularExpression("^(([^<>()[\\]\\.,;:\\s@\"]+(\\.[^<>()[\\]\\.,;:\\s@\"]+)*)|(\".+\"))@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\])|(([a-zA-Z\\-0-9]+\\.)+[a-zA-Z]{2,}))$", ErrorMessage = "Envelid Email Id")]
        public string EmailID { get; set; }

        [EmailAddress(ErrorMessage = "Enter valid email address")]
        public string SecondaryEmail { get; set; }

        [Required(ErrorMessage = "Please Select Country Code")]
        public string PhoneNumberCountryCode { get; set; }

        [Required(ErrorMessage = "Please Enter Phone Nomber")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Only Number Is Allowed")]
        public string PhoneNumber { get; set; }
        public HttpPostedFileBase ProfilePicture { get; set; }
    }
}