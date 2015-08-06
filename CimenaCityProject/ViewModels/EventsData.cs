using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CimenaCityProject.Models;
using CimenaCityProject.Logic;

namespace CimenaCityProject.ViewModels
{
    public class EventsData
    {
        
        private HomeCinemaContext db = new HomeCinemaContext();

        private Event evnt;
        private Movie movie;
        private MovieShowTime movieShowTime;
        //private ChairsOrderd chairsOrderd;
        private Order order;
        private MovieTheaters theaters;
        private TimeScreening timeScreening;

        public int TotalChairOrdered { get; set; }
        public List<string> ChairsNumber { get; set; }

        public string cartID { get; set; }
        public DateTime OrderDate { get; set; }
        public string ifEror { get; set; }

        //public ChairsOrderd ChairsOrderd
        //{
        //    get { return chairsOrderd; }
        //    set { chairsOrderd = value; }
        //}

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
        public EventsData()
        {

        }

        //ctor(order)
        public EventsData(Event _Event, bool OrderExist)
        {

            if (_Event != null)
            {
                try
                {
                    //first get the full implement 
                    evnt = _Event;
                    cartID = evnt.cartID;

                    // get the movieShowTime via Event.MovieShowTimeID 
                    movieShowTime = db.MovieShowTimes.Find(Event.MovieShowTimeID); 

                    movie = MovieShowTime.Movie;

                    // get the theatres item 
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

                    ChairsNumber = new List<string>();
                    int[] ChairOrderID = Event.ChairsOrderds.Where(x=>x.EventID== Event.EventID).Select(x=>x.ChairsOrderdiD).ToArray();

                    ChairsNumber.AddRange(EcomLogic.GetChairNumbers(ChairOrderID,Event.ChairsOrderds));
                    getTotalChairsOrdered();

                    OrderDate = DateTime.Now;

                    // if there is a order all fine.. if no , go to catch and make one. 
                    try
                    {
                        if (OrderExist == true)
                        {
                            order = (from odr in db.Orders
                                     where odr.EventID == evnt.EventID && odr.CartId == cartID
                                     select odr).SingleOrDefault();
                        }

                    }
                    catch (Exception)
                    {

                        ifEror = "Error by adding the Order.";
                    }

                    ifEror = null;
                }
                catch (Exception ex)
                {
                    ifEror = "There is an error while adding detial's."+ex.Message;
                }
            }
        }

        private void getTotalChairsOrdered()
        {

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