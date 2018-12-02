﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;


namespace MoxyTreasures.Models
{
    public class CProduct
    {
		public CProduct()
		{
			this.StatusTypesList = new List<SelectListItem>();
			this.StatusTypesList.Add(new SelectListItem() { Value = "0", Text = " " });
			this.StatusTypesList.Add(new SelectListItem() { Value = "1", Text = " " });
			this.StatusTypesList.Add(new SelectListItem() { Value = "2", Text = " " });

        }

        public int          ProductID     { get; set; }  
        public string       Title         { get; set; }
        public string       Description   { get; set; }
        public double       Price         { get; set; }
        public CImage       PrimaryImage  { get; set; }
        public StatusTypes  StatusID      { get; set; }
        [DisplayName("Category")]
        public int CategoryID    { get; set; }
        public double dblCartSubTotal { get; set; }
        [DisplayName("Stock Amount")]
        public int intStockAmount { get; set; }
        public List<SelectListItem> StatusTypesList;
        public ActionStatusTypes ActionStatus { get; set; }
        public List<CCategories> CategoryList = CCategories.GetCategories();

        public enum ActionStatusTypes
        {
            Unknown = 0,
            ToggleWatchOn = 1,
            ToggleWatchOff = 2,
        }
        public static CProduct GetProduct(int intProductID)
        {
            try
            {
                CDatabase db = new CDatabase();
                List<CProduct> Products = db.GetProducts(intProductID);
                return Products[0];
                
            }
            catch (Exception)
            {
                return null;
            }

        }

        public static List<CProduct> GetProducts()
        {
            try
            {
                CDatabase db = new CDatabase();
                List<CProduct> Products = db.GetProducts();
                return Products;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public int Save()
        {
            try
            {
                CDatabase Database = new CDatabase();
                if (this.ProductID != 0)
                {
                    Database.UpdateProduct(this);
                }
                else
                {
                    Database.InsertProduct(this);
                }  

                return 0; // Success
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int UpdatePrimaryImage()
        {
            try
            {
                CDatabase Database = new CDatabase();
                CImage NewImage = new CImage();

                int intImageID;
                NewImage = Database.GetPrimaryImage(this.ProductID);
                intImageID = NewImage.ImageID;

                Database.UpdateProductImage(intImageID, this.ProductID, this.PrimaryImage.FileName, this.PrimaryImage.FileExtension, this.PrimaryImage.FileSize, this.PrimaryImage.FileBytes);
                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

		public int AddPrimaryImage()
		{
			try
			{
				CDatabase Database = new CDatabase();
				CImage NewImage = new CImage();
				Database.InsertProductImage(this.ProductID, this.PrimaryImage.FileName, this.PrimaryImage.FileExtension, this.PrimaryImage.FileSize, this.PrimaryImage.FileBytes);
				return 0;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}

		}

		public int Delete()
		{
			try
			{
				CDatabase db = new CDatabase();
				db.DeleteProduct(this.ProductID);
				return 0; // success
			}
			catch (Exception)
			{
				return -1;
			}
		}
    }

    public enum ProductTypes : int
    {
        Unknown = 0,
        Necklace = 1,
        Bracelet = 2,
        Earring = 3,
        Ring = 4,
        Other = 5

    }

    public enum StatusTypes : int
    {
        Unknown = 0,
        Active = 1,
        Sold = 2,
        Closed = 3
    }
}