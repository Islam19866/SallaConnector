using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SallaConnector.Models
{
    

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class SallaAuthorizeResponse
    {
        public int id { get; set; }
        public string app_name { get; set; }
        public string app_description { get; set; }
        public string app_type { get; set; }
        public string access_token { get; set; }
        public int expires { get; set; }
        public string refresh_token { get; set; }
        public string scope { get; set; }
        public string token_type { get; set; }
    }
 

}