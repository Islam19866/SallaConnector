using Newtonsoft.Json;
using RestSharp;
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
    public class NotificationsController : ApiController
    {
       
        // POST api/values
       
        
        public IHttpActionResult Post([FromBody]dynamic value)
        {

            //if (Request.Headers.Contains("Authorization"))
            //{
            //    var result = SallaAuth.Authorize((Request.Headers.GetValues("Authorization").FirstOrDefault()));
            //    if (!result)
            //    {
            //        LogManager.LogMessage(Request.Headers.GetValues("Authorization").FirstOrDefault(), "Unauthorized");

            //        return Unauthorized();
            //    }
            //}
            //else
            //{
            //    return BadRequest("Required header is missing.");
            //}

           
            SallaEventDTO sallaEvent = value.ToObject<SallaEventDTO>();

            try
            {
                List<string> events = new List<string>() { "customer.created", "order.created", "order.updated", "app.store.authorize" };
                if (!events.Contains(sallaEvent.@event))
                {
                    LogManager.LogSalaMessage(sallaEvent, null, null, "NA", null);
                    return Ok();
                }


                IRestResponse result = new RestResponse();
                var edaraAccount = ConfigManager.getLinkedEdara(sallaEvent.merchant);
                //if (edaraAccount == null)
                //    return Ok("No Account linked with merchant id " + sallaEvent.merchant.ToString());

                if (sallaEvent.@event.Contains("customer.created"))
                {
                    SallaCustomerDTO sallaCustomer = sallaEvent.data.ToObject<SallaCustomerDTO>();
                    result = EdaraBLLManager.createCustomer(sallaCustomer, edaraAccount);

                }
                if (sallaEvent.@event.Contains("order.created"))
                {
                    SallaManager.RefreshToken(int.Parse(edaraAccount.SallaMerchantId));
                    result = EdaraBLLManager.createSalesOrder(sallaEvent, edaraAccount);

                }
                if (sallaEvent.@event.Contains("order.updated"))
                {

                    result = EdaraBLLManager.updateSalesOrder(sallaEvent, edaraAccount);

                }
                if (sallaEvent.@event.Contains("app.store.authorize"))
                {
                    LogManager.LogMessage(JsonConvert.SerializeObject(sallaEvent), "Salla Event " + sallaEvent.@event);

                    if (ConfigManager.addSallaAccount(sallaEvent))
                        return Ok("App installed ");
                    else
                    {
                        return InternalServerError();

                    }
                }

              
                    LogManager.LogSalaMessage(sallaEvent, result.ResponseUri.ToString(), JsonConvert.SerializeObject(result.Request), result.StatusCode.ToString(), result.Content);

                    if (result.StatusCode == HttpStatusCode.OK)
                        return Ok(result.Content);
                    else
                    {
                        throw new Exception(result.StatusDescription);
                    }
                
            }


            catch (Exception ex)
            {
                LogManager.LogSalaMessage(sallaEvent, null, null, "exception", JsonConvert.SerializeObject(ex.Message));

                // LogManager.LogSalaMessage(JsonConvert.SerializeObject(ex),"exception");
                //
                return InternalServerError(ex);
            }

        }

    }
}
