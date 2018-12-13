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
    public class CAddress
    {
        public int intAddressID { get; set; }

        [DisplayName("Address")]
        public string strAddress { get; set; }

        [DisplayName("City")]
        public string strCity { get; set; }

        [DisplayName("State")]
        public int intStateID { get; set; }

        [DisplayName("Zip Code")]
        public string strZipCode { get; set; }
       

        public static CAddress GetDefaultAddress(int intUserId)
        {
            try
            {
                CDatabase db = new CDatabase();
                CAddress Address = db.GetDefaultAddress(intUserId);
                return Address;

            }
            catch (Exception)
            {
                return null;
            }

        }

        public static List<CAddress> GetAddressList(int intUserId)
        {
            try
            {
                CDatabase db = new CDatabase();
                List<CAddress> Addresses = db.GetAddressList(intUserId);
                return Addresses;

            }
            catch (Exception)
            {
                return null;
            }

        }

        public int AddAddress(CAddress Address)
        {
            try
            {
                CDatabase Database = new CDatabase();
                CCategories Category = new CCategories();
                int intAddressID = Database.InsertAddress(Address);

                return intAddressID;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public int DeleteAddress(int intAddressID,int intUserID = 0)
        {
            try
            {
                CDatabase db = new CDatabase();
                db.DeleteAddress(intAddressID, intUserID);
                return 3; // success
            }
            catch (Exception)
            {
                return 4; // Failure
            }
        }

        public int DoesAddressExist(CAddress NewAddress)
        {
            try
            {
                CDatabase db = new CDatabase();
                List<CAddress> AddressList = db.GetAddressList();

                foreach (CAddress Address in AddressList)
                {
                    if (NewAddress.intStateID == Address.intStateID &&
                        NewAddress.strAddress == Address.strAddress &&
                        NewAddress.strCity == Address.strCity &&
                        NewAddress.strZipCode == Address.strZipCode)
                    {
                        return Address.intAddressID; // match found
                    }

                }
                return 0; // No match found
            }
            catch (Exception)
            {
                return -1; // Failure
            }
        }

    }

    
}