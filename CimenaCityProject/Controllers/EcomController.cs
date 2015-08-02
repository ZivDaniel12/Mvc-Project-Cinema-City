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
            // find the movie by the ID 
            var viewMovieQry = new MovieData(id);

            //DropDownList. 
            ViewBag.HomeCinemaCity = new SelectList(db.HomeCinemas.Where(x => x.Showing == true).ToList(), "HomeCinemaID", "CinemaName");
            ViewBag.ShowTimeList = new SelectList(db.MovieShowTimes.Where(mst => mst.MovieID == id && mst.IsDisplay == true).OrderBy(x => x.ShowTime.CompareTo(DateTime.Now)).ToArray(), "MovieShowTimeID", "ShowTime");

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

        //
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
            // here  start to close the Event with the cartID
            string cartID = Guid.NewGuid().ToString();
            foreach (var item in db.Orders)
            {
                if (item.CartId == cartID)
                {
                    cartID = Guid.NewGuid().ToString();
                }
            }
            bool flag = false;

            var theatresChair = new TheatersChairs(
                                                    theatersChairs.TimeScreening.MovieShowTimeID
                                                    , theatersChairs.TimeScreening.MovieTheatersID
                                                     , theatersChairs.TimeScreening.TimeScreeningID);

            int MovieID = db.MovieShowTimes.Where(mID => mID.MovieShowTimeID == theatersChairs.TimeScreening.MovieShowTimeID).SingleOrDefault().MovieID;

            Event newEvent = new Event();
            newEvent.cartID = cartID;
            newEvent.MovieShowTimeID = theatersChairs.TimeScreening.MovieShowTimeID;
            if (ModelState.IsValid)
            {
                db.Events.Add(newEvent);
                db.SaveChanges();
                flag = true;
            }
            else
            {
                flag = false;
                cartID = "Eror";
            }

            for (int i = 0; i < SelectedChair.Length; i++)
            {
                HallChairs hallChairSelected = db.HallChairs.Find(Convert.ToInt16(SelectedChair[i]));
                hallChairSelected.IsSelected = true;

                ChairsOrderd newChairOrder = new ChairsOrderd();
                newChairOrder.EventID = newEvent.EventID;
                newChairOrder.HallChairID = hallChairSelected.HallChairsID;

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
                        cartID = "Eror";
                        break;
                    }
                }
                catch (Exception)
                {
                    flag = false;
                    cartID = "Eror";
                    break;
                }
            }

            if (flag == true)
            {
                //continue to check out. 
                return RedirectToAction("CheckoutReview", new { _cartID = cartID });
            }
            else
            {
                //get back to movie via MovieID and error massage 
                return RedirectToAction("Movie", "Ecom", new { id = MovieID, error = "Cant add you chair to ticket, try again. " });
            }

        }

        //Order Review 
        // GET: /Ecom/CheckoutReview/_cartID
        public ActionResult CheckoutReview(string _cartID)
        {

            if (string.IsNullOrEmpty(_cartID))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string cartID = _cartID;

            Event eventOrder = db.Events.Single(c => c.cartID == cartID);
            var quryEventInfo = new TimeScreeningDetails(eventOrder);

            if (quryEventInfo.Order == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (string.IsNullOrEmpty(quryEventInfo.ifEror))
            {
                if (ModelState.IsValid)
                {
                    db.Orders.Add(quryEventInfo.Order);
                    db.SaveChanges();
                }
                else
                {
                    quryEventInfo.ifEror = "Error by adding order.";
                }

                if (quryEventInfo.Event == null)
                {
                    return HttpNotFound();
                }
                return View(quryEventInfo);
            }
            else
            {
                return View(quryEventInfo);
            }
        }

        [HttpPost]
        public ActionResult CheckoutReview(TimeScreeningDetails tsd , int? id)
        {
            // i have bug here. need to pass the all data.. 
            CheckOut checkOut = new CheckOut();
            checkOut.CartId = tsd.cartID;
            checkOut.ISOrderComplete = false;
            checkOut.OrderID = tsd.Order.OrderID;
            checkOut.TotalPrice = (tsd.TimeScreening.Price * tsd.TotalChairOrdered);

            if (ModelState.IsValid)
            {
                db.CheckOut.Add(checkOut);
                db.SaveChanges();
            }
            return RedirectToAction("CheckoutComplete", id);
        }

        //Complete Order
        //GET: /Ecom/CheckOutComplete/cartID
        public ActionResult CheckoutComplete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event evnt = db.Events.Find(id);
            var orderComplete = new TimeScreeningDetails(evnt);
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





        //Ajax Addon

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
                            data = theatres.TheatersName + "  Room Number: " + theatres.NumberHall;
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
    }
}
