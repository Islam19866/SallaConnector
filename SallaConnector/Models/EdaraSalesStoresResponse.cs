using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SallaConnector.Models
{
    
    public class EdaraSalesStoreDTO
    {
        public int id { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public int default_customer_id { get; set; }
    }

    public class EdaraSalesStoresResponse
    {
        public int status_code { get; set; }
        public List<EdaraSalesStoreDTO> result { get; set; }
        public int total_count { get; set; }
    }
}