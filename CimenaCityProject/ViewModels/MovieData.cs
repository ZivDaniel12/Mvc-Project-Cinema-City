using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CimenaCityProject.Models;


namespace CimenaCityProject.ViewModels
{
    public class MovieData
    {
        private HomeCinemaContext db = new HomeCinemaContext();
        public IEnumerable<Movie> Movie { get; set; }
        public IEnumerable<MovieShowTime> MovieShowTime { get; set; }
        public IEnumerable<HomeCinema> HomeCinema { get; set; }

        public MovieData()
        {
            Movie = (from me in db.Movies select me).ToArray();
            MovieShowTime = (from mst in db.MovieShowTimes orderby mst.ShowTime.Hour select mst).ToArray();
            HomeCinema = (from hc in db.HomeCinemas select hc).ToArray();
        }
        public MovieData(int? id)
        {
            Movie = (from me in db.Movies
                     where me.MovieID == id
                     select me).ToArray();

            // find all the movie show time by the MovieID 
            MovieShowTime = (from mst in db.MovieShowTimes
                             where mst.MovieID == id
                             select mst).ToArray();
        }

    }
}