using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CimenaCityProject.Models
{
    public class Order
    {

        public Order()
        {
            this.CheckOuts = new HashSet<CheckOut>();
            this.TimeScreening = new HashSet<TimeScreening>();
        }

        public int OrderID { get; set; }
        public int TimeScreeningID { get; set; }
        public int EventID { get; set; }
        public string CartId { get; set; }
        public int TotalChairsOrdered { get; set; }
        public System.DateTime OrderDate { get; set; }
        public Nullable<bool> IsComplete { get; set; }

        public virtual ICollection<CheckOut> CheckOuts { get; set; }
        public virtual Event Event { get; set; }
        public virtual ICollection<TimeScreening> TimeScreening { get; set; }


        
    }
}