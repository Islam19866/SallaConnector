using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SallaConnector.Models
{
    public class SallsRefreshTokenRequest
    {

        public string client_id = "469a6753-af97-4af5-ab5d-59e29ead9553";
        public string client_secret = "28c9fd50fdaad89a8a1c4bee061b67c8";
        public string refresh_token { get; set; }
        public string grant_type = "refresh_token";

    }

    public class SallsRefreshTokenResponse
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }

    }
}