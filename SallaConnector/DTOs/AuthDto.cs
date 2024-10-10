using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SallaConnector.DTOs
{
    public class AuthDto
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string token_type { get; set; }
    }
}