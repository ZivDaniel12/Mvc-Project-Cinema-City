using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using CimenaCityProject.Models;


namespace CimenaCityProject.Logic
{
    public class EcomLogic
    {
        private HomeCinemaContext db = new HomeCinemaContext();

        public bool CheckIfTheContractExist(int TheatresID,int MovieShowTimeID)
        {

            //false == contact Not Exist. 
            bool Checked = false;

            var Contracts = db.TimeScreening.Where(contract => contract.MovieTheatersID == TheatresID).ToList();
            var time = db.MovieShowTimes.Find(MovieShowTimeID).ShowTime;

            foreach (var contract in Contracts)
            {

                Checked = (contract.MovieShowTime.ShowTime == time && contract.MovieTheatersID == TheatresID) ? true : false;
                 if (Checked == true)
                     //true == contact Exist and he need to pick another time OR theatres. 
                     break;                 
            }

            return Checked;
        }



        public EcomLogic()
        {

        }
    }
}