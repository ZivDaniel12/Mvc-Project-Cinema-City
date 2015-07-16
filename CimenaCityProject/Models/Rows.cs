using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CimenaCityProject.Models
{

    public class Rows
    {
        [Key]
        public int RowsID { get; set; }
  
        public int TheatersID { get; set; }

        [DisplayFormat(DataFormatString="Row Number")]
        public int RowNumber { get; set; }

        public int ChairCapacity { get; set; }

        //FK => TheatersID
        [ForeignKey("TheatersID")]
        public virtual MovieTheaters MovieTheaters { get; set; }

        public virtual ICollection<HallChairs> HallChairs { get; set; }
    }
}