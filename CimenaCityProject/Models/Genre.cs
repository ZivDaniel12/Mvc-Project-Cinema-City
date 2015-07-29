using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CimenaCityProject.Models
{
    public class Genre
    {
        [Key]
        public int GenreID { get; set; }
        [Display(Name = "English Name")]
        public string EnglishName { get; set; }
        [Display(Name = "Hebrew Name")]
        public string HebrewName { get; set; }
        [Display(Name = "Arabic Name")]
        public string ArabicName { get; set; }

    }
}