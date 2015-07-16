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

        public int EventID { get; set; }
        public int MovieShowTimeID { get; set; }
        public string cartID { get; set; }

        [ForeignKey("MovieShowTimeID")]
        public virtual MovieShowTime MovieShowTime { get; set; }
       

    }
}