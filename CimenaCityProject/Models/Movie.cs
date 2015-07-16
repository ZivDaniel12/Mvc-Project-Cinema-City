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

        [Display(Name = "Release Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yy}")]
        public DateTime ReleaseDate { get; set; }
        

        //     [Required]
        [Display(Name = "Movie Name")]
        //       [StringLength(30, ErrorMessage = "You can enter up to 30 characters ")]
        public string MovieName { get; set; }


        // do i need to put here "[range(0,5)]" attribute? 
        //      [MaxLength(5, ErrorMessage = "You can enter just 5 point")]
        //[DisplayFormat(NullDisplayText = "No rate")]
        public int Rate { get; set; }

        //      [Required]
        //      [StringLength(15, ErrorMessage = "You can enter up to 15 characters ")]
        //      [Display(Name = "Movie Director")]
        //    [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$")]
        public string Director { get; set; }

        //       [Required]
        [Display(Name = "Movie Descrption")]
        //       [StringLength(250, ErrorMessage = "You can enter up to 250 characters ")]
        public string MovieDescrption { get; set; }


        public byte[] PictureSmall { get; set; }
        public string PicturePathSmall { get; set; }
        public byte[] PictureMedium { get; set; }
        public string PicturePathMedium { get; set; }
        public byte[] PictureLarge { get; set; }
        public string PicturePathLarge { get; set; }

        public virtual  ICollection<MovieShowTime> MovieShowTimes { get; set; }
    }
}