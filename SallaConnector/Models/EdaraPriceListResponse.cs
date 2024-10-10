using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SallaConnector.Models
{
    public class EdaraPriceListResponse
    {
    
        public int priceList_id { get; set; }
        public List<PriceListDetail> priceList_details { get; set; }
    }

    
    public class PriceListDetail
    {
        public int pricelist_id { get; set; }
        public int stock_item_id { get; set; }
        public string sku { get; set; }
        public string stock_item_description { get; set; }
        public string item_code { get; set; }
        public string part_number { get; set; }
        public double pricelist_price { get; set; }
        public string pricelist_discount_type { get; set; }
        public double pricelist_discount { get; set; }
    }

   


}