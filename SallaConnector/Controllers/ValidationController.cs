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
    public class ValidationController : ApiController
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
        public SettingValidationResponse Post([FromBody]dynamic value)
        {
            try
            {
                SallaEventDTO sallaEvent = value.ToObject<SallaEventDTO>();
                LogManager.LogMessage(JsonConvert.SerializeObject(sallaEvent), "Get Sala Validation Event");
                var result = ConfigManager.updateSallaSettings(sallaEvent);

                if (result.success)
                    return result;

                else
                {
                    throw new Exception(result.message);
                }
            }
            catch (Exception ex)
            {
                //SettingValidationResponse settingValidationResponse = new SettingValidationResponse();
                //settingValidationResponse.status = 442;
                //settingValidationResponse.success = false;
                //settingValidationResponse.code = "error";
                //settingValidationResponse.message = ex.Message;

                //return settingValidationResponse;
                LogManager.LogMessage(ex.Message, "Exception");
                throw;
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
