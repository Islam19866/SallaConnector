using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace SallaConnector.Authorization
{
    public class SallaAuth 

    {

        private static string _token = ConfigurationManager.AppSettings["SallaToken"];
        public static bool Authorize(string tokenProvided)
        {
            try
            {


            
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

  



