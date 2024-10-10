using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SallaConnector.Models
{
    public class EdaraCityCreateRequest
    {
        public string name { get; set; }
        public int country_id { get; set; }
    }
}