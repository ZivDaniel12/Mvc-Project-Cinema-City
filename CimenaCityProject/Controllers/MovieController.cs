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

using ImageResizer;
using System.Drawing;
using System.IO;


namespace CimenaCityProject.Controllers
{
    public class MovieController : Controller
    {
        private HomeCinemaContext db = new HomeCinemaContext();
        
        // GET: /Movie/
        public ActionResult Index(int? id, int? MovieShowTimeID)
        {
            var viewMovieQry = new MovieData();

            if (id != null)
            {
                ViewBag.MovieDataID = id.Value;
                viewMovieQry.MovieShowTime = viewMovieQry.Movie.
                    Where(i => i.MovieID == id.Value).Single().MovieShowTimes;
            }

            if (MovieShowTimeID != null)
            {
                ViewBag.movieShowTimeID = MovieShowTimeID.Value;
                viewMovieQry.Movie = viewMovieQry.Movie.Where
                    (i => i.MovieID == viewMovieQry.MovieShowTime.Where
                        (d => d.MovieShowTimeID == MovieShowTimeID.Value).SingleOrDefault().MovieID);
            }

            return View(viewMovieQry);
        }

        // GET: /Movie/Details/5
        public ActionResult Details(int? id, int? error)
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
            ViewBag.ShowTimeList = new SelectList(db.MovieShowTimes.Where(mst => mst.MovieID == id && mst.IsDisplay == true).OrderBy(x=>x.ShowTime.CompareTo(DateTime.Now)).ToArray(), "MovieShowTimeID", "ShowTime");

            // check if have any result in null go to ERR 400 
            if (viewMovieQry.Movie == null)
            {
                return HttpNotFound();
            }
            return View(viewMovieQry);
        }



