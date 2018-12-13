using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MoxyTreasures.Models
{
    public class COrder
    {
        public int intOrderID { get; set; }

        public int intUserID { get; set; }

        public int intTotal { get; set; }

        public int intShippingAddressID { get; set; }

        CAddress ShippingAddress { get; set; }

        public int intStatusID { get; set; }

        public List<CProduct> ProductList = new List<CProduct>();

        public DateTime dtmDateOfOrder = Convert.ToDateTime("01/01/2000");

        public static COrder GetOrder(int intOrderID, int intUserID, int blnCart)
        {
            try
            {
                CDatabase db = new CDatabase();
                COrder Order = db.GetOrder(intOrderID, intUserID, blnCart);
                return Order;

            }
            catch (Exception)
            {
                return null;
            }

        }

        public static int UpdateOrder(COrder Order)
        {
            try
            {
                CDatabase db = new CDatabase();
                db.UpdateOrder(Order);
                return 0;

            }
            catch (Exception)
            {
                return -1;
            }
        }


    }
}