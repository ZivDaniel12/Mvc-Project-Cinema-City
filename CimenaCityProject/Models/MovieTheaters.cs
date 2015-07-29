using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CimenaCityProject.Models
{
    /// <summary>
    /// describing the Movie Theaters numbers of Hall's 
    /// </summary>

    public class MovieTheaters
    {
        [Key]
        public int MovieTheatersID { get; set; }
   
        public int HomeCinemaID { get; set; }

        public string TheatersName { get; set; }
        public int NumberHall { get; set; }

        public int RowAmount { get; set; }
        
        //FK .
       
        public virtual HomeCinema HomeCinema { get; set; }
        public virtual ICollection<Rows> Rows { get; set; }
        public virtual ICollection<TimeScreening> TimeScreening { get; set; }
    }
}