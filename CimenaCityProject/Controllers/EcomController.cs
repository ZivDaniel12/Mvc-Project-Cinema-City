using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using CimenaCityProject.Models;
using CimenaCityProject.CustomHtmlHelper;
using CimenaCityProject.ViewModels;
using CimenaCityProject.Logic;


namespace CimenaCityProject.Controllers
{
    public class EcomController : Controller
    {
        HomeCinemaContext db = new HomeCinemaContext();

        //
        ///Ecom/Movie/5
        // GET: 
        public ActionResult Movie(int? id, int? error)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // this if mmber come form the SelectChair and was error overther.. 
            if (error != null)
            {
                switch (error)
                {
                    case 1:
                        TempData["msg"] = "<script>alert('All Chair for this Time Selected..');</script>";
                        break;
                    default:
                        break;
                }

            }
            //I need to add the geoLocation system.devices.location?
            // find the movie by the ID  => y.Where(a=>a.MovieTheaters.IsActive == true)
            var viewMovieQry = new MovieData(id);


            var theatres = viewMovieQry.TimeScreening.Where(x => x.IsDisplayed == true).Distinct(new DisinctItemComparer()).Select(s => new
                {
                    HomeCinemaID = s.MovieTheaters.HomeCinemaID,
                    CinemaName = s.MovieTheaters.HomeCinema.CinemaName
                }).ToList();

            //DropDownList. 
            TimeSpan time = DateTime.Now.TimeOfDay;
                //ViewBag.HomeCinemaCity = new SelectList(db.HomeCinemas.Where(x=>x.Showing == true), "HomeCinemaID", "CinemaName");
            ViewBag.HomeCinemaCity = new SelectList(theatres, "HomeCinemaID", "CinemaName");

            ViewBag.ShowTimeList = new SelectList(db.MovieShowTimes.Where(mst => mst.MovieID == id && mst.IsDisplay == true)
                .OrderBy(x => time).ToArray(), "MovieShowTimeID", "ShowTime");

            // check if have any result in null go to ERR 400 
            if (viewMovieQry.Movie == null)
            {
                return HttpNotFound();
            }
            return View(viewMovieQry);
        }

        //POST: /encom/Movie/5
        [HttpPost]
        public ActionResult Movie(string ShowTimeList, string HomeCinemaCity, int? MovieID)
        {

            if (string.IsNullOrEmpty(ShowTimeList) || string.IsNullOrEmpty(HomeCinemaCity) || !MovieID.HasValue)
            {
                return RedirectToAction("Index", "Home");
            }

            int showtimeID = int.Parse(ShowTimeList);
            int homeCinemaID = int.Parse(HomeCinemaCity);

            List<TimeScreening> tsID = db.TimeScreening.Where(ts => ts.MovieShowTimeID == showtimeID).ToList();
            List<MovieTheaters> mtID = db.Theaters.Where(mth => mth.HomeCinemaID == homeCinemaID).ToList();

            int theatresID = 1;
            int timescreenid = 1;
            bool flag = false;
            foreach (var theatres in mtID)
            {

                // if flag is true break; 
                if (!flag)
                {
                    foreach (var tScreening in tsID)
                    {
                        if (theatres.MovieTheatersID == tScreening.MovieTheatersID && tScreening.MovieShowTimeID == showtimeID)
                        {
                            timescreenid = tScreening.TimeScreeningID;
                            theatresID = theatres.MovieTheatersID;
                            flag = true;
                            break;
                        }
                    }
                }
                else
                    break;
            }

            return RedirectToAction("Chair", new { id = showtimeID, theatresID = theatresID, timescreenID = timescreenid });
        }

