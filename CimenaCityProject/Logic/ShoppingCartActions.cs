using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using CimenaCityProject.ViewModels;
using CimenaCityProject.Models;

namespace CimenaCityProject.Logic
{
    public class ShoppingCartActions : IDisposable
    {

        public string ShoppingCartId { get; set; }

        public const string CartSessionKey = "CartId";

        private HomeCinemaContext db = new HomeCinemaContext();

        private OrderDetails orderDetails = new OrderDetails();


        #region GetCartId()
        // get cart id 
        /// <summary>
        /// GetCartId()
        /// </summary>
        /// <returns></returns>
        public string GetCartId()
        {
            // check if the session is empty 
            if (HttpContext.Current.Session[CartSessionKey] == null)
            {
                // if the user is registered get the id
                if (!string.IsNullOrWhiteSpace
                    (HttpContext.Current.User.Identity.Name))
                {
                    // implement session to user
                    HttpContext.Current.Session[CartSessionKey] =
                    HttpContext.Current.User.Identity.Name;
                }
                // else for anonymous user - 
                else
                {
                    Guid tempCartId = Guid.NewGuid();
                    HttpContext.Current.Session[CartSessionKey] =
                         tempCartId.ToString();
                }
            }
            return HttpContext.Current.Session[CartSessionKey].ToString();
        }
        #endregion

        #region AddToCart(int id)
        // Add to cart 
        /// <summary>
        /// AddToCart(int id)
        /// </summary>
        /// <param name="id"></param>
        //public void AddToCart(int ChairsOrderdiD,int? PersonID)
        //{
        //    // get the info from DB
        //    ShoppingCartId = GetCartId();
        //    //linq
        //    var orderItem = db.Orders.
        //        SingleOrDefault(c => c.OrderID == ShoppingCartId &&
        //        c.PersonID == PersonID.Value);

        //    if (orderItem == null)
        //    {
        //        // create new cart item 
        //        orderItem = new Order
        //        {
        //            OrderID = Guid.NewGuid().ToString(),
        //            PersonID = PersonID.Value,
        //            CartId = ShoppingCartId,
        //            ChairsOrderdID = db.ChairsOrderd.SingleOrDefault
        //            (c => c.ChairsOrderdiD == ChairsOrderdiD).ChairsOrderdiD,
        //            OrderDate = DateTime.Now

        //        };
        //        db.Orders.Add(orderItem);
        //    }
        //    else
        //    {
                
        //    }
        //    db.SaveChanges();
        //}
        #endregion

        #region GetOrderItems()
        // return List<t> of CartItems
        /// <summary>
        /// GetCartItems()
        /// </summary>
        /// <returns></returns>
        public OrderDetails GetOrderItems(int movieID, int timeScreeningID, int orderID, int hallChairID)
        {

            var orderInfoDetails = new CimenaCityProject.ViewModels.OrderDetails();

            orderInfoDetails.Movie = from m in db.Movies
                                     where m.MovieID == movieID
                                     select m ;

            orderInfoDetails.TimeScreening = from t in db.TimeScreening
                                         where t.TimeScreeningID == timeScreeningID
                                         select t;

            foreach (var item in db.HallChairs)
            {
                if (item.HallChairsID == hallChairID)
                {
                    orderDetails.ChairsOrderd++;
                }
            }

            orderInfoDetails.Order = db.Orders.Where(o => Convert.ToInt16(o.OrderID) == orderID);

            return orderInfoDetails;
        }
        #endregion

        #region GetTotal()
        // get total cart price
        /// <summary>
        /// GetTotal()
        /// </summary>
        /// <returns></returns>
        public decimal GetTotal(int timeScreeningID,int hallChairID)
        {
            ShoppingCartId = GetCartId();
            decimal? total = decimal.Zero;

            var orderInfoDetails = new CimenaCityProject.ViewModels.OrderDetails();

            orderInfoDetails.TimeScreening = from t in db.TimeScreening
                                             where t.TimeScreeningID == timeScreeningID
                                             select t;

            foreach (var item in db.HallChairs)
            {
                if (item.HallChairsID == hallChairID)
                {
                    orderDetails.ChairsOrderd++;
                }
            }


            total = (decimal?)
                    ( orderDetails.ChairsOrderd * orderDetails.TimeScreening.Where(t => t.TimeScreeningID == timeScreeningID).SingleOrDefault().Price);

            return total ?? decimal.Zero;
        }
        #endregion

        #region struct ShoppingCartUpdates
        /// <summary>
        /// shopping cart updates 
        /// </summary>
        public struct ShoppingCartUpdates
        {
            public int MovieID;
            public int PurchaseQuantity;
            public bool RemoveItem;
        }
        #endregion

        #region GetCount()
        /// <summary>
        /// GetCount()
        /// </summary>
        /// <returns></returns>
        //public int GetCount()
        //{
        //    ShoppingCartId = GetCartId();
        //    int? count = (from cartItem in db.Orders
        //                  where cartItem.OrderID == ShoppingCartId
        //                  select (int?)cartItem.).Sum();

        //    return count ?? 0;
        //}
        #endregion

        #region Update Logic

        public void UpdateShoppingCart
            (int movieID, int timeScreeningID, int orderID, int hallChairID, string cartId, ShoppingCartUpdates[] cartItemsUpdate)
        {
            try
            {
                int countCartItem = cartItemsUpdate.Count();
                CimenaCityProject.ViewModels.OrderDetails myCart = GetOrderItems(movieID, timeScreeningID, orderID, hallChairID);

                foreach (var cartItem in myCart.Movie)
                {
                    // iteration over GridView
                    for (int i = 0; i < countCartItem; i++)
                    {
                        if (cartItem.MovieID == cartItemsUpdate[i].MovieID)
                        {
                            if (cartItemsUpdate[i].PurchaseQuantity < 1 ||
                                cartItemsUpdate[i].RemoveItem == true)
                            {
                                RemoveItem(cartId, cartItem.MovieID);
                            }
                            else
                            {
                                UpdateItem(cartId, cartItem.MovieID, cartItemsUpdate[i].PurchaseQuantity);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Error: unable to update cart " +
                    ex.Message);
            }
        }


        #endregion

        #region Remove and Update Item

        public void RemoveItem
            (string removeCartId, int removeProdId)
        {
            try
            {
                using (var db = new HomeCinemaContext())
                {

                    // i need here to select the item and the movieId then i can remove cart 
                    var myItem = (from c in db.Orders
                                  where c.CartId == removeCartId 
                                  select c).FirstOrDefault();
                    if (myItem != null)
                    {
                        // remove from DB
                        db.Orders.Remove(myItem);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error: unable to Remove item " +
                                  ex.Message);
            }
        }


        public void UpdateItem(string updateCartId,
            int updateProductId, int quantity)
        {
            try
            {
                // i need here to update the cart via the quantity
                
            }
            catch (Exception ex)
            {
                throw new Exception("Error: unable to update item " +
                                  ex.Message);
            }
        }


        #endregion

        #region Empty cart

        public void EmptyCart()
        {
            ShoppingCartId = GetCartId();

            // after i getting the ID i need to find the name cart , then remove it . 
        }


        #endregion

        #region Migration from anonymous to Registered User

        public void MigrationCart(string cartId, string userName)
        {
         
            // if user come by anonymous then login i need to add the movies he see for the card with his ID via Guid .

        }

        #endregion

        #region Dispose()
        // class using Dispose for db after done the job . 
        public void Dispose()
        {
            if (db != null)
            {
                db.Dispose();
                db = null;
            }
        }
        #endregion
    }
}