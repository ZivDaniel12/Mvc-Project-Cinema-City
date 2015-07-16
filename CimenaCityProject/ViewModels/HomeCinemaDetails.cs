using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CimenaCityProject.Models;

namespace CimenaCityProject.ViewModels
{
    public class HomeCinemaDetails
    {
        HomeCinemaContext db = new HomeCinemaContext();
        public HomeCinema homeCinema { get; set; }
        public MovieTheaters movieTheaters { get; set; }
        public IEnumerable<MovieTheaters> movieTheatersList { get; set; }
        public IEnumerable<Rows> rows { get; set; }
        public IEnumerable<HallChairs> hallChairs { get; set; }


        public HomeCinemaDetails(string _switch,int? id)
        {
            switch (_switch)
            {
                case "Create":

                    break;
                    
                case "Delete":

                    break;

                case "Details":
                    homeCinema = (from hcid in db.HomeCinemas
                                  where hcid.HomeCinemaID == id
                                  select hcid).SingleOrDefault();
                    movieTheatersList = (from theat in db.Theaters
                                     where theat.HomeCinemaID == theat.HomeCinemaID
                                     select theat);
                    List<Rows> row = new List<Rows>();
                    foreach (var item in movieTheatersList)
                    {
                        
                        row.AddRange(from rw in db.Rows
                                     where rw.TheatersID == item.MovieTheatersID
                                     select rw);
                    }
                    rows = row;
                    List<HallChairs> halChair = new List<HallChairs>();

                    foreach (var item in rows)
                    {
                        halChair.AddRange(from hc in db.HallChairs
                                              where hc.RowID == item.RowsID
                                              select hc);
                    }

                    hallChairs = halChair;

                    break;
                case "Edit":

                    break;
                default:

                    break;
            }
        }
    }
}