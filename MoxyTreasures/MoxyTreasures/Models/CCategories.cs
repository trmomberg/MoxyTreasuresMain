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
    public class CCategories
    {
        public int intCategoryID { get; set; }
        [DisplayName("Category Name")]
        public string strCategory { get; set; }
        public int UserID  { get; set; }
        public bool blnAdmin = true;
        public List<CCategories> CategoryList = new List<CCategories>();
        public ActionStatusTypes ActionStatus { get; set; }

        public enum ActionStatusTypes
        {
            Unknown = 0,
            CategoryAdded = 1,
            CategoryAddFailed = 2,
            CategoryDeleted = 3,
            CategoryDeleteFailed = 4,
            SelectACategory = 5
               
        }

        public static CCategories GetCategory(int intCategoryID)
        {
            try
            {
                CDatabase db = new CDatabase();
                List<CCategories> Categories = db.GetCategories(intCategoryID);
                return Categories[0];

            }
            catch (Exception)
            {
                return null;
            }

        }

        public static List<CCategories> GetCategories()
        {
            try
            {
                CDatabase db = new CDatabase();
                List<CCategories> Categories = db.GetCategories();
                return Categories;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public CCategories AddCategory(string strCategory)
        {
            try
            {
                CDatabase Database = new CDatabase();
                CCategories Category = new CCategories();
                int intCategoryID = Database.InsertCategory(strCategory);

                if (intCategoryID > 0)
                {
                    Category.intCategoryID = intCategoryID;
                    Category.strCategory = strCategory;
                    Category.ActionStatus = CCategories.ActionStatusTypes.CategoryAdded;
                }
                else
                {
                    Category.ActionStatus = CCategories.ActionStatusTypes.CategoryAddFailed;
                }

                return Category;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public int DeleteCategory(int intCategoryID)
        {
            try
            {
                CDatabase db = new CDatabase();
                db.DeleteCategory(intCategoryID);
                return 3; // success
            }
            catch (Exception)
            {
                return 4; // Failure
            }
        }

    }

    
}