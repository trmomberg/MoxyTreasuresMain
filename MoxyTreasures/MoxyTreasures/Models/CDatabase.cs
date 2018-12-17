using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace MoxyTreasures.Models
{
	public class CDatabase
	{
        #region Connection
        // Open Connection \/
        private int OpenDBConnection(ref SqlConnection SQLConn)
        {
            try
            {
                if (SQLConn == null) SQLConn = new SqlConnection();
                if (SQLConn.State != System.Data.ConnectionState.Open)
                {
                    SQLConn.ConnectionString = Properties.Settings.Default.ApplicationDBConnectionString;
                    SQLConn.Open();        
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Close Connection \/
        private int CloseDBConnection(ref SqlConnection SQLConn)
        {
            try
            {
                if (SQLConn.State != ConnectionState.Closed)
                {
                    SQLConn.Close();
                    SQLConn = null;
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Functions
        // Get Product List \
        public List<CProduct> GetProducts(int intProductID = 0, int intCategoryID = -1)
        {

            SqlConnection Connection = new SqlConnection();
            List<CProduct> ProductList = new List<CProduct>();
            
            try
            {
                
                if (OpenDBConnection(ref Connection) == 0)
                { 
                    SqlDataAdapter da = new SqlDataAdapter("uspSelectProduct", Connection);
                    da.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    DataSet ds = new DataSet();
                    int intStatusID;

                    if (intProductID != 0)
                    {
                        SetParameter(ref da, "@intProductID", intProductID, SqlDbType.Int);
                    }

                    if (intCategoryID > -1)
                    {
                        SetParameter(ref da, "@intCategoryID", intCategoryID, SqlDbType.Int);
                    }
                    try
                    {
                        da.Fill(ds);

                        foreach (DataTable dt in ds.Tables)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
								CProduct Product = new CProduct();
								Product.ProductID		= Convert.ToInt32((dr["intProductID"]));
                                Product.Title			= (dr["strTitle"]).ToString();
                                Product.Description		= (dr["strDescription"]).ToString();
                                Product.Price			= Convert.ToDouble((dr["intPrice"]));
                                Product.CategoryID			= Convert.ToInt32((dr["intCategoryID"]));
								Product.PrimaryImage = GetPrimaryImage(Product.ProductID);                             
                                Product.intStockAmount = Convert.ToInt32((dr["intStockAmount"]));

                                ProductList.Add(Product);
                            }
                        }                                             
                    }
                    finally
                    {
                        CloseDBConnection(ref Connection);
                    }                                   
                }
                return ProductList;
            }
            catch (Exception ex )
            {
                throw new Exception(ex.Message);              
            }
        }

		// Get Product 
		public CProduct GetProduct(int intProductID)
		{

			SqlConnection Connection = new SqlConnection();
			CProduct Product = new CProduct();

			try
			{

				if (OpenDBConnection(ref Connection) == 0)
				{
					SqlDataAdapter da = new SqlDataAdapter("uspSelectProduct", Connection);
					da.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
					DataSet ds = new DataSet();
					int intStatusID;
					
					SetParameter(ref da, "@intProductID", intProductID, SqlDbType.Int);

					try
					{
						da.Fill(ds);
						DataTable dt = ds.Tables[0];
						DataRow dr = dt.Rows[0];

						Product.ProductID = Convert.ToInt32((dr["intProductID"]));
						Product.Title = (dr["strTitle"]).ToString();
						Product.Description = (dr["strDescription"]).ToString();
						Product.Price = Convert.ToDouble((dr["intPrice"]));
						Product.CategoryID = Convert.ToInt32((dr["intCategoryID"]));
                        Product.intStockAmount = Convert.ToInt32((dr["intStockAmount"]));
                        Product.PrimaryImage = GetPrimaryImage(Product.ProductID);

					}

					finally
					{
						CloseDBConnection(ref Connection);
					}
				}
				return Product;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

        // Get Address List \
        public CAddress GetDefaultAddress(int intUserID)
        {

            SqlConnection Connection = new SqlConnection();
            List<CAddress> AddressList = new List<CAddress>();

            try
            {

                if (OpenDBConnection(ref Connection) == 0)
                {
                    SqlDataAdapter da = new SqlDataAdapter("uspSelectAddresses", Connection);
                    SetParameter(ref da, "@intUserID", intUserID, SqlDbType.Int);
                    da.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    DataSet ds = new DataSet();

                    try
                    {
                        da.Fill(ds);

                        foreach (DataTable dt in ds.Tables)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                CAddress Address = new CAddress();
                                Address.intAddressID = Convert.ToInt32((dr["intAddressID"]));
                                Address.strAddress = (dr["strStreetAddress"]).ToString();
                                Address.intStateID = Convert.ToInt32((dr["intStateID"]));
                                Address.strCity = (dr["strCity"]).ToString();
                                Address.strZipCode = (dr["strZipCode"]).ToString();

                                AddressList.Add(Address);
                            }
                        }
                    }
                    finally
                    {
                        CloseDBConnection(ref Connection);
                    }
                }
                return AddressList[0];
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Get Address List \
        public List<CAddress> GetAddressList(int intUserID = 0)
        {

            SqlConnection Connection = new SqlConnection();
            List<CAddress> AddressList = new List<CAddress>();

            try
            {

                if (OpenDBConnection(ref Connection) == 0)
                {
                    SqlDataAdapter da = new SqlDataAdapter("uspSelectAddresses", Connection);
                    if (intUserID > 0)
                    {
                        SetParameter(ref da, "@intUserID", intUserID, SqlDbType.Int);
                    }
                    else
                    {
                        SetParameter(ref da, "@intUserID", null, SqlDbType.Int);
                    }
                    da.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    DataSet ds = new DataSet();

                    try
                    {
                        da.Fill(ds);

                        foreach (DataTable dt in ds.Tables)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                CAddress Address = new CAddress();
                                Address.intAddressID = Convert.ToInt32((dr["intAddressID"]));
                                Address.strAddress = (dr["strStreetAddress"]).ToString();
                                Address.intStateID = Convert.ToInt32((dr["intStateID"]));
                                Address.strCity = (dr["strCity"]).ToString();
                                Address.strZipCode = (dr["strZipCode"]).ToString();

                                AddressList.Add(Address);
                            }
                        }
                    }
                    finally
                    {
                        CloseDBConnection(ref Connection);
                    }
                }
                return AddressList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Get User \/
        public CUser GetUser(int intUserID)
        {
            
			SqlConnection Connection = new SqlConnection();
			CUser User = new CUser();
            int intAdmin = 0;
			//List<CUser> UserList = new List<CUser>();

			try
			{

				if (OpenDBConnection(ref Connection) == 0)
				{
					SqlDataAdapter da = new SqlDataAdapter("uspSelectUser", Connection);
					da.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
					DataSet ds = new DataSet();

					if (intUserID != 0)
					{
						SetParameter(ref da, "@intUserID", intUserID, SqlDbType.Int);
					}
					else
					{
						SetParameter(ref da, "@intUserID", null, SqlDbType.Int);
					}

					try
					{
						da.Fill(ds);

						foreach (DataTable dt in ds.Tables)
						{
							foreach (DataRow dr in dt.Rows)
							{
								User.UserID			= Convert.ToInt32((dr["intUserID"]));
								User.FirstName		= (dr["strFirstName"]).ToString();
								User.LastName		= (dr["strLastName"]).ToString();
								User.EmailAddress	= (dr["strEmailAddress"]).ToString();
                                intAdmin            = (int)dr["blnAdmin"];

                                if (intAdmin == 1)
                                {
                                    User.blnAdmin = true;
                                }

								//UserList.Add(User);
							}
						}
					}
					finally
					{
						CloseDBConnection(ref Connection);
					}
				}
				// return UserList;
				return User;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
        }

        // Get User \/
        public List<CCategories> GetCategories(int intCategoryID = 0)
        {

            SqlConnection Connection = new SqlConnection();
            List<CCategories> CategoryList = new List<CCategories>();

            try
            {

                if (OpenDBConnection(ref Connection) == 0)
                {
                    SqlDataAdapter da = new SqlDataAdapter("uspSelectCategory", Connection);
                    da.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    DataSet ds = new DataSet();

                    if (intCategoryID != 0)
                    {
                        SetParameter(ref da, "@intCategoryID", intCategoryID, SqlDbType.Int);
                    }
                    else
                    {
                        SetParameter(ref da, "@intCategoryID", null, SqlDbType.Int);
                    }

                    try
                    {
                        da.Fill(ds);

                        foreach (DataTable dt in ds.Tables)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                CCategories Category = new CCategories();
                                Category.intCategoryID = Convert.ToInt32((dr["intCategoryID"]));
                                Category.strCategory = (dr["strCategory"]).ToString();

                                CategoryList.Add(Category);
                            }
                        }
                    }
                    finally
                    {
                        CloseDBConnection(ref Connection);
                    }
                }
                // return CategoryList;
                return CategoryList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Get Order \/
        public COrder GetOrder(int intOrderID = 0, int intUserID = 0, int blncart = 0)
        {

            SqlConnection Connection = new SqlConnection();
           COrder Order = new COrder();

            try
            {

                if (OpenDBConnection(ref Connection) == 0)
                {
                    SqlDataAdapter da = new SqlDataAdapter("uspSelectOrder", Connection);
                    da.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    DataSet ds = new DataSet();

                    if (intUserID != 0)
                    {
                        SetParameter(ref da, "@intUserID", intUserID, SqlDbType.Int);
                    }

                    if (intOrderID != 0)
                    {
                        SetParameter(ref da, "@intOrderID", intOrderID, SqlDbType.Int);
                    }

                    if (blncart != 0)
                    {
                        SetParameter(ref da, "@blnCart", blncart, SqlDbType.Bit);
                    }

                    try
                    {
                        da.Fill(ds);

                        foreach (DataTable dt in ds.Tables)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                Order.intOrderID = Convert.ToInt32((dr["intOrderID"]));
                                Order.intUserID = Convert.ToInt32((dr["intUserID"]));
                                Order.intTotal = Convert.ToInt32((dr["intTotal"]));
                                Order.intShippingAddressID = Convert.ToInt32((dr["intShippingAddressID"]));
                                Order.intStatusID = Convert.ToInt32((dr["intStatusID"]));
                            }
                        }
                    }
                    finally
                    {
                        CloseDBConnection(ref Connection);
                    }
                }
                // return Order;
                return Order;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Get Primary Image \
        public CImage GetPrimaryImage(int intProductID)
        {
            try
            {
                SqlConnection Connection = null;

                if (OpenDBConnection(ref Connection) == 0)
                {
                    SqlDataAdapter da = new SqlDataAdapter("uspSelectProductPrimaryImage", Connection);
                    DataSet ds = new DataSet();

                    SetParameter(ref da, "@intProductID", intProductID, SqlDbType.Int);

                    try
                    {
                        da.Fill(ds);
                    }
                    finally
                    {
                        CloseDBConnection(ref Connection);
                    }

                    // Load the primary Image
                    CImage PrimaryImage = new CImage();
                    if (ds.Tables.Count != 0)
                    {
                        DataRow dr;
                        dr = ds.Tables[0].Rows[0];
                        PrimaryImage.ImageID = (int)dr["intImageID"];
                        PrimaryImage.FileName = (string)dr["strFileName"];
                        PrimaryImage.FileExtension = (string)dr["strImageType"];
                        PrimaryImage.FileSize = (int)dr["intImageSize"];
                        PrimaryImage.FileBytes = (byte[])dr["ImageData"];
                    }

                    return PrimaryImage;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<CProduct> GetCart(int intUserID)
        {
            try
            {
                SqlConnection Connection = null;
                CProduct Product = new CProduct();
                List<CProduct> Cart = new List<CProduct>();
                int intProductID = 0;

                if (OpenDBConnection(ref Connection) == 0)
                {
                    SqlDataAdapter da = new SqlDataAdapter("uspGetCart", Connection);
                    da.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    DataSet ds = new DataSet();

                    SetParameter(ref da, "@intUserID", intUserID, SqlDbType.Int);

                    da.Fill(ds);

                    foreach (DataTable dt in ds.Tables)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            intProductID = (int)dr["intProductID"];
                            Product = GetProduct(intProductID);
                            Cart.Add(Product);
                        }
                    }

                }

                CloseDBConnection(ref Connection);
                return Cart;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Delete Functions
        // Delete Product \
        public bool DeleteProduct(int intProductID = 0)
        {
            try
            {

                SqlConnection Connection = new SqlConnection();
                if (OpenDBConnection(ref Connection) == 0)
                {
                    SqlCommand cm = new SqlCommand("uspDeleteProduct", Connection);

                    if (intProductID > 0)
                    {
                        SetParameter(ref cm, "@intProductID", intProductID, SqlDbType.Int);
                    }
                    cm.ExecuteNonQuery();

                    CloseDBConnection(ref Connection);

                    return true;
                }
                else
                {
                    return false;
                }
                               
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Delete User \
        public bool DeleteUser(int intUserID)
        {
            try
            {

                SqlConnection Connection = new SqlConnection();
                if (OpenDBConnection(ref Connection) == 0)
                {
                    SqlCommand cm = new SqlCommand("DeleteUser", Connection);

                    SetParameter(ref cm, "@intUserID", intUserID, SqlDbType.Int);
                    
                    cm.ExecuteNonQuery();

                    CloseDBConnection(ref Connection);
                }
                else
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Delete Product \
        public bool DeleteCategory(int intCategoryID = 0)
        {
            try
            {

                SqlConnection Connection = new SqlConnection();
                if (OpenDBConnection(ref Connection) == 0)
                {
                    SqlCommand cm = new SqlCommand("uspDeleteCategory", Connection);

                    if (intCategoryID > 0)
                    {
                        SetParameter(ref cm, "@intCategoryID", intCategoryID, SqlDbType.Int);
                    }
                    cm.ExecuteNonQuery();

                    CloseDBConnection(ref Connection);

                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Delete Address \
        public bool DeleteAddress(int intAddressID = 0, int intUserID = 0)
        {
            try
            {

                SqlConnection Connection = new SqlConnection();
                if (OpenDBConnection(ref Connection) == 0)
                {
                    SqlCommand cm = new SqlCommand("uspDeleteAddress", Connection);

                    if (intAddressID > 0)
                    {
                        SetParameter(ref cm, "@intAddressID", intAddressID, SqlDbType.Int);
                    }

                    if (intUserID > 0)
                    {
                        SetParameter(ref cm, "@intUserID", intUserID, SqlDbType.Int);
                    }
                    cm.ExecuteNonQuery();

                    CloseDBConnection(ref Connection);

                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Update Functions

        // Update Product \
        public int UpdateProduct(CProduct Product)
        {
            try
            {
                SqlConnection Connection = null;

                if (OpenDBConnection(ref Connection) == 0)
                {
                    SqlCommand cm = new SqlCommand("uspEditProduct", Connection);

                    SetParameter(ref cm, "@intProductID",   Product.ProductID, SqlDbType.Int);
                    SetParameter(ref cm, "@intCategoryID",  Product.CategoryID, SqlDbType.Int);
                    SetParameter(ref cm, "@strTitle",       Product.Title, SqlDbType.VarChar);
                    SetParameter(ref cm, "@strDescription", Product.Description, SqlDbType.VarChar);
                    SetParameter(ref cm, "@intPrice",       Product.Price, SqlDbType.Int);
                    SetParameter(ref cm, "@intStockAmount", Product.intStockAmount, SqlDbType.Int);

                    cm.ExecuteReader();
                    CloseDBConnection(ref Connection);

                    return 0; // Success
                }
                else
                {
                    return -1; // No database connection
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Update Product \
        public int UpdateOrder(COrder Order)
        {
            try
            {
                SqlConnection Connection = null;

                if (OpenDBConnection(ref Connection) == 0)
                {
                    SqlCommand cm = new SqlCommand("uspEditOrder", Connection);

                    SetParameter(ref cm, "@intOrderID", Order.intOrderID, SqlDbType.Int);
                    SetParameter(ref cm, "@intTotal", Order.intTotal, SqlDbType.Int);
                    //SetParameter(ref cm, "@dmtDateOfOrder", Order.dtmDateOfOrder, SqlDbType.DateTime);
                    SetParameter(ref cm, "@intAddressID", Order.intShippingAddressID, SqlDbType.Int);
                    SetParameter(ref cm, "@intStatusID", Order.intStatusID, SqlDbType.Int);

                    cm.ExecuteReader();
                    CloseDBConnection(ref Connection);

                    return 0; // Success
                }
                else
                {
                    return -1; // No database connection
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Update User \
        public CUser.ActionStatusTypes UpdateUser(CUser User)
        {
            try
            {
				SqlConnection Connection = null;
				if (OpenDBConnection(ref Connection) == 0)
				{
					SqlCommand Command = new SqlCommand("uspEditUser", Connection);
					int ReturnValue = -1;
                    int intEmailList = 0;
                    if (User.blnEmailList == true)
                    {
                        intEmailList = 1;
                    }

                    SetParameter(ref Command, "@intUserID", User.UserID, SqlDbType.Int);
					SetParameter(ref Command, "@strFirstName", User.FirstName, SqlDbType.VarChar);
					SetParameter(ref Command, "@strLastName", User.LastName, SqlDbType.VarChar);
					SetParameter(ref Command, "@strEmailAddress", User.EmailAddress, SqlDbType.VarChar);
					SetParameter(ref Command, "@strPassword", User.Password, SqlDbType.VarChar);
                    SetParameter(ref Command, "@dtmDateOfBirth", User.dtmDateOfBirth, SqlDbType.DateTime);
                    SetParameter(ref Command, "@intStateID", User.intStateID, SqlDbType.VarChar);
                    SetParameter(ref Command, "@strCity", User.strCity, SqlDbType.VarChar);
                    SetParameter(ref Command, "@intGenderID", User.intGenderID, SqlDbType.VarChar);
                    SetParameter(ref Command, "@strZipCode", User.strZipCode, SqlDbType.VarChar);
                    SetParameter(ref Command, "@blnEmailList", intEmailList, SqlDbType.Int);
                    SetParameter(ref Command, "ReturnValue", 0, SqlDbType.Int, Direction: ParameterDirection.ReturnValue);

					Command.ExecuteReader();

					ReturnValue = (int)Command.Parameters["ReturnValue"].Value;
					CloseDBConnection(ref Connection);

					switch (ReturnValue)
					{
						case 1: // User Updated
							return CUser.ActionStatusTypes.UserUpdated;
						case 2: // User Already Exists
							return CUser.ActionStatusTypes.UserAlreadyExists;
						default:
							return CUser.ActionStatusTypes.Unknown;
					}
				}
				else
				{
					return CUser.ActionStatusTypes.Unknown;
				}
				
			}
            catch (Exception ex)
            {
				throw new Exception(ex.Message);
            }
        }

        // Update Product image\
        public int UpdateProductImage(int intImageID, int intProductID, string strFileName, string strFileExtension, int intFileSize, byte[] bytes)
        {
            try
            {
                SqlConnection Connection = null;
                int intIsPrimary = 1;

                if (OpenDBConnection(ref Connection) == 0)
                {
                    SqlCommand cm = new SqlCommand("uspEditImage", Connection);

                    SetParameter(ref cm, "@intImageID", intImageID, SqlDbType.Int);
                    SetParameter(ref cm, "@intProductID", intProductID, SqlDbType.Int);
                    SetParameter(ref cm, "@blnPrimaryImage", intIsPrimary, SqlDbType.Bit);
                    SetParameter(ref cm, "@strFileName", strFileName, SqlDbType.VarChar);
                    SetParameter(ref cm, "@strImageType", strFileExtension, SqlDbType.VarChar);
                    SetParameter(ref cm, "@intImageSize", intFileSize, SqlDbType.Int);
                    SetParameter(ref cm, "@ImageData", bytes, SqlDbType.VarBinary);

                    cm.ExecuteReader();
                    CloseDBConnection(ref Connection);

                    return 0; //success
                }
                else
                {
                    return -1; // No database connection
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Insert Functions

        // Insert Product \
        public int InsertProduct(CProduct Product)
        {
            try
            {
                SqlConnection Connection = null;
                if (OpenDBConnection(ref Connection) == 0)
                {
                    SqlCommand Command = new SqlCommand("uspAddProduct", Connection);
                    int intID = -1;

                    SetParameter(ref Command, "@intProductID", 0, SqlDbType.Int, Direction: ParameterDirection.Output);
                    SetParameter(ref Command, "@strTitle", Product.Title, SqlDbType.VarChar);
                    SetParameter(ref Command, "@strDescription", Product.Description, SqlDbType.VarChar);
                    SetParameter(ref Command, "@intPrice", Product.Price, SqlDbType.Int);
                    SetParameter(ref Command, "@intStockAmount", Product.intStockAmount, SqlDbType.Int);
                    SetParameter(ref Command, "@intCategoryID", Product.CategoryID, SqlDbType.Int);

                    Command.ExecuteReader();

                    intID = (int)Command.Parameters["@intProductID"].Value;
					Product.ProductID = intID;
                    CloseDBConnection(ref Connection);
                    return 0; // Success
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Insert User \/
        public CUser.ActionStatusTypes InsertUser(CUser User)
        {
            try
            {
				SqlConnection Connection = null;
				if (OpenDBConnection(ref Connection) == 0)
				{
					SqlCommand Command = new SqlCommand("uspAddUser", Connection);
					int ReturnValue = -1;
                    int intEmailList = 0;
                    if (User.blnEmailList == true)
                    {
                        intEmailList = 1;
                    }

					SetParameter(ref Command, "@intUserID", User.UserID, SqlDbType.Int, Direction: ParameterDirection.Output);
					SetParameter(ref Command, "@strFirstName", User.FirstName, SqlDbType.VarChar);
					SetParameter(ref Command, "@strLastName", User.LastName, SqlDbType.VarChar);
					SetParameter(ref Command, "@strEmailAddress", User.EmailAddress, SqlDbType.VarChar);
                    SetParameter(ref Command, "@dtmDateOfBirth", User.dtmDateOfBirth, SqlDbType.DateTime);
                    SetParameter(ref Command, "@strPassword", User.Password, SqlDbType.VarChar);
                    SetParameter(ref Command, "@intStateID", User.intStateID, SqlDbType.Int);
                    SetParameter(ref Command, "@intGenderID", User.intGenderID, SqlDbType.Int);
                    SetParameter(ref Command, "@strCity", User.strCity, SqlDbType.VarChar);
                    SetParameter(ref Command, "@strStreetAddress", User.strAddress, SqlDbType.VarChar);
                    SetParameter(ref Command, "@strZipCode", User.strZipCode, SqlDbType.VarChar);
                    SetParameter(ref Command, "@blnAdmin", 0, SqlDbType.Int);
                    SetParameter(ref Command, "@blnEmailList", intEmailList, SqlDbType.Int);
                    SetParameter(ref Command, "ReturnValue", 0, SqlDbType.Int, Direction: ParameterDirection.ReturnValue);

					Command.ExecuteReader();

					ReturnValue = (int)Command.Parameters["ReturnValue"].Value;
					CloseDBConnection(ref Connection);

					switch (ReturnValue)
					{
						case 1: // User Updated
							User.UserID = (int)Command.Parameters["@intUserID"].Value;
							return CUser.ActionStatusTypes.UserUpdated;
						case 2: // User Already Exists
							return CUser.ActionStatusTypes.UserAlreadyExists;
						default:
							return CUser.ActionStatusTypes.Unknown;
					}
				}
				else
				{
					return CUser.ActionStatusTypes.Unknown;
				}
			}
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Insert Product image \
        public int InsertProductImage(int intProductID, string strFileName, string strFileExtension, int intFileSize, byte[] bytes)
        {
            try
            {
                CImage NewImage = new CImage();
                SqlConnection Connection = null;
                int intIsPrimary = 0;

                if (OpenDBConnection(ref Connection) == 0)
                {
                    SqlCommand cm = new SqlCommand("uspAddImage", Connection);

                    // If there is no primary image for the product, set this image to primary
                    if (HasPrimaryImage(intProductID) == false) { intIsPrimary = 1; };

                    SetParameter(ref cm, "@intProductID", intProductID, SqlDbType.Int);
                    SetParameter(ref cm, "@blnPrimaryImage", intIsPrimary, SqlDbType.Bit);
                    SetParameter(ref cm, "@strFileName", strFileName, SqlDbType.VarChar);
                    SetParameter(ref cm, "@strImageType", strFileExtension, SqlDbType.VarChar);
                    SetParameter(ref cm, "@intImageSize", intFileSize, SqlDbType.Int);
					SetParameter(ref cm, "@ImageData", bytes, SqlDbType.VarBinary);

					cm.ExecuteReader();

                    CloseDBConnection(ref Connection);

                    return 0; //success
                }
                else
                {
                    return -1; // No database connection
                }
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Insert Category \
        public int InsertCategory(string strCategory)
        {
            try
            {
                SqlConnection Connection = null;

                if (OpenDBConnection(ref Connection) == 0)
                {
                    CCategories category = new CCategories();
                    int intReturnID = 0;

                    SqlCommand cm = new SqlCommand("uspAddCategory", Connection);

                    SetParameter(ref cm, "@strCategory", strCategory, SqlDbType.VarChar);
                    SetParameter(ref cm, "ReturnValue", 0, SqlDbType.Int, Direction: ParameterDirection.ReturnValue);

                    cm.ExecuteReader();

                    CloseDBConnection(ref Connection);

                    if (!Convert.IsDBNull(cm.Parameters["ReturnValue"].Value))
                    {
                        intReturnID = (int)cm.Parameters["ReturnValue"].Value;
                        
                    }

                    return intReturnID; //success
                }
                else
                {
                    return -1; // No database connection
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Insert Address \
        public int InsertAddress(CAddress Address)
        {
            try
            {
                SqlConnection Connection = null;

                if (OpenDBConnection(ref Connection) == 0)
                {
                    int intReturnID = 0;
                    CUser User = new CUser();
                    User = User.GetCurrentUser();

                    SqlCommand cm = new SqlCommand("uspAddAddress", Connection);

                    SetParameter(ref cm, "@intUserID", User.UserID, SqlDbType.Int);
                    SetParameter(ref cm, "@strStreetAddress", Address.strAddress, SqlDbType.VarChar);
                    SetParameter(ref cm, "@strCity", Address.strCity, SqlDbType.VarChar);
                    SetParameter(ref cm, "@intStateID", Address.intStateID, SqlDbType.Int);
                    SetParameter(ref cm, "@strZipCode", Address.strZipCode, SqlDbType.VarChar);
                    SetParameter(ref cm, "ReturnValue", 0, SqlDbType.Int, Direction: ParameterDirection.ReturnValue);

                    cm.ExecuteReader();

                    CloseDBConnection(ref Connection);

                    if (!Convert.IsDBNull(cm.Parameters["ReturnValue"].Value))
                    {
                        intReturnID = (int)cm.Parameters["ReturnValue"].Value;

                    }

                    return intReturnID; //success
                }
                else
                {
                    return -1; // No database connection
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        // Has Primary Image \
        public bool HasPrimaryImage(int intProductID)
        {
            try
            {
                bool blnHasPrimary = false;
                SqlConnection Connection = null;

                if (OpenDBConnection(ref Connection) == 0)
                {

                    SqlDataAdapter da = new SqlDataAdapter("uspSelectProductPrimaryImage", Connection);
                    DataSet ds = new DataSet();

                    da.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    SetParameter(ref da, "@intProductID", intProductID, SqlDbType.Int);

                    try
                    {
                        da.Fill(ds);
                    }
                    finally
                    {
                        CloseDBConnection(ref Connection);
                    }

					// If the select returns anything, then there is a primary image
					if (ds.Tables[0].Rows.Count != 0)
                    {
						////DataRow dr = ds.Tables[0].Rows[0];
						//intImageID = (int)(dr["intImageID"]);

						//if (intImageID > 0)
						//{
							blnHasPrimary = true;
						//}					
                    }
                }

                return blnHasPrimary;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        
        // login \/
        public int Login(string strEmailAddress, string strPassword)
        {
            try
            {
                SqlConnection Connection = null;
                int intReturnID = 0;

                if (OpenDBConnection(ref Connection) == 0)
                {
                    SqlCommand cm = new SqlCommand("uspLogin", Connection);
                    
                    SetParameter(ref cm, "@strUserName", strEmailAddress, SqlDbType.VarChar);
                    SetParameter(ref cm, "@strPassword", strPassword, SqlDbType.VarChar);
                    SetParameter(ref cm, "ReturnValue", 0, SqlDbType.Int, Direction: ParameterDirection.ReturnValue);

                    cm.ExecuteReader();

                    if (!Convert.IsDBNull(cm.Parameters["ReturnValue"].Value))
                    {
                        intReturnID = (int)cm.Parameters["ReturnValue"].Value;
                    }                    
                }
                CloseDBConnection(ref Connection);
                return intReturnID;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int ToggleCart(int intUserID, int intProductID)
        {
            try
             {
                SqlConnection Connection = null;
                int intToggle = 0;

                if (OpenDBConnection(ref Connection) == 0)
                {
                    SqlCommand Command = new SqlCommand("uspToggleCart", Connection);                   

                    SetParameter(ref Command, "@intProductID", intProductID, SqlDbType.Int);
                    SetParameter(ref Command, "@intUserID", intUserID, SqlDbType.Int);
                    SetParameter(ref Command, "ReturnValue", 0, SqlDbType.Int, Direction: ParameterDirection.ReturnValue);

                    Command.ExecuteReader();

                    if (!Convert.IsDBNull(Command.Parameters["ReturnValue"].Value))
                    {
                        intToggle = (int)Command.Parameters["ReturnValue"].Value;
                    }

                    if (intToggle == 1)
                    {
                        return 1; // Toggled on
                    }
                    else if (intToggle == -1)
                    {
                        return -1; // Toggled off
                    }
                }

                CloseDBConnection(ref Connection);
                return intToggle;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

		


		public bool IsWatched(int intProductID, int intUserID)
        {
            try
            {
                SqlConnection Connection = null;
                bool blnIsWatched = false;
                int intReturnValue = 0;

                if (OpenDBConnection(ref Connection) == 0)
                {
                    SqlCommand Command = new SqlCommand("uspIsWatched", Connection);

                    SetParameter(ref Command, "@intProductID", intProductID, SqlDbType.Int);
                    SetParameter(ref Command, "@intUserID", intUserID, SqlDbType.Int);
                    SetParameter(ref Command, "ReturnValue", 0, SqlDbType.Int, Direction: ParameterDirection.ReturnValue);

                    Command.ExecuteReader();

                    if (!Convert.IsDBNull(Command.Parameters["ReturnValue"].Value))
                    {
                        intReturnValue = (int)Command.Parameters["ReturnValue"].Value;
                    }

                    if (intReturnValue == 1)
                    {
                        return true; // is being watched
                    }
                    else if (intReturnValue == 0)
                    {
                        return false; // is NOT being watched
                    }
                }

                CloseDBConnection(ref Connection);
                return blnIsWatched;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private int SetParameter(ref SqlCommand cm, string ParameterName, Object Value
            , System.Data.SqlDbType ParameterType, int FieldSize = -1
            , System.Data.ParameterDirection Direction = System.Data.ParameterDirection.Input
            , Byte Precision = 0, Byte Scale = 0)
        {
            try
            {
                cm.CommandType = System.Data.CommandType.StoredProcedure;
                if (FieldSize == -1)
                    cm.Parameters.Add(ParameterName, ParameterType);
                else
                    cm.Parameters.Add(ParameterName, ParameterType, FieldSize);

                if (Precision > 0) cm.Parameters[cm.Parameters.Count - 1].Precision = Precision;
                if (Scale > 0) cm.Parameters[cm.Parameters.Count - 1].Scale = Scale;

                cm.Parameters[cm.Parameters.Count - 1].Value = Value;
                cm.Parameters[cm.Parameters.Count - 1].Direction = Direction;

                return 0;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private int SetParameter(ref SqlDataAdapter cm, string ParameterName, Object Value
            , System.Data.SqlDbType ParameterType, int FieldSize = -1
            , System.Data.ParameterDirection Direction = System.Data.ParameterDirection.Input
            , Byte Precision = 0, Byte Scale = 0)
        {
            try
            {
                cm.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                if (FieldSize == -1)
                    cm.SelectCommand.Parameters.Add(ParameterName, ParameterType);
                else
                    cm.SelectCommand.Parameters.Add(ParameterName, ParameterType, FieldSize);

                if (Precision > 0) cm.SelectCommand.Parameters[cm.SelectCommand.Parameters.Count - 1].Precision = Precision;
                if (Scale > 0) cm.SelectCommand.Parameters[cm.SelectCommand.Parameters.Count - 1].Scale = Scale;

                cm.SelectCommand.Parameters[cm.SelectCommand.Parameters.Count - 1].Value = Value;
                cm.SelectCommand.Parameters[cm.SelectCommand.Parameters.Count - 1].Direction = Direction;

                return 0;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}