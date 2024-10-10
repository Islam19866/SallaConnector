using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SallaConnector.Models
{
    

    public class CustomerAddressResponse
    {
        public int id { get; set; }
        public int customer_id { get; set; }
        public string name { get; set; }
        public int country_id { get; set; }
        public int city_id { get; set; }
        public int district_id { get; set; }
        public string street { get; set; }
        public string description { get; set; }
        public bool is_default { get; set; }
        public string address_phone { get; set; }
    }

    public class CustomerAddressListResponse
    {
        public int status_code { get; set; }
        public List<CustomerAddressResponse> result { get; set; }
    }
}