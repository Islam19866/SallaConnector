using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SallaConnector.Models
{
   

    public class SallaPriceListRequest
    {
        
        public string sku { get; set; }

        public SallaPrice sallaPrice { set; get; }
    }

    public class SallaPrice
    {
        public string price { get; set; }
        //public string sale_price { get; set; }
    }

   
}