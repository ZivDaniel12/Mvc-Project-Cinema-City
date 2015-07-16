using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CimenaCityProject.Models;

namespace CimenaCityProject.ViewModels
{
    public class OrderDetails
    {
        //for PayPalAPI
        public IEnumerable<Movie> Movie { get; set; }

        public IEnumerable<TimeScreening> TimeScreening { get; set; }

        public IEnumerable<Order> Order { get; set; }

        public IEnumerable<ChairsOrderd> ChairOrderd { get; set; }

        public int ChairsOrderd { get; set; }

        public OrderDetails()
        {

        }
    }
}