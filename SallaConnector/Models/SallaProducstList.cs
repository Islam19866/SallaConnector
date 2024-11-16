using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SallaConnector.Models
{
    public class SallaProducstList
    {
        public int status { get; set; }
        public bool success { get; set; }
        public List<Datum> data { get; set; }
        public Pagination pagination { get; set; }
        
    }

   
    
    public class CostPrice1
    {
        public int amount { get; set; }
        public string currency { get; set; }
    }

    public class Datum
    {
        public int id { get; set; }
      
        public int quantity { get; set; }
        public string sku { get; set; }
        public List<Sku> skus { get; set; }
  
    }

  

   

    public class Pagination
    {
        public int count { get; set; }
        public int total { get; set; }
        public int perPage { get; set; }
        public int currentPage { get; set; }
        public int totalPages { get; set; }
        public List<object> links { get; set; }
    }

   
 
   
 
   

    public class Sku
    {
        public int id { get; set; }
  
        public string sku { get; set; }
 
    }
 
 


}