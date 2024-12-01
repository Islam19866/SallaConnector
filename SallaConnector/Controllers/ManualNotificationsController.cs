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
    public class ManualNotificationsController : ApiController
    {
       
        // POST api/values
       
        
        public IHttpActionResult Post([FromBody]dynamic value)
        {




            SallaSearchOrderDetailsResult manualSallaEvent1 = value.ToObject<SallaSearchOrderDetailsResult>();

            SallaSearchOrderDetailsResult2 manualSallaEvent = value.ToObject<SallaSearchOrderDetailsResult2>();

            try
            {
          
                IRestResponse result = new RestResponse();
                var edaraAccount = ConfigManager.getLinkedEdara(manualSallaEvent.merchantid);
                if (edaraAccount == null)
                    return Ok("No Account linked with merchant id " + manualSallaEvent.merchantid.ToString());

                    //result = EdaraBLLManager.createSalesOrder(manualSallaEvent.data, edaraAccount);
                    if (result.StatusCode == HttpStatusCode.OK)
                        return Ok(result.Content);
                    else
                    {
                        throw new Exception(result.StatusDescription);
                    }
                
            }


            catch (Exception ex)
            {
               // LogManager.LogSalaMessage(sallaEvent, null, null, "exception", JsonConvert.SerializeObject(ex));

                // LogManager.LogSalaMessage(JsonConvert.SerializeObject(ex),"exception");
                //
                return InternalServerError(ex);
            }

        }

    }
}
