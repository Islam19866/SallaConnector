using Newtonsoft.Json;
using SallaConnector.Managers;
using SallaConnector.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SallaConnector.Controllers
{
    public class EdaraNotificationsController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

       
        // POST api/values
        public string Post([FromBody]dynamic value)
        {
            try
            {


                EdaraEventDTO edaraEvent = value.ToObject<EdaraEventDTO>();

                if (edaraEvent.entity_type == "StockItem") 
                {
                    LogManager.LogMessage(JsonConvert.SerializeObject(value), "Info");

                    if (edaraEvent.event_type == "Balance_Increased" | edaraEvent.event_type == "Balance_Changed")
                    {
                        StockItemBalanceRoot edaraStockItem = value.ToObject<StockItemBalanceRoot>();

                        foreach (var item in edaraStockItem.message_attributes.StockItemBalances)
                        {
                            SallaManager.UpdateStock(int.Parse(item.OnHandBalance.ToString()), edaraStockItem.data.sku,edaraEvent.tenant_name, item.WarehouseId);

                        }

                    }

                }
                if (edaraEvent.entity_type == "SalesOrder" & edaraEvent.event_type == "Status_Changed")
                {
                    LogManager.LogMessage(JsonConvert.SerializeObject(value), "Info");

                    int statusId;
                    EdaraSOStatusResponse edaraSOStatusResponse = edaraEvent.data.ToObject<EdaraSOStatusResponse>();
                    string edaraStatusName = edaraSOStatusResponse.order_status;
                    string merchantId = edaraSOStatusResponse.channel.Split('-')[1].ToString();
                    if (edaraStatusName == "Processing" | edaraStatusName == "Shipeped")
                    {
                       // merchantId = edaraSOStatusResponse.channel.Split('-')[1].ToString();
                        statusId = ConfigManager.getMappedSallaStatus( merchantId, edaraStatusName);
                        if (statusId != 0)
                                SallaManager.UpdateSalesOrderStatus(new SallaUpdateStatusRequest() { status_id = statusId }, int.Parse(edaraSOStatusResponse.paper_number), edaraEvent.tenant_name, merchantId);
                    }
                }
                if (edaraEvent.entity_type == "PriceList")
                {
                    LogManager.LogMessage(JsonConvert.SerializeObject(value), "Info");

                    // need to get price code
                    string result = "";
                    EdaraPriceListResponse edaraPriceListResponse = edaraEvent.data.ToObject<EdaraPriceListResponse>();
                    SallaPriceListRequest sallPriceRequest = new SallaPriceListRequest();
                    foreach (var price in edaraPriceListResponse.priceList_details)
                    {
                          sallPriceRequest.sallaPrice =new SallaPrice() {
                            price=price.pricelist_price.ToString(),
                            sale_price=price.pricelist_price.ToString()
                             
                        };
                        sallPriceRequest.sku = price.sku;
                         result = result+  SallaManager.UpdatePricesBySKU(sallPriceRequest, edaraEvent.tenant_name, edaraPriceListResponse.priceList_id);
                    }
                    return JsonConvert.SerializeObject(result);

                }

                return "Success";

            }

            catch (Exception ex)
            {
                LogManager.LogMessage(JsonConvert.SerializeObject(ex),"Exception");
                return JsonConvert.SerializeObject(ex.Message);

            }

        }

        private string getMerchantId(string channel)
        {
            return channel.Split('-').FirstOrDefault();
        }




        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
