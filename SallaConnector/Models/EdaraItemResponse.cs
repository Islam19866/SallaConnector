using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SallaConnector.Models
{
    public class EdaraItemResponse
    {

            public int id { get; set; }
            public string description { get; set; }
            public string other_lang_description { get; set; }
            public string code { get; set; }
            public string sku { get; set; }
            public double price { get; set; }
            public double dealer_price { get; set; }
            public double supper_dealer_price { get; set; }
            public double purchase_price { get; set; }
            public double fifo_cost { get; set; }
            public double lifo_cost { get; set; }
            public double average_cost { get; set; }
            public double last_cost { get; set; }
            public string part_number { get; set; }
            public double tax_rate { get; set; }
            public int classification_id { get; set; }
            public int brand_id { get; set; }
            public double warranty { get; set; }
            public string external_id { get; set; }
            public List<object> unit_of_measures { get; set; }
            public double sales_price_discount { get; set; }
            public int sales_price_discount_type { get; set; }
            public bool sales_price_discount_is_limited { get; set; }
            public double dealer_price_discount { get; set; }
            public int dealer_price_discount_type { get; set; }
            public bool dealer_price_discount_is_limited { get; set; }
            public double super_dealer_price_discount { get; set; }
            public int super_dealer_price_discount_type { get; set; }
            public bool super_dealer_price_discount_is_limited { get; set; }
            public double weight { get; set; }
            public string data_sheet { get; set; }
            public string note { get; set; }
            public List<object> dynamic_properties_info { get; set; }
            public bool is_grouping_item { get; set; }
            public List<object> child_items { get; set; }
        
    }
}