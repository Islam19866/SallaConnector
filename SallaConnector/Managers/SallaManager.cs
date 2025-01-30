using Newtonsoft.Json;
using RestSharp;
using SallaConnector.Context;
using SallaConnector.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace SallaConnector.Managers
{
    public class SallaManager
    {


        public static void GetOAuthToken()
        {
            var clientId = "29c21598-0061-4409-9689-91a400738062";
            var clientSecret = "9d38baceb6ae1f8de2f0e1d0fee6f2ad";
            var tokenEndpoint = "https://accounts.salla.sa/oauth2/token";
            var authorizationEndpoint = "https://accounts.salla.sa/oauth2/auth";
            var redirectUri = "your-redirect-uri"; // Replace with your registered redirect URI
            var scope = "offline_access";
            var state = "random-string-to-prevent-csrf"; // Generate a random state for security
            var authorizationUrl = $"{authorizationEndpoint}?response_type=code&client_id={clientId}&redirect_uri={Uri.EscapeDataString(redirectUri)}&scope={scope}&state={state}";

            //using (var httpClient = new HttpClient())
            //{
            //    var parameters = new Dictionary<string, string>()
            //    {
            //        { "grant_type", "client_credentials" },
            //        { "client_id", clientId },
            //        { "client_secret", clientSecret },
            //        { "scope", scope }
            //    };

            //    var encodedContent = new FormUrlEncodedContent(parameters);

            //    var response = await httpClient.PostAsync(tokenEndpoint, encodedContent);

            //    if (response.IsSuccessStatusCode)
            //    {
            //        var json = await response.Content.ReadAsStringAsync();
            //        var tokenResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<TokenResponse>(json);

            //        // Process the token (e.g., log it, store it, or use it in subsequent requests)
            //        Console.WriteLine($"Access Token: {tokenResponse.AccessToken}");
            //    }
            //    else
            //    {
            //        Console.WriteLine($"Error getting token: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
            //    }
            //}
        }
        

        public class TokenResponse
        {
            [Newtonsoft.Json.JsonProperty("access_token")]
            public string AccessToken { get; set; }
        }


        public static IRestResponse UpdateSalesOrderStatus(SallaUpdateStatusRequest sallaUpdateStatusRequest , int SallaOrderId , string tenantName , string merchantId)

        {
            var stores = ConfigManager.getActiveStores(tenantName).ToList().Where(s=>s.SallaMerchantId==merchantId);
            foreach (var store in stores)
            {
                SallaAPIManger<SallaUpdateStatusRequest> man = new SallaAPIManger<SallaUpdateStatusRequest>();
                string body = JsonConvert.SerializeObject(sallaUpdateStatusRequest);
                
                IRestResponse result = man.SendRequest("orders/"+SallaOrderId.ToString()+"/status", Method.POST, body , store.SallaToken);
              
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return result;
                }
                else
                {
                    LogManager.LogMessage("SO update status failed for SO id " + SallaOrderId.ToString() + " status id =" + sallaUpdateStatusRequest.status_id.ToString(),"Exception");
                }

            }

            return null;
        }

        public static string UpdatePricesBySKU( SallaPriceListRequest sallaPriceListRequest, string tenantName , int priceListId)

        {
            string response = "";

            var stores = ConfigManager.getActiveStores(tenantName).Where(s=>s.EdaraPriceListId==priceListId);
            if (stores == null)
                response = "No stores linked with pricelist " + priceListId.ToString();
            int variantId = 0;
            IRestResponse result;
            foreach (var store in stores)
            {
                SallaPrice sallaPrice = new SallaPrice();
                sallaPrice = sallaPriceListRequest.sallaPrice;
                SallaAPIManger<SallaPrice> man = new SallaAPIManger<SallaPrice>();
                string body = JsonConvert.SerializeObject(sallaPrice);
                // Get Item Id 
                variantId = GetVariantId(sallaPriceListRequest.sku, store);

                if (variantId == 0)
                {
                    // update by SKU
                      result = man.SendRequest("products/sku/" + sallaPriceListRequest.sku + "/price", Method.POST, body, store.SallaToken);

                }
                else
                {
                    // update by Variant Id
                     result = man.SendRequest("products/variants/" +variantId.ToString(), Method.PUT, body, store.SallaToken);

                }
                if (result.StatusCode == System.Net.HttpStatusCode.OK | result.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    response = response + "Price updated successfully for SKU " + sallaPriceListRequest.sku;
                }
                else
                {
                    LogManager.LogMessage("Price updated failed for SKU  " + sallaPriceListRequest.sku,"Exception");
                    response = response + " Price updated failed for SKU  " + sallaPriceListRequest.sku;

                }
            }

            return response;
        }

        public static void updateTokeninDB(string SallaMerchantId, SallsRefreshTokenResponse tokenResponse)
        {
            using (InjazSallaConnectorEntities db = new InjazSallaConnectorEntities())
            {
               SallaAccount sallaAccount =  db.SallaAccounts.Where(s => s.SallaMerchantId == SallaMerchantId).FirstOrDefault();
                sallaAccount.SallaToken = tokenResponse.access_token;
                sallaAccount.SallaRefreshToken = tokenResponse.refresh_token;
                sallaAccount.ModifiedDate = DateTime.Now;
                db.SaveChanges();
            }
        }
        public static void UpdateStock(int qty, string SKU , string tenantName , int warehouseId)
        {

            int variantId = 0;
            var stores= ConfigManager.getActiveStoresByWarehouse(tenantName,warehouseId);
            foreach (var store in stores)
            {

                // Get Item Id 
                 variantId= GetVariantId(SKU, store);

                if (variantId == -1)
                {
                    //Product not exist in Salla 
                    //Product not exist in Salla 
                    LogManager.LogMessage("Product not exist in Salla store  "+ store.SallaStoreName + " SKU " + SKU,"Warning");
                }
                if (variantId == 0)
                {
                    // update by SKU
                    SallaUpdateQuantityRequest sallaUpdateStatusRequest = new SallaUpdateQuantityRequest() { quantity = qty, unlimited_quantity = false };
                    SallaAPIManger<SallaUpdateQuantityRequest> man = new SallaAPIManger<SallaUpdateQuantityRequest>();
                    string body = JsonConvert.SerializeObject(sallaUpdateStatusRequest);
                    IRestResponse result = man.SendRequest("products/quantities/bySku/" + SKU, Method.PUT, body, store.SallaToken);

                }
                else
                {
                    // upadate variant by id
                    SallaUpdateQuantityRequest sallaUpdateStatusRequest = new SallaUpdateQuantityRequest() { quantity = qty, unlimited_quantity = false };
                    SallaAPIManger<SallaUpdateQuantityRequest> man = new SallaAPIManger<SallaUpdateQuantityRequest>();
                    string body = JsonConvert.SerializeObject(sallaUpdateStatusRequest);
                    IRestResponse result = man.SendRequest("products/quantities/variant/" + variantId.ToString(), Method.PUT, body, store.SallaToken);

                }




            }

            //return null;
        }

        public static SallsRefreshTokenResponse GetNewToken(SallaAccount store)
        {
            SallsRefreshTokenRequest sallsRefreshTokenReq = new SallsRefreshTokenRequest() { refresh_token = store.SallaRefreshToken };
            SallaAPIManger<SallsRefreshTokenResponse> man = new SallaAPIManger<SallsRefreshTokenResponse>();

            string body = JsonConvert.SerializeObject(sallsRefreshTokenReq);
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("client_id", sallsRefreshTokenReq.client_id);
            parameters.Add("client_secret", sallsRefreshTokenReq.client_secret);
            parameters.Add("refresh_token", sallsRefreshTokenReq.refresh_token);
            parameters.Add("grant_type", sallsRefreshTokenReq.grant_type);
            IRestResponse result = man.SendTokenRequest("https://accounts.salla.sa/oauth2/token",Method.POST, parameters);
            return  JsonConvert.DeserializeObject<SallsRefreshTokenResponse>(result.Content);

        }

        public static void RefreshToken(int  merchantId)
        {

            var store= ConfigManager.getLinkedEdara(merchantId);
         
                if (store.ModifiedDate != null)
                {
                    TimeSpan difference = DateTime.Now.Subtract(store.ModifiedDate.Value);
                    if (difference.Days > 7)
                    {
                        // update token
                         var result=  GetNewToken(store);
                        //update token
                        updateTokeninDB(store.SallaMerchantId, result);
                        }
                     //   return result;
                    }

            }

            //return null;
        

        public static int GetVariantId(string SKU, SallaAccount sallaEdaraAccount)

        {
            SallaAPIManger<SallaProducstList> man = new SallaAPIManger<SallaProducstList>();
            var result = man.Get("products?keyword=" + SKU, null, null,null,sallaEdaraAccount.SallaToken);

            if (result.pagination.count == 0)
                return -1;

            if (result.data.Where(i => i.sku.ToLower() == SKU.ToLower()).Count() > 0)
                return 0; // Normal stock item

            foreach (var item in result.data)
            {
                var variant = item.skus.Where(v => v.sku == SKU).FirstOrDefault();
                if (variant != null) 
                return variant.id;
            }
            return 0;


        }

        public static string GetOrderId(string OrderNo, int merchantId)

        {
            SallaAccount sallaEdaraAccount = ConfigManager.getLinkedEdara(merchantId);

            SallaAPIManger<SallaSearchOrderListResult> man = new SallaAPIManger<SallaSearchOrderListResult>();
            var result = man.Get("orders?keyword=" + OrderNo, null, null, null, sallaEdaraAccount.SallaToken);

            if (result.pagination.count == 0)
                throw new Exception("Order not exist in Salla for # " + OrderNo);


            return result.data.FirstOrDefault().id;


        }

        public static SallaSalesOrderDTO GetOrderDetails(string OrderNo, int merchantId)

        {
            SallaAccount sallaEdaraAccount = ConfigManager.getLinkedEdara(merchantId);

           string orderId= GetOrderId(OrderNo, merchantId);

            SallaAPIManger<SallaSearchOrderDetailsResult> man = new SallaAPIManger<SallaSearchOrderDetailsResult>();
            var result = man.GetResponse("orders/" + orderId, null, null, null, sallaEdaraAccount.SallaToken);
            SallaSearchOrderDetailsResult order = JsonConvert.DeserializeObject<SallaSearchOrderDetailsResult>(result);

            return order.data;


        }

    }
}