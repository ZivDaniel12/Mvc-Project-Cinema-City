using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


using CimenaCityProject.Models;

namespace CimenaCityProject.ViewModels
{
    public class TheatersChairs
    {

        private HomeCinemaContext db;

        private MovieTheaters theaters;
        private TimeScreening timeScreening;
        private ChairsOrderd chairsOrderd;
        private List<Rows> rows;
        private List<HallChairs> hallChairs;
        public int movieID { get; set; }
        public string cartID { get; set; }

        // read only.
        public ChairsOrderd ChairsOrderd
        {
            get { return chairsOrderd; }
            set { chairsOrderd = value; }
        }
        
        public TimeScreening TimeScreening
        {
            get { return timeScreening; }
            set { timeScreening = value; }
        }

        public MovieTheaters Theaters
        {
            get { return theaters; }
            set { theaters = value; }
        }

        //public HomeCinema HomeCinema
        //{
        //    get { return homeCinema; }
        //    set { homeCinema = value; }
        //}

        public List<Rows> Rows
        {
            get { return rows; }
            set { rows = value; }
        }

        public List<HallChairs> HallChairs
        {
            get { return hallChairs; }
            set { hallChairs = value; }
        }
       

        //ctor. 
        public TheatersChairs()
        {
            db = new HomeCinemaContext();
        }

        public TheatersChairs(int? _showTimeID, int? _theatresID, int? timescreenID)
        {

            db= new HomeCinemaContext();

            movieID = (from movid in db.Movies
                       where movid.MovieID ==
                       (from mst in db.MovieShowTimes
                        where mst.MovieShowTimeID == _showTimeID
                        select mst).FirstOrDefault().MovieID
                       select movid).SingleOrDefault().MovieID;

            theaters = (from mt in db.Theaters
                        where mt.MovieTheatersID == _theatresID
                        select mt).SingleOrDefault();

            theaters.HomeCinema = db.HomeCinemas.Find(theaters.HomeCinemaID);

            timeScreening = db.TimeScreening.Find(timescreenID);

            rows = (from rws in db.Rows
                    where rws.TheatersID == theaters.MovieTheatersID
                    select rws).ToList();

            hallChairs = new List<HallChairs>();

            //adding hallChairs item's 
            foreach (var rowItem in rows)
            {
                List<HallChairs> hallChairCollection = new List<Models.HallChairs>();
                 
                hallChairCollection = (from hc in db.HallChairs
                                       where hc.RowID == rowItem.RowsID
                                       select hc).ToList();

                foreach (var hallchairItem in hallChairCollection)
                {
                    hallChairs.Add(hallchairItem);
                }
            }

        }
    }
}