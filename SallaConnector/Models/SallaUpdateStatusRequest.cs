using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SallaConnector.Models
{
    public class SallaUpdateStatusRequest
    {
      
        public int status_id { get; set; }
        public string slug { get; set; }
        public string note { get; set; }
         
 }
}