using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CimenaCityProject.Models
{
    public class Event
    {
        public Event()
        {
            this.ChairsOrderds = new HashSet<ChairsOrderd>();
        }

        public int EventID { get; set; }
        public int MovieShowTimeID { get; set; }
        public string cartID { get; set; }
        public int ChairsOrderedID { get; set; }

        public virtual ICollection<ChairsOrderd> ChairsOrderds { get; set; }
        public virtual MovieShowTime MovieShowTime { get; set; }
        //public virtual Order Order { get; set; }
    }
}