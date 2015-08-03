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
        //PK.
        [Key]
        public int MovieID { get; set; }

        //     [Required]
        [Display(Name = "Movie Name")]
        //       [StringLength(30, ErrorMessage = "You can enter up to 30 characters ")]
        public string MovieName { get; set; }


        [Display(Name = "Release Date")]
        [DisplayFormat(ApplyFormatInEditMode=true,DataFormatString="{0:dd/MM/yyy}")]
        public DateTime ReleaseDate { get; set; }

        [Display(Name = "Genre")]
        public int GenreID { get; set; }


        public int Rate { get; set; }


    //    [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$")]
        public string Director { get; set; }

        //       [Required]
        [Display(Name = "Movie Descrption")]
        public string MovieDescrption { get; set; }


        public byte[] PictureSmall { get; set; }
        public string PicturePathSmall { get; set; }
        public byte[] PictureMedium { get; set; }
        public string PicturePathMedium { get; set; }
        public byte[] PictureLarge { get; set; }
        public string PicturePathLarge { get; set; }

        public virtual Genre Genre  { get; set; }

        public virtual  ICollection<MovieShowTime> MovieShowTimes { get; set; }
    }
}