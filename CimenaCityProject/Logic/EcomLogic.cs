using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using CimenaCityProject.Models;
using CimenaCityProject.ViewModels;

namespace CimenaCityProject.Logic
{
    public static class EcomLogic
    {

         public static bool CheckIfTheContractExist(int TheatresID,int MovieShowTimeID)
        {
            HomeCinemaContext db = new HomeCinemaContext();
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

        public static List<string> GetChairNumbers(int[] chirsOrderdID,ICollection<ChairsOrderd> ChairsOrder )
        {
            HomeCinemaContext db = new HomeCinemaContext();

            List<string> ChairsNumber = new List<string>();

                for (int i = 0; i < chirsOrderdID.Length; i++)
                {
                    var row =  db.Rows.Find(ChairsOrder.Where(x => chirsOrderdID[i] == x.ChairsOrderdiD).First().HallChairs.RowID);
                    ChairsNumber.Add(string.Format(row.RowNumber.ToString() 
                        +":"+ChairsOrder.Where(x => chirsOrderdID[i] == x.ChairsOrderdiD).First().HallChairs.ChairNumber.ToString()));
                }
            return ChairsNumber;
        }

        public static Order AddNewOrder(TheatersChairs OrderInfo, int EventID, DateTime OrderDate, int TotalChairOrdered)
        {
            Order o = new Order();
            o.CartId = OrderInfo.cartID;
            o.EventID = EventID;
            o.OrderDate = OrderDate;
            o.TimeScreeningID = OrderInfo.TimeScreening.TimeScreeningID;
            o.TotalChairsOrdered = TotalChairOrdered;
            return o;
 
        }

    }
}