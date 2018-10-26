using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace Momberg_SET253_Final.Models
{
	public class CHome
	{
		public CUser CurrentUser = new CUser();
		public List<CProduct> Products = new List<CProduct>();      

        public int ToggleCart(int intProductID, int intUserID)
        {
            try
            {
                CDatabase db = new CDatabase();
                if (db.ToggleCart(intUserID, intProductID) == 1)
                {
                    return 1; // Toggled on         
                }
                else if (db.ToggleCart(intUserID, intProductID) == -1)
                {
                    return -1; // Toggled off
                }
                else
                {
                    return 0; // Failed
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }

    
}