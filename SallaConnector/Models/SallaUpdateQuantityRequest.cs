using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SallaConnector.Models
{
    public class SallaUpdateQuantityRequest
    {
        public int quantity { get; set; }
        public Boolean unlimited_quantity { get; set; }
    }
}