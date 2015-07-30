using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace CimenaCityProject.Models
{
    public class HomeCinemaContext : DbContext
    {
        public HomeCinemaContext()
            : base("HomeCinemaContext")
        { }

        public DbSet<CityList> CityList { get; set; }
        public DbSet<Genre> Genre { get; set; }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieShowTime> MovieShowTimes { get; set; }

        public DbSet<HomeCinema> HomeCinemas { get; set; }
        public DbSet<MovieTheaters> Theaters { get; set; }

        public DbSet<TimeScreening> TimeScreening { get; set; }

        public DbSet<Rows> Rows { get; set; }
        public DbSet<HallChairs> HallChairs { get; set; }
        public DbSet<ChairsOrderd> ChairsOrderd { get; set; }

        public DbSet<Event> Events { get; set; }
        public DbSet<Person> Persons { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<CheckOut> CheckOut { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove
                <PluralizingTableNameConvention>();
        }


        internal EntityState Entry<T1, T2, T3>(TimeScreening timeScreening, MovieShowTime movieShowTime, MovieTheaters movieTheaters)
        {
            return EntityState.Modified;
        }
    }
}