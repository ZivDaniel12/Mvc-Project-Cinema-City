using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CimenaCityProject.Models
{
    public class CityList
    {
        [Key]
        public int CityID { get; set; }
        [Display(Name = "English Name")]
        public string EnglishName { get; set; }
        [Display(Name = "Hebrew Name")]
        public string HebrewName { get; set; }
        [Display(Name = "Arabic Name")]
        public string ArabicName { get; set; }
        public string District { get; set; }

    }
}