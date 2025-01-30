using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SallaConnector.Models
{


    public class SallaSearchOrderListResult
    {
        public int status { get; set; }
        public bool success { get; set; }
        public List<OrderNunber> data { get; set; }
        public Pagination pagination { get; set; }
    }
    public class OrderNunber
    {
        public string  id { get; set; }
        public string reference_id { get; set; }
    }

    public class SallaSearchOrderDetailsResult2
    {
        public SallaSearchOrderDetailsResult data { get; set; }
        //public bool success { get; set; }
         public  int merchantid { get; set; }
   
        //  public SallaSalesOrderDTO data { get; set; }
    }
    public class SallaSearchOrderDetailsResult
    {
        public int status { get; set; }
        public bool success { get; set; }
       // public  int merchantid { get; set; }
       public SallaSalesOrderDTO data { get; set; }
      //  public SallaSalesOrderDTO data { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Address
    {
        public object country { get; set; }
        public string country_code { get; set; }
        public object city { get; set; }
        public object shipping_address { get; set; }
        public object street_number { get; set; }
        public object block { get; set; }
        public object postal_code { get; set; }
        public GeoCoordinates geo_coordinates { get; set; }
    }

    public class Amount
    {
        public double amount { get; set; }
        public string currency { get; set; }
    }

    public class Amounts
    {
        public SubTotal sub_total { get; set; }
        public ShippingCost shipping_cost { get; set; }
        public CashOnDelivery cash_on_delivery { get; set; }
        public Tax tax { get; set; }
        public List<DiscountDetails> discounts { get; set; }
        public Total total { get; set; }
        public PriceWithoutTax price_without_tax { get; set; }
        public TotalDiscount total_discount { get; set; }
    }

    public class DiscountDetails
    {
        public string title { get; set; }
        public object type { get; set; }
        public string code { get; set; }
        public double discount { get; set; }
        public string currency { get; set; }
        public string discounted_shipping { get; set; }
        public bool hasMarketing { get; set; }
    }
    public class Bank
    {
        public int id { get; set; }
        public string bank_name { get; set; }
        public int bank_id { get; set; }
        public string account_name { get; set; }
        public string account_number { get; set; }
        public string iban_number { get; set; }
        public object iban_certificate { get; set; }
        public object sbc_certificate { get; set; }
        public string certificate_type { get; set; }
        public string status { get; set; }
        public int lean_supported { get; set; }
    }

    public class Birthday
    {
        public string date { get; set; }
        public int timezone_type { get; set; }
        public string timezone { get; set; }
    }

    public class CashOnDelivery
    {
        public double amount { get; set; }
        public string currency { get; set; }
    }

    public class City
    {
        public int id { get; set; }
        public string name { get; set; }
        public string name_en { get; set; }
        public int country_id { get; set; }
    }

    public class Contacts
    {
        public string phone { get; set; }
        public string whatsapp { get; set; }
        public string telephone { get; set; }
    }

    public class Country
    {
        public int id { get; set; }
        public string name { get; set; }
        public string name_en { get; set; }
        public string code { get; set; }
        public string mobile_code { get; set; }
        public object capital { get; set; }
    }

    public class CreatedAt
    {
        public string date { get; set; }
        public int timezone_type { get; set; }
        public string timezone { get; set; }
    }

 

    public class Customized
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class SallaSalesOrderDTO
    {
        public int id { get; set; }
        public object checkout_id { get; set; }
        public int reference_id { get; set; }
        public Urls urls { get; set; }
        public Date date { get; set; }
        //public UpdatedAt updated_at { get; set; }
        public string source { get; set; }
        public bool draft { get; set; }
        public bool read { get; set; }
        //public SourceDetails source_details { get; set; }
        public Status status { get; set; }
        public bool is_price_quote { get; set; }
        public string payment_method { get; set; }
        public string receipt_image { get; set; }
        public string currency { get; set; }
        public Amounts amounts { get; set; }
        //public ExchangeRate exchange_rate { get; set; }
        public bool can_cancel { get; set; }
        //public bool show_weight { get; set; }
       // public bool can_reorder { get; set; }
        public bool is_pending_payment { get; set; }
        public int pending_payment_ends_at { get; set; }
        //public string total_weight { get; set; }
       // public bool has_suspicious_alert { get; set; }
        public Shipping shipping { get; set; }
        public List<Shipment> shipments { get; set; }
        //public PickupBranch pickup_branch { get; set; }
       // public List<ShipmentBranch> shipment_branch { get; set; }
        public SallaCustomerDTO customer { get; set; }
        public List<Item> items { get; set; }
        public Bank bank { get; set; }
       // public List<object> tags { get; set; }
        public Store store { get; set; }
    }

    public class Date
    {
        public string date { get; set; }
        public int timezone_type { get; set; }
        public string timezone { get; set; }
    }

    public class ExchangeRate
    {
        public string base_currency { get; set; }
        public string exchange_currency { get; set; }
        public string rate { get; set; }
    }

    public class Features
    {
        public object availability_notify { get; set; }
        public bool show_rating { get; set; }
    }

    public class GeoCoordinates
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Item
    {
        public int id { get; set; }
        public string name { get; set; }
        public string sku { get; set; }
        public object product_sku_id { get; set; }
        public int quantity { get; set; }
        public string currency { get; set; }
       // public int weight { get; set; }
        public string weight_label { get; set; }
        public string weight_type { get; set; }
        public string product_type { get; set; }
        public string product_thumbnail { get; set; }
        public object mpn { get; set; }
        public object gtin { get; set; }
        public Amounts amounts { get; set; }
        public string notes { get; set; }
        public Product product { get; set; }
       // public List<object> options { get; set; }
        public List<Option> options { get; set; }
        public List<object> images { get; set; }
        public List<object> codes { get; set; }
        public List<object> files { get; set; }
        public object reservations { get; set; }
        public List<object> product_reservations { get; set; }
        public List<ConsistedProduct> consisted_products { get; set; }
    }

    public class Option
    {
        public int id { get; set; }
        public int product_option_id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public Value value { get; set; }
    }
    public class Value
    {
        public int id { get; set; }
        public string name { get; set; }
        public Price price { get; set; }
        public string option_value { get; set; }
    }

    public class ConsistedProduct
    {
        public int id { get; set; }
        public string type { get; set; }
        public Promotion promotion { get; set; }
        public int quantity { get; set; }
        public string status { get; set; }
        public bool is_available { get; set; }
        public string sku { get; set; }
        public string name { get; set; }
        public Price price { get; set; }
        public SalePrice sale_price { get; set; }
        public string currency { get; set; }
        public string url { get; set; }
        public string thumbnail { get; set; }
        public bool has_special_price { get; set; }
        public RegularPrice regular_price { get; set; }
        public object calories { get; set; }
        public string mpn { get; set; }
        public string gtin { get; set; }
        public string description { get; set; }
        public object favorite { get; set; }
        public Features features { get; set; }
        public int quantity_in_group { get; set; }
    }
    public class Location
    {
        public string lat { get; set; }
        public string lng { get; set; }
    }

    public class Name
    {
        public string ar { get; set; }
        public object en { get; set; }
    }

    public class Package
    {
        public int item_id { get; set; }
        public object external_id { get; set; }
        public string name { get; set; }
        public string sku { get; set; }
        public Price price { get; set; }
        public int quantity { get; set; }
        public Weight weight { get; set; }
    }

    public class PickupAddress
    {
        public string country { get; set; }
        public string country_code { get; set; }
        public string city { get; set; }
        public string shipping_address { get; set; }
        public object street_number { get; set; }
        public object block { get; set; }
        public object postal_code { get; set; }
        public GeoCoordinates geo_coordinates { get; set; }
    }

    public class PickupBranch
    {
        public int id { get; set; }
        public string name { get; set; }
        public string status { get; set; }
        public Location location { get; set; }
        public object street { get; set; }
        public object address_description { get; set; }
        public object additional_number { get; set; }
        public object building_number { get; set; }
        public object local { get; set; }
        public object postal_code { get; set; }
        public Contacts contacts { get; set; }
        public object preparation_time { get; set; }
        public bool is_open { get; set; }
        public object closest_time { get; set; }
        public List<object> working_hours { get; set; }
        public bool is_cod_available { get; set; }
        public bool is_default { get; set; }
        public string type { get; set; }
        public string cod_cost { get; set; }
        public Country country { get; set; }
        public City city { get; set; }
    }

    public class Price
    {
        public double amount { get; set; }
        public string currency { get; set; }
    }

    public class PriceWithoutTax
    {
        public double amount { get; set; }
        public string currency { get; set; }
    }

    public class Product
    {
        public int id { get; set; }
        public string type { get; set; }
        public Promotion promotion { get; set; }
        public object quantity { get; set; }
        public string status { get; set; }
        public bool is_available { get; set; }
        public string sku { get; set; }
        public string name { get; set; }
        public Price price { get; set; }
        public SalePrice sale_price { get; set; }
        public string currency { get; set; }
        public string url { get; set; }
        public string thumbnail { get; set; }
        public bool has_special_price { get; set; }
        public RegularPrice regular_price { get; set; }
        public object calories { get; set; }
        public object mpn { get; set; }
        public object gtin { get; set; }
        public string description { get; set; }
        public object favorite { get; set; }
        public Features features { get; set; }
    }

    public class Promotion
    {
        public object title { get; set; }
        public object sub_title { get; set; }
    }

    public class Receiver
    {
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
    }

    public class RegularPrice
    {
        public double amount { get; set; }
        public string currency { get; set; }
    }

    public class Root
    {
        public string @event { get; set; }
        public int merchant { get; set; }
        public string created_at { get; set; }
        public SallaSalesOrderDTO data { get; set; }
    }

    public class SalePrice
    {
        public double amount { get; set; }
        public string currency { get; set; }
    }

    public class ShipFrom
    {
        public string type { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string country { get; set; }
        public int country_id { get; set; }
        public string country_code { get; set; }
        public string city { get; set; }
        public int city_id { get; set; }
        public string address_line { get; set; }
        public object street_number { get; set; }
        public object block { get; set; }
        public object postal_code { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public int branch_id { get; set; }
    }


    public class Shipment
    {
        public string id { get; set; }
        public object pickup_id { get; set; }
        public string tracking_link { get; set; }
        public object label { get; set; }
    }

    public class Shipment2
    {
        public int id { get; set; }
        public CreatedAt created_at { get; set; }
        public string type { get; set; }
        public ShipFrom ship_from { get; set; }
        public TotalWeight total_weight { get; set; }
        public List<Package> packages { get; set; }
        public List<object> meta { get; set; }
    }

    public class ShipmentBranch
    {
        public int id { get; set; }
        public string name { get; set; }
        public string status { get; set; }
        public bool is_default { get; set; }
        public Type type { get; set; }
    }

    public class Shipper
    {
        public string name { get; set; }
        public string company_name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
    }

    public class Shipping
    {
        public object id { get; set; }
        public object app_id { get; set; }
        public int shipping_details_id { get; set; }
        public string company { get; set; }
        public string logo { get; set; }
        public Receiver receiver { get; set; }
        public Shipper shipper { get; set; }
        public PickupAddress pickup_address { get; set; }
        public Address address { get; set; }
       // public Shipment shipment { get; set; }
        //public List<object> policy_options { get; set; }
        //public int shipment_reference { get; set; }
        //public int branch_id { get; set; }
    }


    public class ShippingCost
    {
        public double amount { get; set; }
        public string currency { get; set; }
    }

    public class SourceDetails
    {
        public string type { get; set; }
        public object value { get; set; }
        public string device { get; set; }

        [JsonProperty("user-agent")]
        public string useragent { get; set; }
        public object ip { get; set; }
    }

    public class Status
    {
        public int id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public Customized customized { get; set; }
    }

    public class Store
    {
        public int id { get; set; }
        public int store_id { get; set; }
        public int user_id { get; set; }
        public string user_email { get; set; }
        public string username { get; set; }
        public Name name { get; set; }
        public string avatar { get; set; }
    }

    public class SubTotal
    {
        public double amount { get; set; }
        public string currency { get; set; }
    }

    public class Tax
    {
        public string percent { get; set; }
        public Amount amount { get; set; }
    }

    public class Total
    {
        public double amount { get; set; }
        public string currency { get; set; }
    }

    public class TotalDiscount
    {
        public double amount { get; set; }
        public string currency { get; set; }
    }

    public class TotalWeight
    {
        public int value { get; set; }
        public string units { get; set; }
    }

    public class Type
    {
    }

    public class UpdatedAt
    {
        public string date { get; set; }
        public int timezone_type { get; set; }
        public string timezone { get; set; }
    }
 

    public class Weight
    {
        public double value { get; set; }
        public string units { get; set; }
    }


}