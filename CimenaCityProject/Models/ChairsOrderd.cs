﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CimenaCityProject.Models
{
    public class ChairsOrderd
    {
        [Key]
        public int ChairsOrderdiD { get; set; }

        public int HallChairID { get; set; }

        public int EventID { get; set; }

       
        public virtual ICollection<HallChairs> HallChairs { get; set; }
        
        public virtual ICollection<Event> Event { get; set; }
        
    }
}