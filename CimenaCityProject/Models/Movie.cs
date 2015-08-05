using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CimenaCityProject.Models
{
    public class Movie
    {
        public Movie()
        {
            this.MovieShowTimes = new HashSet<MovieShowTime>();
        }

        public int MovieID { get; set; }

        [Display(Name = "Movie Name")]
        public string MovieName { get; set; }

        [Display(Name = "Genre")]
        public int GenreID { get; set; }


        [Display(Name = "Release Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyy}")]
        public System.DateTime ReleaseDate { get; set; }


        public int Rate { get; set; }
        public string Director { get; set; }

        [Display(Name = "Movie Descrption")]
        public string MovieDescrption { get; set; }


        public byte[] PictureSmall { get; set; }
        public string PicturePathSmall { get; set; }
        public byte[] PictureMedium { get; set; }
        public string PicturePathMedium { get; set; }
        public byte[] PictureLarge { get; set; }
        public string PicturePathLarge { get; set; }

        public virtual Genre Genre { get; set; }
        public virtual ICollection<MovieShowTime> MovieShowTimes { get; set; }
    }
}