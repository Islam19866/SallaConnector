using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SallaConnector.Models
{
    public class EdaraEventDTO
    {
        public string entity_id { get; set; }
        public string entity_code { get; set; }
        public string entity_type { get; set; }
        public string event_type { get; set; }
        public string external_id { get; set; }
        public string tenant_id { get; set; }
        public string tenant_name { get; set; }
        public dynamic data { get; set; }
        public dynamic message_attributes { get; set; }
        
    }
}