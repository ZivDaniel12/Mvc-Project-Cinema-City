using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.Entity;


namespace CimenaCityProject.Models 
{
    public class HomeCinemaInitializer : DropCreateDatabaseIfModelChanges<HomeCinemaContext>
    {
        protected override void Seed(HomeCinemaContext context)
        {

            var genre = new List<Genre> 
            {
                new Genre{  EnglishName= "Action", HebrewName="אקשן",ArabicName= "."},
                new Genre{  EnglishName= "Drama", HebrewName="דרמה",ArabicName= "."},
                new Genre{  EnglishName= "Comedy", HebrewName="קומדיה",ArabicName= "."},
                new Genre{  EnglishName= "Fantasi", HebrewName="פנטזיה",ArabicName= "."}
            };
            genre.ForEach(h => context.Genre.Add(h));
            context.SaveChanges();

            var movie = new List<Movie>
            {
                new Movie{  Director= "Robert Schwentke", Rate=3,MovieName="Insurgent", ReleaseDate=Convert.ToDateTime("05/05/15") ,
                    MovieDescrption= "Beatrice Prior must confront her inner demons and continue her fight against a powerful alliance which threatens to tear her society apart with the help from others on her side."
               ,  PicturePathSmall= "~/Image/Thumbs/Insurgent.png", PicturePathLarge ="~/Image/Insurgent.png",GenreID=genre[0].GenreID 
                },

                new Movie{  Director=" Glenn Ficarra", Rate=4, MovieName="Focus", ReleaseDate=Convert.ToDateTime("10/01/15"),
                     MovieDescrption="In the midst of veteran con man Nicky's latest scheme, a woman from his past - now an accomplished femme fatale - shows up and throws his plans for a loop."
                     , PicturePathSmall= "~/Image/Thumbs/Focus.png", PicturePathLarge ="~/Image/Focus.png",GenreID=genre[2].GenreID
                },

                new Movie{ Director=" Chris Kyle", Rate=4, MovieName="American Sniper", ReleaseDate= Convert.ToDateTime("03/02/15"),
                     MovieDescrption="Navy SEAL sniper Chris Kyle's pinpoint accuracy saves countless lives on the battlefield and turns him into a legend. Back home to his wife and kids after four tours of duty, however, Chris finds that it is the war he can't leave behind."
                     , PicturePathSmall= "~/Image/Thumbs/American Sniper.png", PicturePathLarge ="~/Image/American Sniper.png", GenreID=genre[0].GenreID
                }
            };
            movie.ForEach(m => context.Movies.Add(m));
            context.SaveChanges();

            var movieShowTime = new List<MovieShowTime>
            {
                new MovieShowTime{ ShowTime=DateTime.Parse("11:00:00"), MovieID=movie.Single(m=> m.MovieID == 1 ).MovieID},
                new MovieShowTime{ ShowTime=DateTime.Parse("13:00:00"), MovieID=movie.Single(m=> m.MovieID == 1 ).MovieID},
                new MovieShowTime{ ShowTime=DateTime.Parse("16:00:00"), MovieID=movie.Single(m=> m.MovieID == 1 ).MovieID},
                new MovieShowTime{ ShowTime=DateTime.Parse("20:00:00"), MovieID=movie.Single(m=> m.MovieID == 2 ).MovieID},
                new MovieShowTime{ ShowTime=DateTime.Parse("11:00:00"), MovieID=movie.Single(m=> m.MovieID == 2 ).MovieID},
                new MovieShowTime{ ShowTime=DateTime.Parse("13:00:00"), MovieID=movie.Single(m=> m.MovieID == 2 ).MovieID},
                new MovieShowTime{ ShowTime=DateTime.Parse("16:00:00"), MovieID=movie.Single(m=> m.MovieID == 3 ).MovieID},
                new MovieShowTime{ ShowTime=DateTime.Parse("20:00:00"), MovieID=movie.Single(m=> m.MovieID == 3 ).MovieID},
                new MovieShowTime{ ShowTime=DateTime.Parse("11:00:00"), MovieID=movie.Single(m=> m.MovieID == 3 ).MovieID}
            };
            movieShowTime.ForEach(m => context.MovieShowTimes.Add(m));
            context.SaveChanges();

            var city = new List<CityList> 
            {
                new CityList{  EnglishName= "Ashkelon", HebrewName="אשקלון",ArabicName= ".", District="South"},
                new CityList{  EnglishName= "Tel Aviv", HebrewName="תל אביב",ArabicName= ".", District="Center"},
                new CityList{  EnglishName= "Jerusalem", HebrewName="ירושלים",ArabicName= ".", District="South"}
            };
            city.ForEach(h => context.CityList.Add(h));
            context.SaveChanges();


           
            //var homeCinema = new List<HomeCinema> 
            //{
            //    new HomeCinema{  CityID= city.Single(c=>c.CityID == 1).CityID, CinemaName="GlobsMax",Address= "Ben Zion 9", PhoneNumber="08-8888888"},
            //    new HomeCinema{  CityID= city.Single(c=>c.CityID == 2).CityID, CinemaName="GoodTimes",Address= "Rotshild 14", PhoneNumber="03-3333333"},
            //    new HomeCinema{  CityID= city.Single(c=>c.CityID == 3).CityID, CinemaName="RavHen",Address= "Har Gilo 1", PhoneNumber="02-2222222"}
            //};
            //homeCinema.ForEach(h => context.HomeCinemas.Add(h));
            //context.SaveChanges();

            //var movieTheatrs = new List<MovieTheaters> 
            //{
            //        new MovieTheaters{ HomeCinemaID=homeCinema.Single(h=>h.HomeCinemaID == 1).HomeCinemaID 
            //        , RowAmount=4,TheatersName="NumberOne",NumberHall=1 },
            //        new MovieTheaters{ HomeCinemaID=homeCinema.Single(h=>h.HomeCinemaID == 1).HomeCinemaID 
            //        , RowAmount=4,TheatersName="NumberTw4",NumberHall=2 },
            //        new MovieTheaters{ HomeCinemaID=homeCinema.Single(h=>h.HomeCinemaID == 2).HomeCinemaID 
            //        , RowAmount=4,TheatersName="NumberOne",NumberHall=3 },
            //        new MovieTheaters{ HomeCinemaID=homeCinema.Single(h=>h.HomeCinemaID == 2).HomeCinemaID 
            //        , RowAmount=4,TheatersName="NumberTw3",NumberHall=4 },
            //        new MovieTheaters{ HomeCinemaID=homeCinema.Single(h=>h.HomeCinemaID == 3).HomeCinemaID 
            //        , RowAmount=4,TheatersName="NumberOne",NumberHall=1 },
            //        new MovieTheaters{ HomeCinemaID=homeCinema.Single(h=>h.HomeCinemaID == 3).HomeCinemaID 
            //        , RowAmount=4,TheatersName="NumberTw2",NumberHall=3 }
            //};
            //movieTheatrs.ForEach(m => context.Theaters.Add(m));
            //context.SaveChanges();


            //var timeScreen = new List<TimeScreening> 
            //{
            //    new TimeScreening { MovieShowTimeID= movieShowTime.Single(mst=>mst.MovieShowTimeID == 1).MovieShowTimeID,
            //    TheatresID = movieTheatrs.Single(mt=>mt.MovieTheatersID==1).MovieTheatersID,
            //    Date = Convert.ToDateTime("01/01/2015"), Price = 34, IsDisplayed = true},
            //     new TimeScreening { MovieShowTimeID= movieShowTime.Single(mst=>mst.MovieShowTimeID == 2).MovieShowTimeID,
            //    TheatresID = movieTheatrs.Single(mt=>mt.MovieTheatersID==1).MovieTheatersID,
            //    Date = Convert.ToDateTime("02/02/2015"), Price = 34, IsDisplayed = true},
            //     new TimeScreening { MovieShowTimeID= movieShowTime.Single(mst=>mst.MovieShowTimeID ==3).MovieShowTimeID,
            //    TheatresID = movieTheatrs.Single(mt=>mt.MovieTheatersID==2).MovieTheatersID,
            //    Date = Convert.ToDateTime("03/03/2015"), Price = 34, IsDisplayed = true},
            //     new TimeScreening { MovieShowTimeID= movieShowTime.Single(mst=>mst.MovieShowTimeID == 4).MovieShowTimeID,
            //    TheatresID = movieTheatrs.Single(mt=>mt.MovieTheatersID==2).MovieTheatersID,
            //    Date = Convert.ToDateTime("04/04/2015"), Price = 34, IsDisplayed = true},
            //     new TimeScreening { MovieShowTimeID= movieShowTime.Single(mst=>mst.MovieShowTimeID == 5).MovieShowTimeID,
            //    TheatresID = movieTheatrs.Single(mt=>mt.MovieTheatersID==3).MovieTheatersID,
            //    Date = Convert.ToDateTime("05/05/2015"), Price = 34, IsDisplayed = true},
            //     new TimeScreening { MovieShowTimeID= movieShowTime.Single(mst=>mst.MovieShowTimeID == 6).MovieShowTimeID,
            //    TheatresID = movieTheatrs.Single(mt=>mt.MovieTheatersID==3).MovieTheatersID,
            //    Date = Convert.ToDateTime("02/02/2015"), Price = 34, IsDisplayed = true},
            //     new TimeScreening { MovieShowTimeID= movieShowTime.Single(mst=>mst.MovieShowTimeID == 7).MovieShowTimeID,
            //    TheatresID = movieTheatrs.Single(mt=>mt.MovieTheatersID==1).MovieTheatersID,
            //    Date = Convert.ToDateTime("03/03/2015"), Price = 34, IsDisplayed = true},
            //     new TimeScreening { MovieShowTimeID= movieShowTime.Single(mst=>mst.MovieShowTimeID == 8).MovieShowTimeID,
            //    TheatresID = movieTheatrs.Single(mt=>mt.MovieTheatersID==2).MovieTheatersID,
            //    Date = Convert.ToDateTime("04/04/2015"), Price = 34, IsDisplayed = true},
            //     new TimeScreening { MovieShowTimeID= movieShowTime.Single(mst=>mst.MovieShowTimeID == 9).MovieShowTimeID,
            //    TheatresID = movieTheatrs.Single(mt=>mt.MovieTheatersID==3).MovieTheatersID,
            //    Date = Convert.ToDateTime("05/05/2015"), Price = 34, IsDisplayed = true}

            //};

            //timeScreen.ForEach(ts => context.TimeScreening.Add(ts));
            //context.SaveChanges();


            //var rows = new List<Rows>
            //{
            //    new Rows{ TheatersID = movieTheatrs.Single(mt=>mt.MovieTheatersID==1).MovieTheatersID, RowNumber=1 , ChairCapacity = 5},
            //    new Rows{ TheatersID = movieTheatrs.Single(mt=>mt.MovieTheatersID==1).MovieTheatersID, RowNumber=2 , ChairCapacity = 5},
            //    new Rows{ TheatersID = movieTheatrs.Single(mt=>mt.MovieTheatersID==1).MovieTheatersID, RowNumber=3 , ChairCapacity = 5},
            //    new Rows{ TheatersID = movieTheatrs.Single(mt=>mt.MovieTheatersID==1).MovieTheatersID, RowNumber=4 , ChairCapacity = 5},
            //    new Rows{ TheatersID = movieTheatrs.Single(mt=>mt.MovieTheatersID==2).MovieTheatersID, RowNumber=1 , ChairCapacity = 5},
            //    new Rows{ TheatersID = movieTheatrs.Single(mt=>mt.MovieTheatersID==2).MovieTheatersID, RowNumber=2 , ChairCapacity = 5},
            //    new Rows{ TheatersID = movieTheatrs.Single(mt=>mt.MovieTheatersID==2).MovieTheatersID, RowNumber=3 , ChairCapacity = 5},
            //    new Rows{ TheatersID = movieTheatrs.Single(mt=>mt.MovieTheatersID==2).MovieTheatersID, RowNumber=4 , ChairCapacity = 5}
            //};
            //rows.ForEach(r => context.Rows.Add(r));
            //context.SaveChanges();

            //var hallChairs = new List<HallChairs> 
            //{
            //    new HallChairs{ RowID=rows.Single(r=>r.RowsID==1).RowsID, ChairNumber= 1},
            //    new HallChairs{ RowID=rows.Single(r=>r.RowsID==1).RowsID, ChairNumber= 2},
            //    new HallChairs{ RowID=rows.Single(r=>r.RowsID==2).RowsID, ChairNumber= 1},
            //    new HallChairs{ RowID=rows.Single(r=>r.RowsID==2).RowsID, ChairNumber= 3} 
            //};
            //hallChairs.ForEach(h => context.HallChairs.Add(h));
            //context.SaveChanges();

            //var chairsOrderd = new List<ChairsOrderd>
            //{
            //    new ChairsOrderd { HallChairID = hallChairs.Single(hc=>hc.HallChairsID==1).HallChairsID },
            //    new ChairsOrderd { HallChairID = hallChairs.Single(hc=>hc.HallChairsID==2).HallChairsID },
            //    new ChairsOrderd { HallChairID = hallChairs.Single(hc=>hc.HallChairsID==3).HallChairsID },
            //    new ChairsOrderd { HallChairID = hallChairs.Single(hc=>hc.HallChairsID==4).HallChairsID },
            //};
            //chairsOrderd.ForEach(co => context.ChairsOrderd.Add(co));
            //context.SaveChanges();

            //var Events = new List<Event>
            //{
            //    new Event { MovieShowTimeID = movieShowTime.Single(mst=>mst.MovieShowTimeID==1).MovieShowTimeID}
            //};
            //Events.ForEach(evnt => context.Events.Add(evnt));
            //context.SaveChanges();


               var person = new List<Person>
            {
                    new Person{ PersonalID=204036131 ,FirstName = "Afi", LastName="Koman", Address= "Ashkelon Tiran 10", BirthDate=DateTime.Parse("01/01/2000")},
                    new Person{ PersonalID=241032475, FirstName = "Avi", LastName="Ron",Address= "TelAviv Shemer 11", BirthDate=DateTime.Parse("02/01/1994")},
                    new Person{ PersonalID=284214275, FirstName = "Na", LastName="Hum",Address= "Herzelia Tovim 4", BirthDate=DateTime.Parse("03/05/1999")},
                    new Person{ PersonalID=246540157, FirstName = "Sha", LastName="Lom",Address= "Beer Sheva Tomer 9", BirthDate=DateTime.Parse("04/01/1990")},
                    new Person{ PersonalID=304125471, FirstName = "Avi", LastName="Ron",Address= "TelAviv Shemer 11", BirthDate=DateTime.Parse("12/11/2001")}
            };
            person.ForEach(p => context.Persons.Add(p));
            context.SaveChanges();

            //var order = new List<Order> 
            //{

            //        new Order{  OrderDate=Convert.ToDateTime("01/02/2015"), EventID =  Events.Single(evt=>evt.EventID==1).EventID,
            //        TimeScreeningID=timeScreen.Single(ts=>ts.TimeScreeningID == 2).TimeScreeningID }
            //};
            //order.ForEach(o => context.Orders.Add(o));
            //context.SaveChanges();
        }
    }
}