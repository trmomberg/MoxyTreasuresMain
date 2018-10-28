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

        // Get Product List \
        public List<CProduct> GetProducts(int intProductID = 0)
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
                    int intCategoryID;
                    int intStatusID;

                    if (intProductID != 0)
                    {
                        SetParameter(ref da, "@intProductID", intProductID, SqlDbType.Int);
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
                                intStatusID				= Convert.ToInt32((dr["intStatusID"]));
                                intCategoryID			= Convert.ToInt32((dr["intCategoryID"]));
								Product.PrimaryImage = GetPrimaryImage(Product.ProductID);                             

                                switch (intStatusID)
                                {
                                    case 0:
                                        Product.StatusID = StatusTypes.Unknown;
                                        break;
                                    case 1:
                                        Product.StatusID = StatusTypes.Active;
                                        break;
                                    case 2:
                                        Product.StatusID = StatusTypes.Sold;
                                        break;
                                    case 3:
                                        Product.StatusID = StatusTypes.Closed;
                                        break;
                                }

                                switch (intCategoryID)
                                {
                                    case 0:
                                        Product.CategoryID = ProductTypes.Unknown;
                                        break;
                                    case 1:
                                        Product.CategoryID = ProductTypes.Necklace;
                                        break;
                                    case 2:
                                        Product.CategoryID = ProductTypes.Bracelet;
                                        break;
                                    case 3:
                                        Product.CategoryID = ProductTypes.Earring;
                                        break;
                                    case 4:
                                        Product.CategoryID = ProductTypes.Ring;
                                        break;
                                    case 5:
                                        Product.CategoryID = ProductTypes.Other;
                                        break;
                                }
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
					int intCategoryID;
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
						intStatusID = Convert.ToInt32((dr["intStatusID"]));
						intCategoryID = Convert.ToInt32((dr["intCategoryID"]));
						Product.PrimaryImage = GetPrimaryImage(Product.ProductID);

						switch (intStatusID)
						{
							case 0:
								Product.StatusID = StatusTypes.Unknown;
								break;
							case 1:
								Product.StatusID = StatusTypes.Active;
								break;
							case 2:
								Product.StatusID = StatusTypes.Sold;
								break;
							case 3:
								Product.StatusID = StatusTypes.Closed;
								break;
						}

						switch (intCategoryID)
						{
                            case 0:
                                Product.CategoryID = ProductTypes.Unknown;
                                break;
                            case 1:
                                Product.CategoryID = ProductTypes.Necklace;
                                break;
                            case 2:
                                Product.CategoryID = ProductTypes.Bracelet;
                                break;
                            case 3:
                                Product.CategoryID = ProductTypes.Earring;
                                break;
                            case 4:
                                Product.CategoryID = ProductTypes.Ring;
                                break;
                            case 5:
                                Product.CategoryID = ProductTypes.Other;
                                break;
                        }
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
                    SetParameter(ref cm, "@intStatusID",    Product.StatusID, SqlDbType.Int);

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

        // Update Product Buyer \
        public int UpdateProductBuyer(int intProductID, int intBuyerID)
        {
            try
            {
                SqlConnection Connection = null;

                if (OpenDBConnection(ref Connection) == 0)
                {
                    SqlCommand cm = new SqlCommand("uspEditProductBuyer");

                    SetParameter(ref cm, "@intProductID", intProductID, SqlDbType.Int);
                    SetParameter(ref cm, "@intBuyerID", intBuyerID, SqlDbType.Int);

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

					SetParameter(ref Command, "@intUserID", User.UserID, SqlDbType.Int);
					SetParameter(ref Command, "@strFirstName", User.FirstName, SqlDbType.VarChar);
					SetParameter(ref Command, "@strLastName", User.LastName, SqlDbType.VarChar);
					SetParameter(ref Command, "@strEmailAddress", User.EmailAddress, SqlDbType.VarChar);
					SetParameter(ref Command, "@strPassword", User.Password, SqlDbType.VarChar);
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
                    SetParameter(ref Command, "@intStatusID", Product.StatusID, SqlDbType.Int);
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

					SetParameter(ref Command, "@intUserID", User.UserID, SqlDbType.Int, Direction: ParameterDirection.Output);
					SetParameter(ref Command, "@strFirstName", User.FirstName, SqlDbType.VarChar);
					SetParameter(ref Command, "@strLastName", User.LastName, SqlDbType.VarChar);
					SetParameter(ref Command, "@strEmailAddress", User.EmailAddress, SqlDbType.VarChar);
					SetParameter(ref Command, "@strPassword", User.Password, SqlDbType.VarChar);
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

        // Update Product image\
        public int UpdateProductImage(int intProductID, string strFileName, string strFileExtension, int intFileSize, byte[] bytes)
        {
            try
            {
                SqlConnection Connection = null;
                int intIsPrimary = 0;

                if (OpenDBConnection(ref Connection) == 0)
                {
                    SqlCommand cm = new SqlCommand("uspEditImage", Connection);

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