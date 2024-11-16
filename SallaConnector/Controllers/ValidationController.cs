using Newtonsoft.Json;
using SallaConnector.Managers;
using SallaConnector.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace SallaConnector.Controllers
{
    public class ValidationController : ApiController
    {
         

        // POST api/values
        public IHttpActionResult Post([FromBody]dynamic value)
        {
            try
            {
                SallaEventDTO sallaEvent = value.ToObject<SallaEventDTO>();
                LogManager.LogMessage(JsonConvert.SerializeObject(sallaEvent), "Get Sala Validation Event");
                var result = ConfigManager.updateSallaSettings(sallaEvent);

                if (result.success)
                    return Ok();

                else
                {
                    throw new Exception(result.message);
                }
            }
            catch (Exception ex)
            {
                SettingValidationResponse settingValidationResponse = new SettingValidationResponse();
                settingValidationResponse.status = 442;
                settingValidationResponse.success = false;
                settingValidationResponse.code = "error";
                settingValidationResponse.message = ex.Message;
                settingValidationResponse.fields = new SallaAppSettingsDTO()
                {
                    WarehouseName = ex.Message

                };
               // return settingValidationResponse;
                LogManager.LogMessage(ex.Message, "Exception");
                return StatusCode((HttpStatusCode)422);
                throw;

            }

        }

    }
}
