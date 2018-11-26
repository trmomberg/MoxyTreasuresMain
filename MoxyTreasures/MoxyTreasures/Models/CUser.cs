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
    public class CUser
    {

        public CUser()
        {
            this.GenderList = new List<SelectListItem>();
            this.GenderList.Add(new SelectListItem() { Value = "0", Text = "Male " });
            this.GenderList.Add(new SelectListItem() { Value = "1", Text = "Female" });
            this.GenderList.Add(new SelectListItem() { Value = "2", Text = "Other" });

            this.StatesList = new List<SelectListItem>();
            this.StatesList.Add(new SelectListItem() { Value = "1", Text = "Alabama"});
            this.StatesList.Add(new SelectListItem() { Value = "2", Text = "Alaska"});
            this.StatesList.Add(new SelectListItem() { Value = "3", Text = "Arizona"});
            this.StatesList.Add(new SelectListItem() { Value = "4", Text = "Arkansas"});
            this.StatesList.Add(new SelectListItem() { Value = "5", Text = "California"});
            this.StatesList.Add(new SelectListItem() { Value = "6", Text = "Colorado"});
            this.StatesList.Add(new SelectListItem() { Value = "7", Text = "Connecticut"});
            this.StatesList.Add(new SelectListItem() { Value = "8", Text = "Delaware"});
            this.StatesList.Add(new SelectListItem() { Value = "9", Text = "Florida"});
            this.StatesList.Add(new SelectListItem() { Value = "10", Text = "Georgia"});
            this.StatesList.Add(new SelectListItem() { Value = "11", Text = "Hawaii"});
            this.StatesList.Add(new SelectListItem() { Value = "12", Text = "Idaho"});
            this.StatesList.Add(new SelectListItem() { Value = "13", Text = "Illinois"});
            this.StatesList.Add(new SelectListItem() { Value = "14", Text = "Indiana"});
            this.StatesList.Add(new SelectListItem() { Value = "15", Text = "Iowa"});
            this.StatesList.Add(new SelectListItem() { Value = "16", Text = "Kansas"});
            this.StatesList.Add(new SelectListItem() { Value = "17", Text = "Kentucky" });
            this.StatesList.Add(new SelectListItem() { Value = "18", Text = "Louisiana"});
            this.StatesList.Add(new SelectListItem() { Value = "19", Text = "Maine"});
            this.StatesList.Add(new SelectListItem() { Value = "20", Text = "Maryland"});
            this.StatesList.Add(new SelectListItem() { Value = "21", Text = "Massachusetts"});
            this.StatesList.Add(new SelectListItem() { Value = "22", Text = "Michigan"});
            this.StatesList.Add(new SelectListItem() { Value = "23", Text = "Minnesota"});
            this.StatesList.Add(new SelectListItem() { Value = "24", Text = "Mississippi"});
            this.StatesList.Add(new SelectListItem() { Value = "25", Text = "Missouri"});
            this.StatesList.Add(new SelectListItem() { Value = "26", Text = "Montana"});
            this.StatesList.Add(new SelectListItem() { Value = "27", Text = "Nebraska"});
            this.StatesList.Add(new SelectListItem() { Value = "28", Text = "Nevada"});
            this.StatesList.Add(new SelectListItem() { Value = "29", Text = "New Hampshire"});
            this.StatesList.Add(new SelectListItem() { Value = "30", Text = "New Jersey"});
            this.StatesList.Add(new SelectListItem() { Value = "31", Text = "New Mexico"});
            this.StatesList.Add(new SelectListItem() { Value = "32", Text = "New York"});
            this.StatesList.Add(new SelectListItem() { Value = "33", Text = "North Carolina"});
            this.StatesList.Add(new SelectListItem() { Value = "34", Text = "North Dakota"});
            this.StatesList.Add(new SelectListItem() { Value = "35", Text = "Ohio"});
            this.StatesList.Add(new SelectListItem() { Value = "36", Text = "Oklahoma"});
            this.StatesList.Add(new SelectListItem() { Value = "37", Text = "Oregon"});
            this.StatesList.Add(new SelectListItem() { Value = "38", Text = "Pennsylvania"});
            this.StatesList.Add(new SelectListItem() { Value = "39", Text = "Rhode Island"});
            this.StatesList.Add(new SelectListItem() { Value = "40", Text = "South Carolina"});
            this.StatesList.Add(new SelectListItem() { Value = "41", Text = "South Dakota"});
            this.StatesList.Add(new SelectListItem() { Value = "42", Text = "Tennessee"});
            this.StatesList.Add(new SelectListItem() { Value = "43", Text = "Texas"});
            this.StatesList.Add(new SelectListItem() { Value = "44", Text = "Utah"});
            this.StatesList.Add(new SelectListItem() { Value = "45", Text = "Vermont"});
            this.StatesList.Add(new SelectListItem() { Value = "46", Text = "Virginia"});
            this.StatesList.Add(new SelectListItem() { Value = "47", Text = "Washington"});
            this.StatesList.Add(new SelectListItem() { Value = "48", Text = "West Virginia"});
            this.StatesList.Add(new SelectListItem() { Value = "49", Text = "Wisconsin"});
            this.StatesList.Add(new SelectListItem() { Value = "50", Text = "Wyoming"});
           
        }

        public int    UserID        { get; set; }

		[DisplayName("First Name")]
        [Required]
        public string FirstName     { get; set; }

		[DisplayName("Last Name")]
        [Required]
        public string LastName      { get; set; }

		[DisplayName("Email Address")]
        [Required]
		public string EmailAddress  { get; set; }
        [Required]
        public string Password{ get; set; }
		public ActionStatusTypes ActionStatus { get; set; }
		public List<CProduct> ProductList = new List<CProduct>();
		public List<CProduct> Cart = new List<CProduct>();

        [DisplayName("Sub Total:")]
        public double CartSubTotal { get; set; }
        
        [DisplayName("Address")]
        public string strAddress { get; set; }

        [DisplayName("City")]
        public string strCity { get; set; }

        [DisplayName("State")]
        public int intStateID { get; set; }

        [DisplayName("Birth Date")]
        public DateTime Birthdate { get; set; }

        [DisplayName("Gender")]
        public int intGenderID { get; set; }

        [DisplayName("Zip Code")]
        public string strZipCode { get; set; }

        [DisplayName("Date Of Birth")]
        public DateTime? dtmDateOfBirth { get; set; }

        public bool blnAdmin { get; set; }

        [DisplayName("Email Signup")]
        public bool blnEmailList { get; set; }
        public List<SelectListItem> GenderList;
        public List<SelectListItem> StatesList;

        public enum ActionStatusTypes
		{
			Unknown				= 0,
			UserUpdated			= 1,
			UserAlreadyExists	= 2,
			UserLoggedIn		= 3,
			FailedLogin			= 4
        }

		public CUser Login(string strUserName, string strPassword)
		{
            try
			{
				CDatabase Database = new CDatabase();
				CUser User = new CUser();

				int intUserID = Database.Login(strUserName, strPassword);

				if(intUserID > 0)
				{
					// Login successful
					User = GetUser(intUserID);
					User.SetCurrentUser();
					User.ActionStatus = ActionStatusTypes.UserLoggedIn;
					                 
                }
                else
                {
					// Login failed
					User.ActionStatus = ActionStatusTypes.FailedLogin;
				}

				return User;
			}
			catch (Exception)
			{
				return null;
			}
		}

		public CUser.ActionStatusTypes Save()
		{
			try
			{
				CDatabase Database = new CDatabase();
				if (this.UserID == 0)
				{
					this.ActionStatus = Database.InsertUser(this);
				}
				else
				{
					this.ActionStatus = Database.UpdateUser(this);
				}
				return ActionStatus;
			}
			catch (Exception)
			{
				return ActionStatusTypes.Unknown;
			}
		}

		// Set current user
		public bool SetCurrentUser()
		{
			try
			{
				HttpContext.Current.Session["CurrentUser"] = this;
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		// Get current user
		public CUser GetCurrentUser()
		{
			try
			{
				CUser User;
				User = (CUser)HttpContext.Current.Session["CurrentUser"];
				ActionStatus = ActionStatusTypes.Unknown; // Clear the action status before saving
				return User;
			}
			catch (Exception)
			{
				return null;
			}
		}

		// Clear current user
		public bool ClearCurrentUser()
		{
			try
			{
				HttpContext.Current.Session["CurrentUser"] = null;
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public static CUser GetUser(int intUserID)
		{
			try
			{
				CUser User = new CUser();
				CDatabase db = new CDatabase();

				User = db.GetUser(intUserID);

				return User;
			}
			catch (Exception)
			{
				return null;
			}
		}
    }
}