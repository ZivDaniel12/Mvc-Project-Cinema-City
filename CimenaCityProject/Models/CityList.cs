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
        
        public CityList()
        {
            this.HomeCinemas = new HashSet<HomeCinema>();
        }
        [Key]
        public int CityID { get; set; }
        public string EnglishName { get; set; }
        public string HebrewName { get; set; }
        public string ArabicName { get; set; }
        public string District { get; set; }
    
        public virtual ICollection<HomeCinema> HomeCinemas { get; set; }
    }
}