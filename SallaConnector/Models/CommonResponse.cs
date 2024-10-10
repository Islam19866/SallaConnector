using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SallaConnector.Models
{
    public class CommonResponse
    {
         
            public int status_code { get; set; }
            public ResultResponse result { get; set; }
       
    }

    public class ResultResponse
    {
        public int id { get; set; }
        public string name { get; set; }

    }
    
        public class EdaraCity
        {
            public int id { get; set; }
            public string name { get; set; }
            public int country_id { get; set; }
        }

        public class CityResponse
        {
            public int status_code { get; set; }
            public EdaraCity result { get; set; }
        }

    

    public class DistrictResponse : CommonResponse
    {
        public int city_id { get; set; }

    }



}