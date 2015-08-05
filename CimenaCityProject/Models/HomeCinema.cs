using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CimenaCityProject.Models
{
    /// <summary>
    /// Describing the HomeCinema
    /// what City he placed , address, name
    /// </summary>
    public class HomeCinema
    {

        public HomeCinema()
        {
            this.MovieTheaters = new HashSet<MovieTheaters>();
        }

        public int HomeCinemaID { get; set; }
        public string CinemaName { get; set; }
        public int CityID { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public Nullable<bool> Showing { get; set; }

        public virtual CityList CityList { get; set; }
        public virtual ICollection<MovieTheaters> MovieTheaters { get; set; }
    }
}