        //Chair
        // GET: /Chairs/SelectChair/showTimeID
        public ActionResult Chair(int? id, int? theatresID, int? timescreenID)
        {
            if (!id.HasValue || !theatresID.HasValue || !timescreenID.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var theatresChair = new TheatersChairs(id, theatresID, timescreenID);

            return View(theatresChair);
        }

        // POST: /Chairs/SelectChair
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Chair(string[] SelectedChair, TheatersChairs theatersChairs)
        {

            if (SelectedChair != null)
            {
                // here  start to close the Event with the cartID
                string cartID = Guid.NewGuid().ToString();

                theatersChairs = new TheatersChairs(theatersChairs.TimeScreening.MovieShowTimeID,
                                                    theatersChairs.TimeScreening.MovieTheatersID,
                                                    theatersChairs.TimeScreening.TimeScreeningID);

                theatersChairs.cartID = cartID;
                try
                {
                    TempData.Clear();
                    TempData.Add("SelectedChair", SelectedChair);
                    TempData.Add("MovieShowTimeID", theatersChairs.TimeScreening.MovieShowTimeID);
                    TempData.Add("theatersChairs", theatersChairs);
                }
                catch (Exception )
                {
                }
            

                //continue to check out. 
                return RedirectToAction("CheckoutReview", new { _cartID = cartID });
            }

            else
            {
                //get back to movie via MovieID and error massage 
                return RedirectToAction("Movie", "Ecom", new { id = theatersChairs.movieID, error = "Cant add you chair to ticket, try again. " });
            }
        }

        //checkout Review get
        // GET: /Ecom/CheckoutReview/_cartID
        public ActionResult CheckoutReview(string _cartID)
        {
            bool flag = false;
            string errorMessage = string.Empty;
            int? MovieShowTimeID = (int)TempData["MovieShowTimeID"];
            string[] SelectedChair = (string[])TempData["SelectedChair"];

            if (string.IsNullOrEmpty(_cartID) || !MovieShowTimeID.HasValue || SelectedChair == null || SelectedChair.Length == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                Event evnt = new Event();
                evnt.cartID = _cartID;
                evnt.MovieShowTimeID = MovieShowTimeID.Value;

                if (ModelState.IsValid)
                {
                    db.Events.Add(evnt);
                    db.SaveChanges();
                    flag = true;
                }
                else
                {
                    flag = false;
                }

                for (int i = 0; i < SelectedChair.Length; i++)
                {
                    HallChairs hallChairSelected = db.HallChairs.Find(Convert.ToInt16(SelectedChair[i]));
                    hallChairSelected.IsSelected = true;

                    ChairsOrderd newChairOrder = new ChairsOrderd();
                    newChairOrder.EventID = evnt.EventID;
                    newChairOrder.HallChairID = hallChairSelected.HallChairsID;
                    newChairOrder.HallChairs = hallChairSelected;

                    try
                    {

                        if (ModelState.IsValid)
                        {
                            db.Entry<HallChairs>(hallChairSelected).State = EntityState.Modified;
                            db.ChairsOrderd.Add(newChairOrder);
                            db.SaveChanges();
                            flag = true;
                        }
                        else
                        {
                            flag = false;
                        }

                    }
                    catch (Exception ex)
                    {
                        errorMessage= ex.Message;
                        flag = false;
                    }
                }
                if (flag == true)
                {
                    var quryEventInfo = new EventsData(evnt, false);
                    TempData.Add("OrderDate", quryEventInfo.OrderDate);
                    TempData.Add("TotalChairOrdered", quryEventInfo.TotalChairOrdered);

                    if (string.IsNullOrEmpty(quryEventInfo.ifEror))
                    {
                        if (quryEventInfo.Event == null)
                        {
                            return HttpNotFound();
                        }
                        return View(quryEventInfo);
                    }
                    else
                    {
                        //was an error.redirect to checkout error
                        return RedirectToAction("CheckoutError", new { cartID = _cartID, message = errorMessage });
                    }
                }
                else
                {
                    //was an error.redirect to checkout error
                    return RedirectToAction("CheckoutError", new { cartID = _cartID, message = errorMessage });
                }
                
            }
            catch (Exception ex)
            {
                TempData.Add("ExeptionMessage", ex.InnerException.Message);
                //was an error.redirect to checkout error
                return RedirectToAction("CheckoutError", new { cartID = _cartID, message = errorMessage });
            }
        }

        [HttpPost]
        public ActionResult CheckoutReview(decimal TotalPrice, string cartID, int? EventID)
        {
            TheatersChairs theatersChairs = (TheatersChairs)TempData["theatersChairs"];
            DateTime OrderDate = (DateTime)TempData["OrderDate"];
            int? TotalChairOrdered = (int)TempData["TotalChairOrdered"];

            if (!EventID.HasValue || TotalPrice <= 0 || string.IsNullOrEmpty(cartID) || theatersChairs == null || OrderDate == null || !TotalChairOrdered.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Order order = EcomLogic.AddNewOrder(theatersChairs, EventID.Value, OrderDate, TotalChairOrdered.Value);

            CheckOut checkOut = new CheckOut();
            checkOut.CartId = cartID;
            checkOut.ISOrderComplete = false;
            checkOut.OrderID = order.OrderID;
            checkOut.TotalPrice = TotalPrice;
            order.IsComplete = true;
            
            try
            {
                if (ModelState.IsValid)
                {
                    db.Orders.Add(order);
                    db.CheckOut.Add(checkOut);
                    db.SaveChanges();
                }
                return RedirectToAction("CheckoutComplete", new { id = EventID });
            }
            catch (Exception ex)
            {
                //was an error.redirect to checkout error
                return RedirectToAction("CheckoutError", new { cartID = cartID, message = ex.Message });
            }
        }

        //Complete Order GET
        //GET: /Ecom/CheckOutComplete/cartID
        public ActionResult CheckoutComplete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Event evnt = db.Events.Find(id);
            var orderComplete = new EventsData(evnt, true);
            var checkout = db.CheckOut.Where(x => x.CartId == evnt.cartID).First();

            if (orderComplete.Event == null)
            {
                return View("CheckoutError");
            }

            if (ModelState.IsValid)
            {
                checkout.ISOrderComplete = true;
                db.Entry<CheckOut>(checkout).State = EntityState.Modified;
                db.SaveChanges();
            }

            return View(orderComplete);
        }

        //GET: //Ecom/CheckoutError/cartID
        public ActionResult CheckoutError(string cartID)
        {
            if (string.IsNullOrEmpty(cartID))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(cartID);
        }


        // get the ShowTime when the mmber pick a homecinema. 
        public JsonResult GetShowTime(int HomeCinemaID, int MovieID)
        {
            List<dynamic> data = new List<dynamic>();
            foreach (var showtime in db.MovieShowTimes)
            {
                foreach (var theatres in db.Theaters)
                {
                    if (showtime.MovieID == MovieID && theatres.HomeCinemaID == HomeCinemaID)
                    {
                        try
                        {
                            int mvieShowTimeID = db.TimeScreening
                                .Where(x => x.TimeScreeningID == db.TimeScreening
                                    .Where(y => y.MovieShowTimeID ==
                                        showtime.MovieShowTimeID & y.MovieTheatersID ==
                                        theatres.MovieTheatersID)
                                        .FirstOrDefault().TimeScreeningID)
                                        .First().MovieShowTimeID;

                            //add to list. 
                            data.AddRange((from time in db.MovieShowTimes
                                           where time.MovieShowTimeID == mvieShowTimeID
                                           select new
                                           {
                                               id = time.MovieShowTimeID,
                                               name = time.ShowTime
                                           }).ToList());
                        }
                        catch (Exception ex)
                        {
                            string message = ex.Message;
                            // sometime we have a homecinema but he didnt screen this movie . so for this the catch.
                        }

                    }
                }
            }
            TimeSpan timespan= DateTime.Now.TimeOfDay;
            data = data.OrderBy(x => timespan).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        // get theatres name for the Movie Detial's when the user pick a showtime. what Room he will get. 
        public JsonResult getTheatresName(int ShowTimeID)
        {
            bool flag = false;
            string data = string.Empty;
            foreach (var theatres in db.Theaters)
            {
                if (!flag)
                {
                    foreach (var timescreen in db.TimeScreening)
                    {
                        if (timescreen.MovieShowTimeID == ShowTimeID && theatres.MovieTheatersID == timescreen.MovieTheatersID)
                        {
                            data = theatres.TheatersName + "  Theater Number: " + theatres.NumberHall;
                            flag = true;
                            break;
                        }
                    }
                }
                else
                    break;

            }
            return Json(data, JsonRequestBehavior.AllowGet);

        }


        private bool AddTheDitals(List<ChairsOrderd> newChairOrder, Event newEvent)
        {
            bool flag = false;
            List<HallChairs> items = new List<HallChairs>();



            return flag;
        }


        class DisinctItemComparer : IEqualityComparer<TimeScreening> 
        {

            public bool Equals(TimeScreening x, TimeScreening y)
            {
                return x.MovieTheaters.HomeCinemaID == y.MovieTheaters.HomeCinemaID &&
                x.MovieTheaters.HomeCinema.CinemaName == y.MovieTheaters.HomeCinema.CinemaName;
            }

            public int GetHashCode(TimeScreening obj)
            {
                return obj.MovieTheaters.HomeCinema.CinemaName.GetHashCode() ^
                    obj.MovieTheaters.HomeCinemaID;
            }
        }
    }

}
