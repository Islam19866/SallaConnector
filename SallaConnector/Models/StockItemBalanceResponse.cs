using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SallaConnector.Models
{

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class StockItemDetails
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
        public double warranty { get; set; }
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

    public class StockDetailsResponse
    {
        public double GlobalBalance { get; set; }
        public double GlobalReservedBalance { get; set; }
        public List<StockItemBalance> StockItemBalances { get; set; }
        public int WarehouseId { get; set; }
        public double LocalBalance { get; set; }
        public double LocalReservedBalance { get; set; }
        public double StockItemBalanceIncreased { get; set; }
        public double TotalBalance { get; set; }
        public double TotalReservedBalance { get; set; }
    }

    public class StockItemBalanceRoot
    {
        public string entity_id { get; set; }
        public string entity_code { get; set; }
        public string entity_type { get; set; }
        public string event_type { get; set; }
        public string tenant_id { get; set; }
        public string tenant_name { get; set; }
        public StockItemDetails data { get; set; }
        public StockDetailsResponse message_attributes { get; set; }
    }

    public class StockItemBalance
    {
        public int StockItemId { get; set; }
        public int WarehouseId { get; set; }
        public double OnHandBalance { get; set; }
        public double Reserved_Balance { get; set; }
    }







}