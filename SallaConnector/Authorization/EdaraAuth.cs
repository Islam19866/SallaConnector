using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;

namespace SallaConnector.Authorization
{
    
    public class EdaraAuth : AuthorizeAttribute
    {

        private readonly string _token = ConfigurationManager.AppSettings["EdaraToken"];

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            try
            {

            
            // Get token from headers (for POST requests)
            var request = httpContext.Request;

             
            var tokenProvided = request.Headers.GetValues("Authorization").FirstOrDefault();

            // Validate token
            if (string.IsNullOrEmpty(tokenProvided) || tokenProvided != _token)
            {
                return false;
            }

            return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

    }
}

  



