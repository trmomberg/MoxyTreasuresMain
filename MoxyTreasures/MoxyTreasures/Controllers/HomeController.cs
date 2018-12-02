using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoxyTreasures.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Home()
        {

            int intCategoryID = Convert.ToInt32(RouteData.Values["id"]);
            Models.CHome h = new Models.CHome();
            Models.CDatabase db = new Models.CDatabase();
            if (h.CurrentUser.GetCurrentUser() != null)
            {
                h.CurrentUser = h.CurrentUser.GetCurrentUser();
            }
            else
            {
                h.CurrentUser.UserID = 0;
            }
                       
            switch (intCategoryID)
            {
                case 0:
                    // All Products
                    h.Products = Models.CProduct.GetProducts();
                    h.HeaderText = "All";
                    break;
                case 1:
                    h.Products = db.GetProducts(0, 1);
                    h.HeaderText = "Rings";
                    break;
                case 2:
                    h.Products = db.GetProducts(0, 2);
                    h.HeaderText = "Necklaces";
                    break;
                case 3:
                    h.Products = db.GetProducts(0, 3);
                    h.HeaderText = "Earrings";
                    break;
                case 4:
                    h.Products = db.GetProducts(0, 4);
                    h.HeaderText = "Bracelets";
                    break;
                case 5:
                    h.Products = db.GetProducts(0, 5);
                    h.HeaderText = "Clothing";
                    break;
                default:
                    // All Products
                    h.Products = Models.CProduct.GetProducts();
                    break;
            }

            if (h.Products != null)
            {
                foreach (Models.CProduct p in h.Products)
                {
                    if (db.IsWatched(p.ProductID, h.CurrentUser.UserID))
                    {
                        p.ActionStatus = Models.CProduct.ActionStatusTypes.ToggleWatchOn;
                    }
                }
            }
            return View(h);
		}

        // GET: About
        public ActionResult About()
        {
            Models.CHome h = new Models.CHome();
            Models.CDatabase db = new Models.CDatabase();
            if (h.CurrentUser.GetCurrentUser() != null)
            {
                h.CurrentUser = h.CurrentUser.GetCurrentUser();
            }
            else
            {
                h.CurrentUser.UserID = 0;
            }

            return View(h);
        }

        // GET: Main
        public ActionResult Main()
        {
            Models.CHome h = new Models.CHome();
            Models.CDatabase db = new Models.CDatabase();
            if (h.CurrentUser.GetCurrentUser() != null)
            {
                h.CurrentUser = h.CurrentUser.GetCurrentUser();
            }
            else
            {
                h.CurrentUser.UserID = 0;
            }

            return View(h);
        }

        //[HttpPost]
        //public ActionResult Home()
        //{
        //	Models.CHome h = new Models.CHome();
        //	if (h.CurrentUser.GetCurrentUser() != null)
        //	{
        //		h.CurrentUser = h.CurrentUser.GetCurrentUser();
        //	}
        //	else
        //	{
        //		h.CurrentUser.UserID = 0;
        //	}
        //	h.Products = Models.CProduct.GetProducts(0);
        //	return View(h);
        //}

        public ActionResult ProductDetails()
		{
            Models.CHome Home = new Models.CHome();
            if (Home.CurrentUser.GetCurrentUser() != null)
            {
                Home.CurrentUser = Home.CurrentUser.GetCurrentUser();
            }

            if (RouteData.Values["id"] != null)
            {
                Home.Products.Add(Models.CProduct.GetProduct(Convert.ToInt32(RouteData.Values["id"])));
            }

            return View(Home);
		}

        public ActionResult ToggleCart()
        {
            Models.CHome Home = new Models.CHome();
            return View(Home);
        }

        [HttpPost]
        public JsonResult Home(int intProductID, int intUserID)
        {
            try
            {
                              
                Models.CHome Home = new Models.CHome();
                Models.CDatabase db = new Models.CDatabase();

                Home.Products = Models.CProduct.GetProducts();

                foreach (Models.CProduct p in Home.Products)
                {
                    if (db.IsWatched(p.ProductID, intUserID))
                    {
                        p.ActionStatus = Models.CProduct.ActionStatusTypes.ToggleWatchOn;
                    }
                }

                int intResult = 0;

                intResult = db.ToggleCart(intUserID, intProductID);

                if (intResult == 1)
                {
                    Home.Products[intProductID - 1].ActionStatus = Models.CProduct.ActionStatusTypes.ToggleWatchOn;
                }
                else if (intResult == -1)
                {
                    Home.Products[intProductID - 1].ActionStatus = Models.CProduct.ActionStatusTypes.ToggleWatchOff;
                }

                return Json(Home.Products[intProductID - 1]);
            }
            catch (Exception ex)
            {
                string a = ex.Message;
                return null;
            }
        }
    }
}