        //POST: /Movie/Details/5
        [HttpPost]
        public ActionResult Details(string ShowTimeList, string HomeCinemaCity, int? MovieID)
        {

            if (string.IsNullOrEmpty(ShowTimeList) || string.IsNullOrEmpty(HomeCinemaCity) || !MovieID.HasValue)
            {
                return RedirectToAction("Index","Home");
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
                        if (theatres.MovieTheatersID == tScreening.TheatresID && tScreening.MovieShowTimeID == showtimeID)
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

            return RedirectToAction("SelectChair", "Chairs", new { id = showtimeID, theatresID = theatresID, timescreenID=timescreenid});
        }

        // GET: /Movie/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Movie/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase file,[Bind(Include = "MovieID,ReleaseDate,MovieName,Rate,Director,MovieDescrption,Picture")] Movie movie )
        {

            //add the Picture
            if (file != null)
            {
                string pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(
                                       Server.MapPath("~/Image/" + pic));
                try
                {
                    // file is uploaded
                    file.SaveAs(path);
                }
                catch (Exception ex)
                {

                    ViewBag.ExeptionMessage = ex.Message;
                }

                Dictionary<string, string> Versions = new Dictionary<string, string>();


                // save the image path path to the database or you can send image
                // directly to database
                // in-case if you want to store byte[] ie. for DB
                using (MemoryStream ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();

                    IList<string> VersionResizings = GenerateVersions(path);

                    ImageConverter imgCon = new ImageConverter();

                    Image imgSmall = Image.FromFile(VersionResizings[0]);
                    imgSmall.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] small = (byte[])imgCon.ConvertTo(imgSmall, typeof(byte[]));

                    Image imgMedume = Image.FromFile(VersionResizings[1]);
                    imgMedume.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] mediume = (byte[])imgCon.ConvertTo(imgMedume, typeof(byte[]));

                    Image imgLarge = Image.FromFile(VersionResizings[2]);
                    imgLarge.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] large = (byte[])imgCon.ConvertTo(imgLarge, typeof(byte[]));

                    if (VersionResizings.Count == 3)
                    {
                        movie.PicturePathSmall = VersionResizings[0];
                        movie.PictureSmall = small;
                        movie.PicturePathMedium = VersionResizings[1];
                        movie.PictureMedium = mediume;
                        movie.PicturePathLarge = VersionResizings[2];
                        movie.PictureLarge = large;


                        db.Movies.Add(movie);
                        db.SaveChanges();

                        // after successfully uploading redirect the user

                        return RedirectToAction("Create", "MovieShowTime", new { id = movie.MovieID });
                        //return RedirectToAction("Edit", "Movie", new { id = movie.MovieID });
                    }
                    else
                    {

                        return RedirectToAction("Index", "Movie");
                    }
                }
              
            }
            if (ModelState.IsValid)
            {
                
                db.Movies.Add(movie);
                db.SaveChanges();
                return RedirectToAction("Create", "MovieShowTime", new { id = movie.MovieID });
            }
            return View(movie);
        }

        // GET: /Movie/Edit/5
        public ActionResult Edit(int? id)
        {

            if (TempData.ContainsKey("MovieID"))
            {
                TempData.Remove("MovieID");
            }
            
            

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            MovieData md =  new MovieData();
            md.MovieShowTime = from mst in db.MovieShowTimes
                               where mst.MovieID == id
                               orderby mst.ShowTime
                               select mst;

            ViewData.Add("showTimeQry",md);

            Movie movie = db.Movies.Find(id);

            if (movie == null)
            {
                return HttpNotFound();
            }

            TempData.Add("MovieID", movie.MovieID);
            return View(movie);
        }

        // POST: /Movie/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string ReleaseDate,[Bind(Include = "MovieID,ReleaseDate,MovieName,Rate,Director,MovieDescrption,Picture")]Movie movie, HttpPostedFileBase file)
        {
            if (ReleaseDate != "" && ReleaseDate != null)
            {
                movie.ReleaseDate = Convert.ToDateTime(ReleaseDate);
            }

            //upload file. 
            if (file != null)
            {
                string pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(
                                       Server.MapPath("~/Image/" + pic));
                try
                {
                    // file is uploaded
                    file.SaveAs(path);
                }
                catch (Exception ex)
                {

                    ViewBag.ExeptionMessage = ex.Message;
                }

                Dictionary<string, string> Versions = new Dictionary<string, string>();

                // save the image path path to the database or you can send image
                // directly to database
                // in-case if you want to store byte[] ie. for DB
                using (MemoryStream ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();

                    IList<string> VersionResizings = GenerateVersions(path);

                    ImageConverter imgCon = new ImageConverter();

                    Image imgSmall = Image.FromFile(VersionResizings[0]);
                    imgSmall.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] small = (byte[])imgCon.ConvertTo(imgSmall, typeof(byte[]));

                    Image imgMedume = Image.FromFile(VersionResizings[1]);
                    imgMedume.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] mediume = (byte[])imgCon.ConvertTo(imgMedume, typeof(byte[]));

                    Image imgLarge = Image.FromFile(VersionResizings[2]);
                    imgLarge.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] large = (byte[])imgCon.ConvertTo(imgLarge, typeof(byte[]));

                    //check for the .3 versions 
                    if (VersionResizings.Count == 3)
                    {
                        movie.PicturePathSmall = VersionResizings[0];
                        movie.PictureSmall = small;
                        movie.PicturePathMedium = VersionResizings[1];
                        movie.PictureMedium = mediume;
                        movie.PicturePathLarge = VersionResizings[2];
                        movie.PictureLarge = large;
                        

                         if (ModelState.IsValid)
                            {
                                db.Entry(movie).State = EntityState.Modified;
                                db.SaveChanges();
                                ViewBag.exeptionMessage = "true";
                                // after successfully uploading redirect the user
                                return RedirectToAction("Index");                            
                            }
                             ViewBag.exeptionMessage = "false";
                             return View(movie);
                    }
                    else
                    {
                        if (ModelState.IsValid)
                        {
                            db.Entry(movie).State = EntityState.Modified;
                            db.SaveChanges();
                            ViewBag.exeptionMessage = "false";
                            return RedirectToAction("Index");
                        }
                        ViewBag.exeptionMessage = "false";
                        return View(movie);
                    }
                }
            }
            return View(movie);
        }

        // GET: /Movie/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: /Movie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Movie movie = db.Movies.Find(id);
            db.Movies.Remove(movie);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //GET: /Movie/FileUpload/1
        [HttpGet]
        public ActionResult FileUpload(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);

        }


        // POST: /Movie/FileUpload/1
        [HttpPost, ActionName("FileUpload")]
        public ActionResult FileUploadPost(HttpPostedFileBase file 
            , [Bind(Include = "MovieID,ReleaseDate,MovieName,Rate,Director,MovieDescrption,Picture")]Movie movie)
        {

            if (file != null)
            {
                string pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(
                                       Server.MapPath("~/Image/" + pic));
                try
                {
                    // file is uploaded
                    file.SaveAs(path);
                }
                catch (Exception ex)
                {

                    ViewBag.ExeptionMessage = ex.Message;
                }

                Dictionary<string, string> Versions = new Dictionary<string, string>();


                // save the image path path to the database or you can send image
                // directly to database
                // in-case if you want to store byte[] ie. for DB
                using (MemoryStream ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();

                    IList<string> VersionResizings = GenerateVersions(path);

                    ImageConverter imgCon = new ImageConverter();

                    Image imgSmall = Image.FromFile(VersionResizings[0]);
                    imgSmall.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] small = (byte[])imgCon.ConvertTo(imgSmall, typeof(byte[]));

                    Image imgMedume = Image.FromFile(VersionResizings[1]);
                    imgMedume.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] mediume = (byte[])imgCon.ConvertTo(imgMedume, typeof(byte[]));

                    Image imgLarge = Image.FromFile(VersionResizings[2]);
                    imgLarge.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] large = (byte[])imgCon.ConvertTo(imgLarge, typeof(byte[]));



                    if (VersionResizings.Count == 3)
                    {
                        movie.PicturePathSmall = VersionResizings[0];
                        movie.PictureSmall = small;
                        movie.PicturePathMedium = VersionResizings[1];
                        movie.PictureMedium = mediume;
                        movie.PicturePathLarge = VersionResizings[2];
                        movie.PictureLarge = large;


                        db.Entry(movie).State = EntityState.Modified;
                        db.SaveChanges();
                        // after successfully uploading redirect the user
                        return RedirectToAction("Edit", "Movie", new { id = movie.MovieID });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Movie");
                    }
                }
            }
            else
            {
                return RedirectToAction("Index", "Movie");
            }

        }


        public JsonResult GetShowTime(int HomeCinemaID,int MovieID)
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
                                    .Where(y=>y.MovieShowTimeID == 
                                        showtime.MovieShowTimeID & y.TheatresID == 
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
                        catch (Exception ex )
                        {
                            string message = ex.Message;
                            // sometime we have a homecinema but he didnt screen this movie . so for this the catch.
                        }
                        
                    }
                }
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }
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
                        if (timescreen.MovieShowTimeID == ShowTimeID && theatres.MovieTheatersID == timescreen.TheatresID)
                        {
                            data = theatres.TheatersName + " " + theatres.NumberHall;
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

        //change the size of the pic XL L S .
        public IList<string> GenerateVersions(string original)
        {

            Dictionary<string, string> versions = new Dictionary<string, string>();

            //Define the versions to generate and their filename suffixes.
            versions.Add("_Thumb", "width=100&height=120&format=png"); //Crop to square thumbnail
            versions.Add("_Medium", "maxwidth=250&maxheight=3500&format=png"); //Fit inside 400x400 area, png
            versions.Add("_Large", "maxwidth=850&maxheight=950&format=png"); //Fit inside 1900x1200 area

            string basePath = ImageResizer.Util.PathUtils.RemoveExtension(original);
            ICollection<string> gFiles = new List<string>();

            //To store the list of generated paths
            var generatedFiles= new List<string>();
            
          

            //Generate each version
            foreach (string PathFix in versions.Keys)
            {
                //Let the image builder add the correct extension based on the output file type
                generatedFiles.Add(ImageBuilder.Current.Build(original, basePath + PathFix, new ResizeSettings(
                    versions[PathFix]), false, true));

            }

            return generatedFiles;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                
            }
            base.Dispose(disposing);
        }
    }
}
