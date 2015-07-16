using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CimenaCityProject.Models;

namespace CimenaCityProject.ViewModels
{
    public class TimeScreeningData
    {
        HomeCinemaContext db;
        public List<MovieShowTime> ShowsTime { get; set; }
        public List<MovieTheaters> Theatres { get; set; }
        public List<HomeCinema> HomeCinemas { get; set; }
        public List<Movie> movies { get; set; }
        public List<TimeScreening> timeScreening { get; set; }

        //ctor.
        public TimeScreeningData()
        {
            db = new HomeCinemaContext();
            ShowsTime = db.MovieShowTimes.ToList();
            HomeCinemas = db.HomeCinemas.ToList();
            Theatres = db.Theaters.ToList();
            movies = db.Movies.ToList();
            timeScreening = db.TimeScreening.ToList();
        }

        //ctor(terms)
        public TimeScreeningData(string termMovie = null , string termHomeCinema = null)
        {
            db = new HomeCinemaContext();
            ShowsTime = new List<MovieShowTime>();
            Theatres = new List<MovieTheaters>();
            HomeCinemas = new List<HomeCinema>();
            movies = new List<Movie>();
            timeScreening = new List<TimeScreening>();

            if (!string.IsNullOrEmpty(termMovie)&&!string.IsNullOrEmpty(termHomeCinema))
            {
                movies = db.Movies.Where(m => m.MovieName.Contains(termMovie)).GroupBy(n => n.MovieName).SelectMany(na => na).ToList();
                HomeCinemas = db.HomeCinemas.Where(x => x.CinemaName.Contains(termHomeCinema)).GroupBy(na => na.CinemaName).SelectMany(na => na).ToList();
                foreach (var movie in movies)
                {
                    ShowsTime.AddRange(db.MovieShowTimes.Where(t => t.MovieID == movie.MovieID).ToList());
                }
                foreach (var homeCinema in HomeCinemas)
                {
                    Theatres.AddRange(db.Theaters.Where(x => x.HomeCinemaID == homeCinema.HomeCinemaID).GroupBy(x => x.TheatersName).SelectMany(n => n).ToList());
                }
                foreach (var showtime in ShowsTime)
                {
                    foreach (var theater in Theatres)
                    {
                        timeScreening.AddRange(db.TimeScreening.Where(x => x.TheatresID == theater.MovieTheatersID && x.MovieShowTimeID == showtime.MovieShowTimeID).GroupBy(x => x.TimeScreeningID).SelectMany(x => x).ToList());
                    }
                }
            }
            else if (!string.IsNullOrEmpty(termMovie))
            {

                movies = db.Movies.Where(m => m.MovieName.Contains(termMovie)).GroupBy(n => n.MovieName).SelectMany(na => na).ToList();
                foreach (var movie in movies)
                {
                    ShowsTime.AddRange(db.MovieShowTimes.Where(t => t.MovieID == movie.MovieID).ToList());
                }
                foreach (var ShowTime in ShowsTime)
                { 
                    timeScreening.AddRange(db.TimeScreening.Where(x=>x.MovieShowTimeID==ShowTime.MovieShowTimeID).GroupBy(n=>n.MovieShowTimeID).SelectMany(na=>na).ToList());
                }
                foreach (var timeScreen in timeScreening)
                {
                    Theatres.AddRange(db.Theaters.Where(t => t.MovieTheatersID == timeScreen.TheatresID).GroupBy(x=>x.MovieTheatersID).SelectMany(n=>n).ToList());
                }
                foreach (var theatres in Theatres)
                {
                    
                    HomeCinemas.AddRange(db.HomeCinemas.Where(h => h.HomeCinemaID == theatres.HomeCinemaID).GroupBy(x => x.CinemaName).SelectMany(na => na).ToList());
                }
            }
            else if (!string.IsNullOrEmpty(termHomeCinema))
            {
                HomeCinemas = db.HomeCinemas.Where(x => x.CinemaName.Contains(termHomeCinema)).GroupBy(na=>na.CinemaName).SelectMany(na=>na).ToList();

                foreach (var homeCinema in HomeCinemas)
                {
                    Theatres.AddRange(db.Theaters.Where(x => x.HomeCinemaID == homeCinema.HomeCinemaID).GroupBy(x=>x.TheatersName).SelectMany(n=>n).ToList());
                }
                foreach (var thea in Theatres)
                {
                    timeScreening.AddRange(db.TimeScreening.Where(x => x.TheatresID == thea.MovieTheatersID).GroupBy(x=>x.Price).SelectMany(x=>x).ToList());
                }
                foreach (var timescreen in timeScreening)
                {
                    ShowsTime.AddRange(db.MovieShowTimes.Where(x => x.MovieShowTimeID == timescreen.MovieShowTimeID).GroupBy(x=>x.ShowTime).SelectMany(x=>x).ToList());
                }
                foreach (var showtime in ShowsTime)
                {
                    movies.AddRange(db.Movies.Where(m => m.MovieID == showtime.MovieID).GroupBy(x=>x.MovieName).SelectMany(x=>x).ToList());
                }
            }
            else
            {
                ShowsTime = db.MovieShowTimes.ToList();
                HomeCinemas = db.HomeCinemas.ToList();
                Theatres = db.Theaters.ToList();
                movies = db.Movies.ToList();
                timeScreening = db.TimeScreening.ToList();
            }
        }
    }
}