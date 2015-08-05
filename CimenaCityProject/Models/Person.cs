using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CimenaCityProject.Models
{
    public class Person
    {

        public Person()
        {
            this.CheckOuts = new HashSet<CheckOut>();
        }

        public int PersonID { get; set; }
        public int PersonalID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public Nullable<int> PhoneNumber { get; set; }
        public System.DateTime BirthDate { get; set; }

        public virtual ICollection<CheckOut> CheckOuts { get; set; }
    }
}