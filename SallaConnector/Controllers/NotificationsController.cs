using Newtonsoft.Json;
using RestSharp;
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
    public class NotificationsController : ApiController
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

                SallaEventDTO sallaEvent = value.ToObject<SallaEventDTO>();

                var edaraAccount = ConfigManager.getLinkedEdara(sallaEvent.merchant);

                if (edaraAccount == null)
                {
                    LogManager.LogMessage(JsonConvert.SerializeObject(sallaEvent),"No edara account linked to merchant id " + sallaEvent.merchant);

                    return "No edara account linked to merchant id " + sallaEvent.merchant;
                }
                if (sallaEvent.@event.Contains("customer.created"))
                {
                    LogManager.LogMessage(JsonConvert.SerializeObject(sallaEvent), "Salla Event " + sallaEvent.@event);

                    SallaCustomerDTO sallaCustomer = sallaEvent.data.ToObject<SallaCustomerDTO>();
                    return  EdaraBLLManager.createCustomer(sallaCustomer, edaraAccount).Content;
                }

                if (sallaEvent.@event.Contains("order.created"))
                {
                    LogManager.LogMessage(JsonConvert.SerializeObject(sallaEvent), "Salla Event " + sallaEvent.@event);


                    return EdaraBLLManager.createSalesOrder(sallaEvent, edaraAccount).Content;

                }
                if (sallaEvent.@event.Contains("order.updated"))
                {
                    LogManager.LogMessage(JsonConvert.SerializeObject(sallaEvent), "Salla Event " + sallaEvent.@event);


                    return EdaraBLLManager.updateSalesOrder(sallaEvent, edaraAccount).Content;

                }
                if (sallaEvent.@event.Contains("app.store.authorize"))
                {
                    LogManager.LogMessage(JsonConvert.SerializeObject(sallaEvent), "Salla Event " + sallaEvent.@event);

                    if (ConfigManager.addSallaAccount(sallaEvent))
                        return "App installed ";
                    else
                    {
                        return "App not installed ";
                    }
                }
                
                else
                {
                    LogManager.LogMessage(JsonConvert.SerializeObject(sallaEvent), "Salla Event " + sallaEvent.@event);

                    return "Event not defined";
                }

                //if (sallaEvent.@event.Contains("order.created"))
                //{

                //}
                
            }

            catch (Exception ex)
            {
                LogManager.LogMessage(JsonConvert.SerializeObject(ex),"exception");
                //
                return JsonConvert.SerializeObject(ex);
            }

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
