using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SallaConnector.Models
{

    public class SalesOrderDTO
    {
        //public string order_status { get; set; }
        public int id { get; set; }
        //public string document_code { get; set; }
        public string document_type { get; set; }
        public string paper_number { get; set; }
        //public string run_sheet_id { get; set; }
        public int customer_id { get; set; }
        //public int salesPerson_id { get; set; }
        public int? warehouse_id { get; set; }
        public int salesstore_id { get; set; }
        //public int shippment_cost { get; set; }
        public DateTime document_date { get; set; }
        //public DateTime shipping_date { get; set; }
        //public int gross_total { get; set; }
        //public int sub_total { get; set; }
        //public int net_total { get; set; }
        //public double total_item_discounts { get; set; }
        public double discount { get; set; }
        //public int discount_rate { get; set; }
        public bool taxable { get; set; }
       // public bool apply_tax_after_discount { get; set; }
        //public int tax { get; set; }
        public double cash_amount { get; set; }
        public int currency_id { get; set; }
        //public int exchange_rate { get; set; }
        public string channel { get; set; }
        //public string notes { get; set; }
        public string external_id { get; set; }
        //public string related_workorder_code { get; set; }
        //public int related_workorder_value { get; set; }
        //public List<string> related_salesreturn_codes { get; set; }
        //public List<string> related_salesorder_codes { get; set; }
        //public PaymentInformation payment_information { get; set; }
        public List<SalesOrderDetail> salesOrder_details { get; set; }
        //public List<SalesOrderInstallment> salesOrder_installments { get; set; }
        //public int insert_user_id { get; set; }
        //public DateTime insert_date { get; set; }
        //public int update_user_id { get; set; }
       // public DateTime update_date { get; set; }
        //public int address_id { get; set; }
       // public string payment_status { get; set; }
        //public int other_credit_amount { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class PaymentInformation
    {
        public int property1 { get; set; }
        public int property2 { get; set; }
    }


    public class SalesOrderDetail
    {
        //public int id { get; set; }
        //public int document_id { get; set; }
        public double quantity { get; set; }
        //public int Issuedquantity { get; set; }
        //public int unit_price { get; set; }
        public double price { get; set; }
        public int? tax_id { get; set; }
        public double item_discount { get; set; }
        public int item_discount_type { get; set; }
        public int warehouse_id { get; set; }
        public int? bundle_id { get; set; }
        public int? service_item_id { get; set; }
        public int? stock_item_id { get; set; }
        //public string stock_item_code { get; set; }
        public string stock_item_description { get; set; }
        public string comments { get; set; }
        //public int returned_quantity { get; set; }
        //public int insert_user_id { get; set; }
        //public DateTime insert_date { get; set; }
        //public int update_user_id { get; set; }
        //public DateTime update_date { get; set; }
        public int? bundle_quantity { get; set; }
    }
 

    public class SalesOrderInstallment
    {
        public int id { get; set; }
        public int document_id { get; set; }
        public int days_limit { get; set; }
        public int amount { get; set; }
        public DateTime due_date { get; set; }
        public int account_id { get; set; }
        public string payment_type { get; set; }
    }

    public class PaymentDTO
    {
        public double paid_amount { get; set; }
        public string related_sales_order_code { get; set; }
        public string cash_account_id { get; set; }
        public string notes { get; set; }
    }
}