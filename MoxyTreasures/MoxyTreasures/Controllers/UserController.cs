using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace MoxyTreasures.Controllers
{
    public class UserController : Controller
    {
        
        public ActionResult SignUp()
        {
			Models.CUser User = new Models.CUser();
            return View(User);
        }

		public ActionResult SignOut()
		{
			Models.CUser User = new Models.CUser();
			User.ClearCurrentUser();
			return View(User);		
		}

        public ActionResult Login()
        {
			Models.CUser User = new Models.CUser();
			User.EmailAddress = "Todd0929971@gmail.com";
			User.Password = "12345";
			return View(User);
        }
       
        public ActionResult Email()
        {
            Models.CUser User = new Models.CUser();
            User = User.GetCurrentUser();
            return View(User);
        }

        public ActionResult Data()
        {
            Models.CUser User = new Models.CUser();
            User = User.GetCurrentUser();
            return View(User);
        }

        public ActionResult Cart()
		{
			Models.CUser User = new Models.CUser();
            Models.CProduct product = new Models.CProduct();
			Models.CDatabase db = new Models.CDatabase();
			User = User.GetCurrentUser();
            double dblSubTotal = 0;

            if (User != null && User.UserID > 0)
            {
                User.Cart = db.GetCart(User.UserID);

                for (int i = 0; i < User.Cart.Count(); i++)
                {                   
                    dblSubTotal += User.Cart[i].Price;
                }

                User.CartSubTotal = dblSubTotal;
            }
			return View(User);
		}

        public ActionResult Categories()
        {
            Models.CUser User = new Models.CUser();
            User = User.GetCurrentUser();

            Models.CCategories categories = new Models.CCategories();
            categories.CategoryList = Models.CCategories.GetCategories();
            categories.UserID = User.UserID;

            return View(categories);
        }

        public ActionResult AjaxTest()
		{
			Models.CUser User = new Models.CUser();
			return View(User);
		}

		public ActionResult AjaxLogin()
		{
			Models.CUser User = new Models.CUser();
			return View(User);
		}

		[HttpPost]
		public JsonResult AjaxTest(string property1, string property2)
		{
			try
			{
				Models.CUser User = new Models.CUser();
				User.UserID = 99;
				User.FirstName = "Jimmy";
				User.LastName = "Dean";
				return Json(User);
			}
			catch (Exception)
			{
				return null;
			}
		}

        [HttpPost]
        public JsonResult Categories(string strtranstype, string strCategory = "", int intCategoryID = 0)
        {
            Models.CCategories category = new Models.CCategories();
            Models.CDatabase db = new Models.CDatabase();

            if (strtranstype == "add")
            {
                category = category.AddCategory(strCategory);
                if (category.ActionStatus == Models.CCategories.ActionStatusTypes.CategoryAdded)
                {
                    return Json(new
                    {
                        CategoryID = category.intCategoryID,
                        strCategory = strCategory,
                        ActionStatus = Models.CCategories.ActionStatusTypes.CategoryAdded
                    });
                }
                else
                {
                    return Json(new
                    {
                        ActionStatus = Models.CCategories.ActionStatusTypes.CategoryAddFailed
                    });
                }
            }
            else if (strtranstype == "delete")
            {
                int intActionStatusID;
                category = Models.CCategories.GetCategory(intCategoryID);
                intActionStatusID = category.DeleteCategory(intCategoryID);
                if ((Models.CCategories.ActionStatusTypes)intActionStatusID == Models.CCategories.ActionStatusTypes.CategoryDeleted)
                {
                    return Json(new
                    {
                        intCategoryID = category.intCategoryID,
                        strCategory = category.strCategory,
                        ActionStatus = Models.CCategories.ActionStatusTypes.CategoryDeleted
                    });
                }
                else
                {
                    return Json(new
                    {
                        ActionStatus = Models.CCategories.ActionStatusTypes.CategoryDeleteFailed
                    });
                }
            }
            else
            {
                return Json(new
                {
                    ActionStatus = Models.CCategories.ActionStatusTypes.SelectACategory
                });
            }
        }

        [HttpPost]
		public JsonResult AjaxLogin(string EmailAddress, string Password)
		{
			try
			{
				Models.CUser User = new Models.CUser();

				User = User.Login(EmailAddress, Password);

				if (User.ActionStatus == Models.CUser.ActionStatusTypes.UserLoggedIn)
				{
					User.SetCurrentUser();
					return Json(new { FirstName = User.FirstName
									, LastName = User.LastName
									, UserID = User.UserID
									, ActionStatus = User.ActionStatus});
				}
				else
				{
					return Json(new { FirstName = ""
									, LastName = ""
									, UserID = 0
									, ActionStatus = Models.CUser.ActionStatusTypes.FailedLogin});
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public ActionResult MyProfile()
		{
			Models.CUser User = new Models.CUser();
			User = User.GetCurrentUser();
			return View(User);
		}

		public ActionResult MyProducts()
		{
			Models.CUser User = new Models.CUser();
			User = User.GetCurrentUser();
			User.ProductList = Models.CProduct.GetProducts();
			return View(User);
		}

		public ActionResult MyProduct()
		{
			Models.CUser User = new Models.CUser();
			User = User.GetCurrentUser();
            if (User.ProductList != null)
            {
                User.ProductList.Clear();
            }
            else
            {
                User.ProductList = new List<Models.CProduct>();
            }
			Models.CProduct Product = new Models.CProduct();
			if (RouteData.Values["id"] != null)
			{
				Product = Models.CProduct.GetProduct(Convert.ToInt32(RouteData.Values["id"]));
				User.ProductList.Add(Product);
			}
			else
			{
				User.ProductList.Add(Product);
			}
			return View(User);
		}

		[HttpPost]
		public ActionResult MyProduct(HttpPostedFileBase PrimaryImage, FormCollection Collection)
		{
			try
			{

				// Cancel
				if (Collection["btnSubmit"] == "cancel") return RedirectToAction("MyProducts");

				Models.CDatabase db = new Models.CDatabase();
				Models.CProduct Product = new Models.CProduct();
				Models.CUser User = new Models.CUser();
				User = User.GetCurrentUser();
				Product.ProductID = (Convert.ToInt32(RouteData.Values["id"]));

				// Delete
				if (Collection["btnSubmit"] == "delete")
				{				
					Product.Delete();
					return RedirectToAction("MyProducts");
				}
				if (Collection["btnSubmit"] == "save")
				{
					Product.Title = (string)Collection["ProductList[0].Title"];
					Product.Price = Convert.ToInt32(Collection["ProductList[0].Price"]);
                    Product.CategoryID = Convert.ToInt32(Collection["ProductList[0].CategoryList[0].intCategoryID"]);
					Product.Description = (string)Collection["ProductList[0].Description"];
					Product.StatusID = Models.StatusTypes.Active;
                    Product.intStockAmount = Convert.ToInt32(Collection["ProductList[0].intStockAmount"]);
                    Product.Save();

					if (PrimaryImage != null)
					{
						Product.PrimaryImage = new Models.CImage();
						Product.PrimaryImage.FileName = System.IO.Path.GetFileName(PrimaryImage.FileName);
						Product.PrimaryImage.FileExtension = System.IO.Path.GetExtension(Product.PrimaryImage.FileName);
						if (Product.PrimaryImage.IsImageFile())
						{
							Product.PrimaryImage.FileSize = PrimaryImage.ContentLength;
							Stream stream = PrimaryImage.InputStream;
							BinaryReader binaryReader = new BinaryReader(stream);
							Product.PrimaryImage.FileBytes = binaryReader.ReadBytes((int)stream.Length);

							// If the product already has an image
							if (db.HasPrimaryImage(Product.ProductID) == true)
							{
								// Edit the existing one
								Product.UpdatePrimaryImage();
							}
							else
							{
								// Otherwise, add an image
								Product.AddPrimaryImage();
							}
							
						}
						else
						{
							Product.PrimaryImage = null;
						}
					}
				}
				return RedirectToAction("MyProducts");
			}
			catch (Exception)
			{
				return null;
			}
		}

		[HttpPost]
		public ActionResult MyProfile(FormCollection Collection)
		{
			try
			{
				Models.CUser User = new Models.CUser();
				Models.CUser CurrentUser = User.GetCurrentUser();

				User.UserID = CurrentUser.UserID;
				User.EmailAddress = Collection["EmailAddress"];
				User.FirstName = Collection["FirstName"];
				User.LastName = Collection["LastName"];
				User.Password = Collection["Password"];

                Models.CUser.ActionStatusTypes Status = User.Save();
                switch (Status)
                {
                    case Models.CUser.ActionStatusTypes.UserAlreadyExists:
                        break;
                    case Models.CUser.ActionStatusTypes.UserUpdated:
                        CurrentUser = null;
                        User.SetCurrentUser();
                        return RedirectToAction("Home", "Home");
                    default:
                        break;
                }
                return View(User);
            }
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

        [HttpPost]
        public ActionResult Login(FormCollection Collection)
        {
            try
            {
                Models.CDatabase db = new Models.CDatabase();
                Models.CUser User = new Models.CUser();
                User.EmailAddress = Collection["EmailAddress"];
                User.Password = Collection["Password"];

				User = User.Login(User.EmailAddress, User.Password);

				if (User.UserID > 0)
				{				
					User.SetCurrentUser();
					return RedirectToAction("Main", "Home");
				}
				else
				{
					return View(User);
				}
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
		public ActionResult SignUp(FormCollection Collection)
		{
			try
			{
                if (ModelState.IsValid)
                {
                    Models.CUser User = new Models.CUser();
                    User.EmailAddress = Collection["EmailAddress"];
                    User.FirstName = Collection["FirstName"];
                    User.LastName = Collection["LastName"];
                    User.Password = Collection["Password"];
                    User.strCity = Collection["strCity"];
                    User.strAddress = Collection["strAddress"];
                    if (Collection["dtmDateOfBirth"] != "")
                    {
                        User.dtmDateOfBirth = Convert.ToDateTime(Collection["dtmDateOfBirth"]);
                    }
                    else
                    {
                        User.dtmDateOfBirth = DateTime.MinValue;
                    }
                    User.intStateID = Convert.ToInt32(Collection["intStateID"]);
                    User.intGenderID = Convert.ToInt32(Collection["intGenderID"]);
                    User.strZipCode = Collection["strZipCode"];
                    string str = Collection["blnEmailList"];
                    if (Collection["blnEmailList"] == "true,false")
                    {
                        User.blnEmailList = true;
                    }
                    else
                    {
                        User.blnEmailList = false;
                    }
                    //User.blnEmailList = Convert.ToInt32(Collection["blnEmailList"]);

                    Models.CUser.ActionStatusTypes Status = User.Save();
                    switch (Status)
                    {
                        case Models.CUser.ActionStatusTypes.UserAlreadyExists:
                            break;
                        case Models.CUser.ActionStatusTypes.UserUpdated:
                            User.SetCurrentUser();
                            return RedirectToAction("MyProfile", "User");
                        default:
                            break;
                    }
                }
				return View(User);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}			
		}
	}
}