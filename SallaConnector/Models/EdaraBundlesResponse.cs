using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SallaConnector.Models
{
     

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class BundleDetail
    {
        public int id { get; set; }
        public int bundle_id { get; set; }
        public double quantity { get; set; }
        public double unit_price { get; set; }
        public double price { get; set; }
        public object tax_rate { get; set; }
        public int stock_item_id { get; set; }
        public string stock_item_code { get; set; }
        public string stock_item_description { get; set; }
        public object unit_of_measure_id { get; set; }
        public double dimensions_value { get; set; }
        public string dimensions { get; set; }
        public string comments { get; set; }
    }

    public class EdaraBundle
    {
        public int id { get; set; }
        public string description { get; set; }
        public List<BundleDetail> bundle_details { get; set; }
    }

    public class EdaraBundlesResponse
    {
        public int status_code { get; set; }
        public List<EdaraBundle> result { get; set; }
        public int total_count { get; set; }
    }


}