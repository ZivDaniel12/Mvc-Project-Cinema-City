using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using CimenaCityProject.Models;
using CimenaCityProject.ViewModels;

namespace CimenaCityProject.ViewModels
{
    public class TimeScreeningDetails
    {

        private HomeCinemaContext db = new HomeCinemaContext();

        private Event evnt;
        private Movie movie;
        private MovieShowTime movieShowTime;
        private ChairsOrderd chairsOrderd;
        private Order order;
        private MovieTheaters theaters;
        private TimeScreening timeScreening;

        public int TotalChairOrdered { get; set; }
        public List<int> ChairNumbers { get; set; }
        public List<int> RowNumber { get; set; }

        public string cartID { get; set; }

        public string ifEror { get; set; }

        public ChairsOrderd ChairsOrderd
        {
            get { return chairsOrderd; }
            set { chairsOrderd = value; }
        }

        public Order Order
        {
            get { return order; }

        }

        public Event Event
        {
            get { return evnt; }
        }

        public Movie Movie
        {
            get { return movie; }
        }

        public MovieTheaters MovieTheaters
        {
            get { return theaters; }
        }

        public TimeScreening TimeScreening
        {
            get { return timeScreening; }
        }

        public MovieShowTime MovieShowTime
        {
            get { return movieShowTime; }
        }


        //ctor()
        public TimeScreeningDetails()
        {

        }

        //ctor(order)
        public TimeScreeningDetails(Event _Event)
        {

            if (_Event != null)
            {
                try
                {
                    //first get the full implement 
                    evnt = _Event;
                    cartID = evnt.cartID;

                    // get the movieShowTime via timeScreeningID 
                    movieShowTime = (from mst in db.MovieShowTimes
                                     where
                                         (evnt.MovieShowTimeID) == mst.MovieShowTimeID
                                     select mst).SingleOrDefault();

                    // get the movie item. via the TimeScreeningID via MovieShowTimeID 
                    movie = (from mv in db.Movies
                             where mv.MovieID == movieShowTime.MovieID
                             select mv).SingleOrDefault();

                    // get the theatres item via timeScreeningID. 
                    theaters = (from theat in db.Theaters
                                where theat.MovieTheatersID ==
                                (from rw in db.Rows
                                 where rw.RowsID ==
                                 (from hc in db.HallChairs
                                  where hc.HallChairsID ==
                                  (from co in db.ChairsOrderd
                                   where co.EventID == Event.EventID
                                   select co).FirstOrDefault().HallChairID
                                  select hc).FirstOrDefault().RowID
                                 select rw).FirstOrDefault().TheatersID
                                select theat).SingleOrDefault();

                    // get the TimeScreening item
                    timeScreening = (from ts in db.TimeScreening
                                     where _Event.MovieShowTimeID == ts.MovieShowTimeID && ts.MovieTheatersID == theaters.MovieTheatersID
                                     select ts).SingleOrDefault();

                    ChairNumbers = new List<int>();
                    RowNumber = new List<int>();

                  

                    getTotalChairsOrdered();

                    

                    // if there is a order all fine.. if no , go to catch and make one. 
                    try
                    {
                        order = (from odr in db.Orders
                                 where odr.EventID == evnt.EventID && odr.CartId == cartID
                                 select odr).SingleOrDefault();

                        if (order == null)
                        {
                            order = new Order();
                            order.TimeScreeningID = timeScreening.TimeScreeningID;
                            order.CartId = cartID;
                            order.TotalChairsOrdered = TotalChairOrdered;
                            order.EventID = evnt.EventID;
                            order.OrderDate = DateTime.Now;
                        }
                    }
                    catch (Exception)
                    {
                        
                        order = new Order();
                        order.TimeScreeningID = timeScreening.TimeScreeningID;
                        order.CartId = cartID;
                        order.TotalChairsOrdered = TotalChairOrdered;
                        order.EventID = evnt.EventID;
                        order.OrderDate = DateTime.Now;
                    }

                    ifEror = null;
                }
                catch (Exception)
                {
                    ifEror = "Cannot load Order Details";
                }
            }
        }

        private void getTotalChairsOrdered()
        {
            //get how many chaires ordered... 
            //what is the chairs number
            //what is row number
            
            foreach (var item in db.ChairsOrderd)
            {
                if (item.EventID == evnt.EventID)
                {
                    TotalChairOrdered++;


                }
            }
        }


 
    }
}