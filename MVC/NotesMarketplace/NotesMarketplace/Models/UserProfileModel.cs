using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NotesMarketplace.Models
{
    public class UserProfileModel
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

        [Required(ErrorMessage = "Please Select Country")]
        public string Country { get; set; }

        public Nullable<System.DateTime> DOB { get; set; }
        public Nullable<int> Gender { get; set; }

        [Required(ErrorMessage = "Please Select Country Code")]
        public string PhoneNumberCountryCode { get; set; }

        [Required(ErrorMessage = "Please Enter Phone Nomber")]
        [RegularExpression("^[0-9]+$",ErrorMessage ="Only Number Is Allowed")]
        public string PhoneNumber { get; set; }
        public HttpPostedFileBase ProfilePicture { get; set; }

        [Required(ErrorMessage = "Please Enter Address Line 1")]
        public string AddressLine1 { get; set; }

        [Required(ErrorMessage = "Please Enter Address Line 2")]
        public string AddressLine2 { get; set; }

        [Required(ErrorMessage = "Please Enter City")]
        public string City { get; set; }

        [Required(ErrorMessage = "Please Enter State")]
        public string State { get; set; }

        [Required(ErrorMessage = "Please Enter Zip Code")]
        [RegularExpression("^[1-9]{1}[0-9]{5}|[1-9]{1}[0-9]{3}\\s[0-9]{3}$",ErrorMessage ="Please Enter Valid Zip Code")]
        public string ZipCode { get; set; }
        public string University { get; set; }
        public string College { get; set; }
    }
}