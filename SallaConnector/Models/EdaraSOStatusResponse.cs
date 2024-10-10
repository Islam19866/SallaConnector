using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SallaConnector.Models
{
    public class EdaraSOStatusResponse
    {
 
            public string order_status { get; set; }
            public int id { get; set; }
            public string document_code { get; set; }
            public string paper_number { get; set; }
            public int customer_id { get; set; }
            public int warehouse_id { get; set; }
            public double shippment_cost { get; set; }
            public DateTime document_date { get; set; }
            public double gross_total { get; set; }
            public double sub_total { get; set; }
            public double net_total { get; set; }
            public double total_item_discounts { get; set; }
            public double discount { get; set; }
            public double discount_rate { get; set; }
            public bool taxable { get; set; }
            public bool apply_tax_after_discount { get; set; }
            public double tax { get; set; }
            public double cash_amount { get; set; }
            public double onAccount_ammount { get; set; }
            public double cash_paid { get; set; }
            public string payment_status { get; set; }
            public int currency_id { get; set; }
            public double exchange_rate { get; set; }
            public string channel { get; set; }
            public string external_id { get; set; }
            public double related_workorder_value { get; set; }
            public List<object> related_salesreturn_codes { get; set; }
            public List<object> related_salesorder_codes { get; set; }
            public double other_credit_amount { get; set; }
        
    }
}