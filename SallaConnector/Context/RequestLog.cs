//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SallaConnector.Context
{
    using System;
    using System.Collections.Generic;
    
    public partial class RequestLog
    {
        public int Id { get; set; }
        public string MerchantId { get; set; }
        public string RequestId { get; set; }
        public Nullable<System.DateTime> RequestDate { get; set; }
        public string SourceSystem { get; set; }
        public string DestinationSystem { get; set; }
        public string Payload { get; set; }
        public string ResponseStatus { get; set; }
        public string ResponseDetails { get; set; }
    }
}