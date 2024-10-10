using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SallaConnector.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class SallaCustomerDTO
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public int mobile { get; set; }
        public string mobile_code { get; set; }
        public string email { get; set; }
        public Urls urls { get; set; }
        public string avatar { get; set; }
        public string gender { get; set; }
        public object birthday { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string country_code { get; set; }
        public string currency { get; set; }
        public string location { get; set; }
        public UpdatedAt updated_at { get; set; }
        public List<object> groups { get; set; }
        public Source source { get; set; }
    }

   

    public class Source
    {
        public string device { get; set; }

        [JsonProperty("user-agent")]
        public string useragent { get; set; }
        public string ip { get; set; }
    }

 

    public class Urls
    {
        public string customer { get; set; }
        public string admin { get; set; }
    }


}