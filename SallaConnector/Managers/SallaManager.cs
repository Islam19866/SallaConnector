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
                IRestResponse result = man.SendRequest("orders/:order_id/status", Method.POST, body , store.SallaToken);
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("order_id", SallaOrderId.ToString());
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

            foreach (var store in stores)
            {
                SallaPrice sallaPrice = new SallaPrice();
                sallaPrice = sallaPriceListRequest.sallaPrice;
                SallaAPIManger<SallaPrice> man = new SallaAPIManger<SallaPrice>();
                string body = JsonConvert.SerializeObject(sallaPrice);
                IRestResponse result = man.SendRequest("products/sku/"+ sallaPriceListRequest.sku +"/price", Method.POST, body, store.SallaToken);
               
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

        public static void UpdateStock(int qty, string SKU , string tenantName , int warehouseId)

        {
            var stores= ConfigManager.getActiveStoresByWarehouse(tenantName,warehouseId);
            foreach (var store in stores)
            {
                SallaUpdateQuantityRequest sallaUpdateStatusRequest = new SallaUpdateQuantityRequest() { quantity = qty, unlimited_quantity = false };
                SallaAPIManger<SallaUpdateQuantityRequest> man = new SallaAPIManger<SallaUpdateQuantityRequest>();
                string body = JsonConvert.SerializeObject(sallaUpdateStatusRequest);
                IRestResponse result = man.SendRequest("products/quantities/bySku/" + SKU, Method.PUT, body, store.SallaToken);
                // parameters.Add("sku", SKU);
           

            }

            //return null;
        }

    }
}