using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CimenaCityProject.Models
{
    /// <summary>
    /// Describing the Time each Movie will show up
    /// </summary>
    public class MovieShowTime
    {
        public MovieShowTime()
        {
            this.Events = new HashSet<Event>();
            this.TimeScreening = new HashSet<TimeScreening>();
        }

        public int MovieShowTimeID { get; set; }
        public int MovieID { get; set; }
        public System.DateTime ShowTime { get; set; }
        public bool IsDisplay { get; set; }

        public virtual ICollection<Event> Events { get; set; }
        public virtual Movie Movie { get; set; }
        public virtual ICollection<TimeScreening> TimeScreening { get; set; }
    }
}