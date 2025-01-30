using Newtonsoft.Json;
using SallaConnector.Managers;
using SallaConnector.Models;
using SallaConnector.Authorization;
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
        

       
        // POST api/values
       // [EdaraAuth]
        public string Post([FromBody]dynamic value)
        {
            // LogManager.LogMessage(value,"Edara Event");

          //  SallaProducstList edaraEvent1 = value.ToObject<SallaProducstList>();

            EdaraEventDTO edaraEvent = value.ToObject<EdaraEventDTO>();

            LogManager.LogEdaraMessage(edaraEvent, null, null, "Start", null);

            string result = "";
            try
            {

                if (edaraEvent.entity_type == "StockItem") 
                {
                   

                    if (edaraEvent.event_type == "Balance_Increased" | edaraEvent.event_type == "Balance_Changed" | edaraEvent.event_type ==  "Balance_Decreased")
                    {
                        StockItemBalanceRoot edaraStockItem = value.ToObject<StockItemBalanceRoot>();
                 

                        foreach (var item in edaraStockItem.message_attributes.StockItemBalances)
                        {
                            SallaManager.UpdateStock(int.Parse((item.OnHandBalance - item.Reserved_Balance).ToString()), edaraStockItem.data.sku,edaraEvent.tenant_name, item.WarehouseId);

                        }

                    }

                }
                if (edaraEvent.entity_type == "SalesOrder" & edaraEvent.event_type == "Status_Changed")
                {
                    

                    int statusId;
                    EdaraSOStatusResponse edaraSOStatusResponse = edaraEvent.data.ToObject<EdaraSOStatusResponse>();
                    string edaraStatusName = edaraSOStatusResponse.order_status;
                    string merchantId = edaraSOStatusResponse.channel.Split('-')[1].ToString();
                    string sallaInternalId = ConfigManager.geSallaInternalId(edaraSOStatusResponse.document_code,merchantId);
                    if (edaraStatusName == "Processing" | edaraStatusName == "Shipeped")
                    {
                        // merchantId = edaraSOStatusResponse.channel.Split('-')[1].ToString();
                        statusId = ConfigManager.getMappedSallaStatus(merchantId, edaraStatusName);
                        {
                            if (statusId != 0)
                               SallaManager.UpdateSalesOrderStatus(new SallaUpdateStatusRequest() { status_id = statusId }, int.Parse(sallaInternalId), edaraEvent.tenant_name, merchantId);
                        }
                    }
                }
                if (edaraEvent.entity_type == "PriceList")
                {
                    

                    // need to get price code
                    
                    EdaraPriceListResponse edaraPriceListResponse = edaraEvent.data.ToObject<EdaraPriceListResponse>();
                    SallaPriceListRequest sallPriceRequest = new SallaPriceListRequest();
                    foreach (var price in edaraPriceListResponse.priceList_details)
                    {
                          sallPriceRequest.sallaPrice =new SallaPrice() {
                            price=price.pricelist_price.ToString()
                            //sale_price=price.pricelist_price.ToString()
                             
                        };
                        sallPriceRequest.sku = price.sku;
                         result = result+  SallaManager.UpdatePricesBySKU(sallPriceRequest, edaraEvent.tenant_name, edaraPriceListResponse.priceList_id);
                    }
                    result= JsonConvert.SerializeObject(result);

                }


                LogManager.LogEdaraMessage(edaraEvent, null, result, "Ok", result);
             
                    return "Success";
            }

            catch (Exception ex)
            {
                //LogManager.LogMessage(JsonConvert.SerializeObject(ex),"Exception");
                LogManager.LogEdaraMessage(edaraEvent, null, null, "failed", JsonConvert.SerializeObject(ex));

                return JsonConvert.SerializeObject(ex.Message);

            }

        }

        private string getMerchantId(string channel)
        {
            return channel.Split('-').FirstOrDefault();
        }

 
    }
}
