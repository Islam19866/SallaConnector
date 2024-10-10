using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SallaConnector.Models
{
    public class EdaraUserCreateRequest
    {
        public  List<string> roles { get; set; }
        public string user_name { get; set; }
        public int password { get; set; }
        public string email { get; set; }

       
    }
}