using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SallaConnector.Models
{
    public class CustomerDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string relatedaccountcode { get; set; }
        public string phone { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string payment_type { get; set; }
        public int credit_limit { get; set; }
        public int balance { get; set; }
        public string shipping_term { get; set; }
        public string insurance { get; set; }
        public bool has_discount { get; set; }
        public int discount_from { get; set; }
        public int discount_to { get; set; }
        public string customer_type { get; set; }
        public string pricing_type { get; set; }
        public int payment_max_due_days { get; set; }
        public int related_account_id { get; set; }
        public int related_account_parent_id { get; set; }
        public string tax_registeration_id { get; set; }
        public string external_id { get; set; }
        public List<CustomerAddress> customer_addresses { get; set; }
    }

    public class CustomerAddress
    {
        public int id { get; set; }
        public int customer_id { get; set; }
        public string name { get; set; }
        public int country_id { get; set; }
        public int city_id { get; set; }
        public int? district_id { get; set; }
        public string street { get; set; }
        public string description { get; set; }
        public bool is_default { get; set; }
        public string entity_state { get; set; }
        public string address_phone { get; set; }
    }

    public class CustomeResponseDTO
    {
        public int status_code { get; set; }
        public CustomerDTO result { get; set; }
    }


}