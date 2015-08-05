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
        public Genre()
        {
            this.Movies = new HashSet<Movie>();
        }

        public int GenreID { get; set; }
        public string EnglishName { get; set; }
        public string HebrewName { get; set; }
        public string ArabicName { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }
    }
}