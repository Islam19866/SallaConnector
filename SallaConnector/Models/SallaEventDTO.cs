using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SallaConnector.Models
{
    public class SallaEventDTO
    {
         
            public string @event { get; set; }
            public int merchant { get; set; }
            public string created_at { get; set; }
            public dynamic data { get; set; }

    }
 
   


}