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

        [Key]
        public int TimeScreeningID { get; set; }

        public int MovieShowTimeID { get; set; }

        public int TheatresID { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public bool IsDisplayed { get; set; }


        public virtual ICollection<MovieShowTime> MovieShowTime { get; set; }


        public virtual MovieTheaters MovieTheaters { get; set; }

    }
}