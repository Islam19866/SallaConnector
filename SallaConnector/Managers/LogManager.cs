using Newtonsoft.Json;
using RestSharp;
using SallaConnector.Context;
using SallaConnector.Models;
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
        public static void LogSalaMessage(SallaEventDTO sallaevent, string url, string request, string statusCode, string response  , string salesId ="")
        {
            using (InjazSallaConnectorEntities db = new InjazSallaConnectorEntities())
            {


                // Log the request body
                // var bodyParameter = request.Parameters.Find(p => p.Type == ParameterType.RequestBody);

                // initiate log
                RequestLog requestLog = new RequestLog();
                requestLog.MerchantId = sallaevent.merchant.ToString();
                requestLog.EventType = sallaevent.@event.ToString();
                requestLog.EventDetails = JsonConvert.SerializeObject(sallaevent);
                requestLog.RequestDate = DateTime.Now;
                requestLog.DestinationSystem = url;
                requestLog.Payload = request;
                requestLog.ResponseStatus = statusCode.ToString();
                requestLog.ResponseDetails = response;
                db.RequestLogs.Add(requestLog);
                db.SaveChanges();
            }
        }
        public static void LogMessage(string url, string request, string statusCode, string response ,string merchantId="")
        {
            using (InjazSallaConnectorEntities db = new InjazSallaConnectorEntities())
            {


                // Log the request body
                // var bodyParameter = request.Parameters.Find(p => p.Type == ParameterType.RequestBody);

                // initiate log
                RequestLog requestLog = new RequestLog();
                requestLog.RequestDate = DateTime.Now;
                requestLog.MerchantId = merchantId;
                requestLog.DestinationSystem = url;
                requestLog.Payload = request;
                requestLog.ResponseStatus = statusCode.ToString();
                requestLog.ResponseDetails = response;
                db.RequestLogs.Add(requestLog);
                db.SaveChanges();
            }
        }

        public static void LogEdaraMessage(EdaraEventDTO edaraEvent , string url , string request, string statusCode, string response )
        {
            using (InjazSallaConnectorEntities db = new InjazSallaConnectorEntities())
            {
                 

                // Log the request body
               // var bodyParameter = request.Parameters.Find(p => p.Type == ParameterType.RequestBody);
                
                // initiate log
                RequestLog requestLog = new RequestLog();
                requestLog.MerchantId = edaraEvent.entity_id.ToString();
                requestLog.EventType = edaraEvent.entity_type.ToString()+" "+ edaraEvent.event_type ;
                requestLog.EventDetails = JsonConvert.SerializeObject(edaraEvent);  ;
                requestLog.RequestDate = DateTime.Now;
                requestLog.DestinationSystem = url;
                requestLog.Payload = request;
                requestLog.ResponseStatus = statusCode.ToString();
                requestLog.ResponseDetails = response;
                db.RequestLogs.Add(requestLog);
                db.SaveChanges();
            }
        }

        public static void LogSalesOrderMapping(int MerchantId,  string SallaSO , string EdaraSO, string sourceSalesId)
        {
            SalesOrdersMapping salesOrdersMapping = new SalesOrdersMapping();
            salesOrdersMapping.MerchantId = MerchantId.ToString();
            salesOrdersMapping.SourceSalesId = SallaSO;
            salesOrdersMapping.DestinationSalesId = EdaraSO;
            salesOrdersMapping.SourceInternalId = sourceSalesId;
            using (InjazSallaConnectorEntities db = new InjazSallaConnectorEntities())
            {
                db.SalesOrdersMappings.Add(salesOrdersMapping);
                db.SaveChanges();
            }
        }

    }
}