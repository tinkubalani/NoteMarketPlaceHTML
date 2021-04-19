using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NotesMarketplace.Models
{
    public class AddCountry
    {
        public int CoutryID { get; set; }

        [Required(ErrorMessage = "Please Enter Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please Enter Country Code")]
        public string CountryCode { get; set; }
    }
}