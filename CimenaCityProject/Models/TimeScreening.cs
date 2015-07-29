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

        public int MovieTheatersID { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public bool IsDisplayed { get; set; }

      //  [ForeignKey("MovieShowTimeID")]
        public virtual MovieShowTime MovieShowTime { get; set; }

     //   [ForeignKey("MovieTheatersID")]
        public virtual MovieTheaters MovieTheaters { get; set; }

        //[ForeignKey("TimeScreeningID")]
        //public virtual Order Order { get; set; }
    }
}