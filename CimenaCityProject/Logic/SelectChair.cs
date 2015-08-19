using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CimenaCityProject.Logic
{
    public static class SelectChair
    {
        /// <summary>
        /// pass here rowID and ChairsID
        /// </summary>
        /// <param name="chairs"></param>
        /// <returns></returns>
        public static Dictionary<int,bool> SetOptimalChair(Dictionary<int,int> chairs)
        {
            Dictionary<int, bool> OptimalChair = new Dictionary<int, bool>();

            try
            {
                List<int> row = new List<int>();
                
                decimal keyNumber = chairs.Keys.Count / 2;

                List<Dictionary<int,int>> lists = new List<Dictionary<int,int>>();

                int selectKey = (int)(Math.Round(keyNumber,MidpointRounding.ToEven));
                int row;

                var chairList = chairs.ToList();
                

                foreach (var item in chairs)
                {
                    if (item.Key == selectKey)
                    {
                        row.Add(item.Key);
                    }
                }

            List<int> chair = new List<int>();

            int ChairCount = chairs.Keys.Count;

            int rowNumbers = row.Count;
            int rowSelected = ChairCount / 2;

            foreach (var item in chairs)
            {
                if (item.Key ==row[rowSelected])
                {

                }
            }


            }
            catch (DivideByZeroException ex)
            {
                
            }

            return OptimalChair;
        }
    }
}