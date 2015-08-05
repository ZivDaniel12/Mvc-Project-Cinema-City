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
        public Rows()
        {
            this.HallChairs = new HashSet<HallChairs>();
        }

        public int RowsID { get; set; }
        public int TheatersID { get; set; }
        public int RowNumber { get; set; }
        public int ChairCapacity { get; set; }
    
        public virtual ICollection<HallChairs> HallChairs { get; set; }
        public virtual MovieTheaters MovieTheaters { get; set; }
    }
}