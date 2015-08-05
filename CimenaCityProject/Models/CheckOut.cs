using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CimenaCityProject.Models
{
    public class CheckOut
    {
        [Key]
        public int CheckOutID { get; set; }
        public string CartId { get; set; }
        public int OrderID { get; set; }
        public Nullable<int> PersonID { get; set; }
        public decimal TotalPrice { get; set; }
        public bool  ISOrderComplete { get; set; }

        public virtual Order Order { get; set; }
        public virtual Person Person { get; set; }


    }
}