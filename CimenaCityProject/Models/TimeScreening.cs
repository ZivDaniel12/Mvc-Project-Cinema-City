using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CimenaCityProject.Models
{
    public class TimeScreening
    {

        public TimeScreening()
        {
        }

        public int TimeScreeningID { get; set; }
        public int MovieShowTimeID { get; set; }
        public int MovieTheatersID { get; set; }
        public System.DateTime Date { get; set; }
        public decimal Price { get; set; }
        public bool IsDisplayed { get; set; }

        public virtual MovieShowTime MovieShowTime { get; set; }
        public virtual MovieTheaters MovieTheaters { get; set; }
        public virtual Order Order { get; set; }

    }
}