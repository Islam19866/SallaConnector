using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SallaConnector.Models
{
    public class EdaraTaxResponse
    {
        public int status_code { get; set; }
        public List<Result> result { get; set; }
    }

    public class Result
    {
        public int id { get; set; }
        public string name { get; set; }
        public double rate { get; set; }
        public int e_invoce_tax_type_id { get; set; }
        public object scope { get; set; }
        public int sales_account_id { get; set; }
        public object sales_withholding_account_id { get; set; }
        public int? purchase_account_id { get; set; }
        public bool active { get; set; }
    }

    
}
