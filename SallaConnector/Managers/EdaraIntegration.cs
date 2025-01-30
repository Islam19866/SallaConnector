using Newtonsoft.Json;
using RestSharp;
using SallaConnector.Context;
using SallaConnector.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SallaConnector.Managers
{
    public class EdaraIntegration
    {
       
        public static IRestResponse PostPayment(PaymentDTO model, SallaAccount sallaEdaraAccount)

        {
           
            APIManger<PaymentDTO> man = new APIManger<PaymentDTO>(sallaEdaraAccount);
            string body = JsonConvert.SerializeObject(model);
            IRestResponse result = man.SendRequest("salesOrders/cash-in", Method.POST, body);

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {

                return result;
            }

            return result;
        }

        public static IRestResponse PostSalesOrder(SalesOrderDTO model ,SallaAccount sallaEdaraAccount)

        {
            APIManger<SalesOrderDTO> man = new APIManger<SalesOrderDTO>(sallaEdaraAccount);
            string body = JsonConvert.SerializeObject(model);
            IRestResponse result = man.SendRequest("Salesorders", Method.POST, body);

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {

                return result;
            }

            return result;
        }
        public static IRestResponse UpdateSalesOrder(SalesOrderDTO model, SallaAccount sallaEdaraAccount , string SO_Code)

        {
            APIManger<SalesOrderDTO> man = new APIManger<SalesOrderDTO>(sallaEdaraAccount);
            string body = JsonConvert.SerializeObject(model);
            IRestResponse result = man.SendRequest("Salesorders/UpdateByCode/"+SO_Code, Method.PUT, body);

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {



                return result;
            }

            return result;
        }

        public static IRestResponse PostCustomer(CustomerDTO model , SallaAccount sallaEdaraAccount)

        {
            APIManger<CustomerDTO> man = new APIManger<CustomerDTO>(sallaEdaraAccount);
            string body = JsonConvert.SerializeObject(model);
            IRestResponse result = man.SendRequest("Customers", Method.POST, body);

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return result;
            }

            return null;
        }

        public static IRestResponse updateCustomer(CustomerDTO model, SallaAccount sallaEdaraAccount)

        {
            APIManger<CustomerDTO> man = new APIManger<CustomerDTO>(sallaEdaraAccount);
            string body = JsonConvert.SerializeObject(model);
            IRestResponse result = man.SendRequest("Customers", Method.PUT, body);

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return result;
            }
            else
            {
                return result;
            }

           
        }

        public static int GetServiceItemId(string serviceName, SallaAccount sallaEdaraAccount)

        {
            APIManger<CommonResponse> man = new APIManger<CommonResponse>(sallaEdaraAccount); 
            var item = man.Get("serviceItems/FindByName/"+ serviceName, null, null, null);
            if (item.result == null)
                throw new Exception("ServiceName not exist SKU " + serviceName);

            return item.result.id;


        }

        public static bool CheckEdaraUser(string userName, SallaAccount sallaEdaraAccount)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("username", userName);
            APIManger<EdaraIntegrationUserResponse> man = new APIManger<EdaraIntegrationUserResponse>(sallaEdaraAccount);
            var item = man.Get("integrations/users" ,null, param, null);
            if (item == null)
                throw new Exception("userName not exist userName " + userName);

            return true;


        }

 

        public static int GetWarehouseIdByName(string warehouseName, SallaAccount sallaEdaraAccount)
        {
            APIManger<CommonResponse> man = new APIManger<CommonResponse>(sallaEdaraAccount);
            var item = man.Get("warehouses/FindByName/"+ warehouseName, null, null, null);
            if (item.result == null)
                throw new Exception("warehouseName not exist warehouse Name " + warehouseName);

            return item.result.id;

        }
        public static int GetStoredByName(string storename, SallaAccount sallaEdaraAccount)
        {
            APIManger<EdaraSalesStoresResponse> man = new APIManger<EdaraSalesStoresResponse>(sallaEdaraAccount);
            var item = man.Get("salesStores?offset=1&limit=10000" , null, null, null);
            var store = item.result.Where(s => s.description == storename).FirstOrDefault();
            if (store == null)
                throw new Exception("storename not exist store Name " + storename);

            return store.id;

        }
        public static List<BundleDetail> GetBundleByName(string name, SallaAccount sallaEdaraAccount)
        {
            APIManger<EdaraBundlesResponse> man = new APIManger<EdaraBundlesResponse>(sallaEdaraAccount);
            var item = man.Get("salesBundles?offset=1&limit=10000", null, null, null);
            var result = item.result.Where(s => s.description == name).FirstOrDefault();
            if (result == null)
                throw new Exception("bundle not exist bundle Name " + name);

            return result.bundle_details;

        }


        public static int GetStockItemId(string SKU, SallaAccount sallaEdaraAccount)

        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("SKU", SKU);

            APIManger<EdaraItemDTO> man = new APIManger<EdaraItemDTO>( sallaEdaraAccount);
            var item = man.Get("StockItems/Find",null, parameters, null);
            if (item.result == null)
                throw new Exception("Item not exist SKU " + SKU);

            return item.result.id;

           
        }
        public static int GetTaxId(string rate , SallaAccount sallaEdaraAccount)

        {
            if (sallaEdaraAccount.TaxRateId.HasValue)
                return sallaEdaraAccount.TaxRateId.Value;
            else
            {

                APIManger<EdaraTaxResponse> man = new APIManger<EdaraTaxResponse>(sallaEdaraAccount);
                var item = man.Get("Taxes/FindByRate/" + rate, null, null, null);
                if (item.result == null)
                    throw new Exception("Taxes ByRate " + rate + " ot exist  ");

                //update TaxId
                ConfigManager.updateTaxId(sallaEdaraAccount.SallaMerchantId, item.result.FirstOrDefault().id);
                return item.result.FirstOrDefault().id;
            }

        }

        

        public static int GetCustomerId(string Code , SallaAccount sallaEdaraAccount)

        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("Code", Code);

            APIManger<CustomeResponseDTO> man = new APIManger<CustomeResponseDTO>(sallaEdaraAccount);
            var customer = man.Get("Customers/Find", null, parameters, null);
            if (customer.result == null)
                return 0;
            return customer.result.id;


        }

        public static int GetCurrencyId(string Code, SallaAccount sallaEdaraAccount)

        {
 
            APIManger<CommonResponse> man = new APIManger<CommonResponse>(sallaEdaraAccount);
            var response = man.Get("currencies/FindByCode/"+ Code, null, null, null);
            if (response.result == null)
                throw new Exception("currency not exist Code " + Code);
            return response.result.id;


        }
        public static CustomerAddressListResponse GetCustomerAddress(string customerId, SallaAccount sallaEdaraAccount)
        {
            APIManger<CustomerAddressListResponse> man = new APIManger<CustomerAddressListResponse>(sallaEdaraAccount);
            var result = man.Get("customers/addresses/" + customerId, null, null, null);

            return result;

        }

        public static CityResponse GetCityByName(string Name, SallaAccount sallaEdaraAccount)

        {
            APIManger<CityResponse> man = new APIManger<CityResponse>(sallaEdaraAccount);
            var city = man.Get("cities/FindByName/"+Name, null, null, null);
           
            return city;
            
        }
        public static DistrictResponse GetDistrictName(string Name, SallaAccount sallaEdaraAccount)

        {
            APIManger<DistrictResponse> man = new APIManger<DistrictResponse>(sallaEdaraAccount);
            var district = man.Get("Districts/FindByName/" + Name, null, null, null);

            return district;

        }

        public static IRestResponse CreateUser(EdaraUserCreateRequest user , SallaAccount sallaEdaraAccount)

        {
            APIManger<Dictionary<string, object>> man = new APIManger<Dictionary<string, object>>(sallaEdaraAccount);
            string body = JsonConvert.SerializeObject(user);
            IRestResponse result = man.SendRequest("integrations", Method.POST, body);

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return result;
            }

            return null;
        }

        public static IRestResponse CreateCity(EdaraCityCreateRequest city, SallaAccount sallaEdaraAccount)

        {
            APIManger<Dictionary<string, object>> man = new APIManger<Dictionary<string, object>>(sallaEdaraAccount);
            string body = JsonConvert.SerializeObject(city);
            IRestResponse result = man.SendRequest("cities", Method.POST, body);

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {

                return result;
            }
            else
            {

                return result;
            }

            
        }

        public static IRestResponse CreateDistrict(EdaraDistrictCreateRequest district, SallaAccount sallaEdaraAccount)

        {
            APIManger<Dictionary<string, object>> man = new APIManger<Dictionary<string, object>>(sallaEdaraAccount);
            string body = JsonConvert.SerializeObject(district);
            IRestResponse result = man.SendRequest("districts", Method.POST, body);

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return result;
            }
            else
            {

                return result;
            }


        }

    }
}