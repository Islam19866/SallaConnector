using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SallaConnector.Models
{
    

    

    public class SettingValidationResponse
    {
        public int status { get; set; }
        public bool success { get; set; }
        public string code { get; set; }
        public string message { get; set; }
        public SallaAppSettingsDTO fields { get; set; }
    }
}