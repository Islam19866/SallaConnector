using Newtonsoft.Json;
using RestSharp;
using SallaConnector.Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace SallaConnector.Managers
{
    public class LogManager
    {
        public static void LogMessage(string message, string type)
        {
            using (InjazSallaConnectorEntities db = new InjazSallaConnectorEntities())
            {

                // initiate log
                RequestLog requestLog = new RequestLog();
                requestLog.RequestDate = DateTime.Now;
                requestLog.Payload = message;
                requestLog.ResponseStatus = type;
                db.RequestLogs.Add(requestLog);
                db.SaveChanges();
            }
        }

        public static void LogMessage( string url , string request, string statusCode, string response )
        {
            using (InjazSallaConnectorEntities db = new InjazSallaConnectorEntities())
            {
                 

                // Log the request body
               // var bodyParameter = request.Parameters.Find(p => p.Type == ParameterType.RequestBody);
                
                // initiate log
                RequestLog requestLog = new RequestLog();
                requestLog.RequestDate = DateTime.Now;
                requestLog.DestinationSystem = url;
                requestLog.Payload = request;
                requestLog.ResponseStatus = statusCode.ToString();
                requestLog.ResponseDetails = response;
                db.RequestLogs.Add(requestLog);
                db.SaveChanges();
            }
        }

        public static void LogSalesOrderMapping(int MerchantId,  string SallaSO , string EdaraSO)
        {
            SalesOrdersMapping salesOrdersMapping = new SalesOrdersMapping();
            salesOrdersMapping.MerchantId = MerchantId.ToString();
            salesOrdersMapping.SourceSalesId = SallaSO;
            salesOrdersMapping.DestinationSalesId = EdaraSO;

            using (InjazSallaConnectorEntities db = new InjazSallaConnectorEntities())
            {
                db.SalesOrdersMappings.Add(salesOrdersMapping);
                db.SaveChanges();
            }
        }

    }
}