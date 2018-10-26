using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Momberg_SET253_Final.Models
{
	public class CProduct
	{
		public int ProductID { get; set; }
		public CUser SellerID { get; set; }
		public CUser BuyerID { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public double Price { get; set; }
		public bool Picture { get; set; }
		public double Milage { get; set; }
		public string City { get; set; }
		public int StateID { get; set; }
		public StatusTypes StatusTypeID { get; set; }
		public ProductTypes ProductTypeID { get; set; }

	}

	public enum StatusTypes
	{
		NoType = 0,
		Active = 1,
		Sold = 2,
		Closed = 3,
	}

	public enum ProductTypes
	{
		NoType = 0,
		Automobile = 1,
		Boat = 2,
		Motorcycle = 3,
		Chicken = 4,
	}
}

