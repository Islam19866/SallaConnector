using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SallaConnector.Models
{
 

        public class EdaraIntegrationUser
        {
            public int user_id { get; set; }
            public string user_name { get; set; }
            public string email { get; set; }
            public string access_token { get; set; }
        }

        public class EdaraIntegrationUserResponse
        {
            public int status_code { get; set; }
            public EdaraIntegrationUser result { get; set; }
        }
    }
