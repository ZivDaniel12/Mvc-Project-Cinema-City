using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CimenaCityProject.Models
{
    public class HallChairs
    {
        public HallChairs()
        {
            this.ChairsOrderds = new HashSet<ChairsOrderd>();
        }
    
        public int HallChairsID { get; set; }
        public int RowID { get; set; }
        public int ChairNumber { get; set; }
        public bool IsSelected { get; set; }
    
        public virtual ICollection<ChairsOrderd> ChairsOrderds { get; set; }
        public virtual Rows Rows { get; set; }
      
        
    }
}