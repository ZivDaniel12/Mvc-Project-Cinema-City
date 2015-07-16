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
        [Key]
        public int MovieShowTimeID { get; set; }

        public int MovieID { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm:ss}")]
        public DateTime ShowTime { get; set; }

        public bool IsDisplay { get; set; }

        [ForeignKey("MovieID")]
        public virtual Movie Movie { get; set; }

        



    }
}