using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Momberg_SET253_Final.Models
{
    public class CFilter
    {
        public ProductTypes intCategoryID { get; set; }
        public int intPriceLow            { get; set; }
        public int intPriceHigh           { get; set; }
		
    }
}