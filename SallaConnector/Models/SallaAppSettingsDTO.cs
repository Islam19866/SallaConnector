using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SallaConnector.Models
{
    //public class SallaAppSettingsDTO
    //{
    //    public int id { get; set; }
    //    public string app_name { get; set; }
    //    public string app_description { get; set; }
    //    public string app_type { get; set; }
    //    public Settings settings { get; set; }
    //}

    public class SallaAppSettingsDTO
    {
        public string Name { get; set; }
        public string email { get; set; }
        public string UserName { get; set; }
        public string EdaraToken { get; set; }
        public string EdaraURL { get; set; }
        public string WarehouseName { get; set; }
        public string SalesStoreName { get; set; }
        public string CODService { get; set; }
        public string ShippingServiceName { get; set; }
        public int PriceListId { get; set; }
        public int InProgressStatus { get; set; }
        public int ShippingStatus { get; set; }


    }


  
}