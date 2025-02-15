﻿using Newtonsoft.Json;
using RestSharp;
using SallaConnector.Context;
using SallaConnector.DTOs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace SallaConnector.Managers
{
    public class SallaAPIManger<T> where T : new()
    {
        internal string Token { get; set; }

        public SallaAPIManger()
        {
            Token = ConfigurationManager.AppSettings["sallaToken"].ToString();
            //using (SFAEntities ctx = new SFAEntities())
            //{
            //    var token = ctx.ApplicationConfigs.FirstOrDefault(i => i.ConKey == "AuthToken");
            //    if (token == null)
            //    {

            //        GetToken();
            //    }
            //    else
            //    {
            //        Token = token.ConValue;
            //    }

            //}

        }

        //public AuthDto GetToken()
        //{
        //    string authUrl = ConfigurationManager.AppSettings["authUrl"].ToString();
        //    string authusername = ConfigurationManager.AppSettings["authusername"].ToString();
        //    string authpassword = ConfigurationManager.AppSettings["authpassword"].ToString();
        //    string clientId = ConfigurationManager.AppSettings["clientId"].ToString();
        //    string clientSecret = ConfigurationManager.AppSettings["clientSecret"].ToString();



        //    var client = new RestClient(authUrl);

        //    var request = new RestRequest(Method.POST);
        //    request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
        //    request.AddParameter("grant_type", "password");
        //    request.AddParameter("username", authusername);
        //    request.AddParameter("password", authpassword);
        //    request.AddParameter("scope", "api");
        //    request.AddParameter("client_id", clientId);
        //    request.AddParameter("client_secret", clientSecret);
        //    ServicePointManager.ServerCertificateValidationCallback +=
        //(sender, certificate, chain, sslPolicyErrors) => true;

        //    //request.AddParameter("username", "Radwa");
        //    //request.AddParameter("password", "Azxcvb@123");
        //    //request.AddParameter("scope", "api");
        //    //request.AddParameter("client_id", "4BCDAE56-1248-5516-BD37-D3BA858422EF@2B Egypt");
        //    //request.AddParameter("client_secret", "1zZLEacVzjf5jFyNHfOceQ");
        //    IRestResponse<AuthDto> response = client.Execute<AuthDto>(request);

        //    using (SFAEntities ctx = new SFAEntities())
        //    {
        //        var token = ctx.ApplicationConfigs.FirstOrDefault(i => i.ConKey == "AuthToken");

        //        if (token == null)
        //        {
        //            ctx.ApplicationConfigs.Add(new ApplicationConfig()
        //            {
        //                ConKey = "AuthToken",
        //                ConValue = response.Data.access_token
        //            });
        //            ctx.SaveChanges();
        //        }

        //        token.ConValue = response.Data.access_token;
        //        ctx.SaveChanges();

        //        Token = response.Data.access_token;

        //    }

        //    return response.Data;

        //}

        public T Post(string url, Dictionary<string, string> headers, string body)
        {
            var baseurl = ConfigurationManager.AppSettings["sallaIntegBaseUrl"].ToString();
            var client = new RestClient(baseurl + url);
            
            var request = new RestRequest(Method.POST);

            request.AddHeader("Authorization", "Bearer " + Token);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            if (headers != null)
            {
                foreach (var item in headers)
                {
                    request.AddHeader(item.Key, item.Value);
                }
            }

            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse<T> response = client.Execute<T>(request);
          LogManager.LogMessage(client.BaseUrl,body, response.StatusCode.ToString(),response.Content);
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new Exception("Unauthorized");
            }

            return response.Data;

        }

        public T Put(string url, Dictionary<string, string> headers, string body, string referenceNo, string userId)
        {
            var baseurl = ConfigurationManager.AppSettings["sallaIntegBaseUrl"].ToString();
            var client = new RestClient(url);
            var request = new RestRequest(Method.PUT);
            request.AddHeader("Authorization", "Bearer " + Token);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            if (headers != null)
            {
                foreach (var item in headers)
                {
                    request.AddHeader(item.Key, item.Value);
                }
            }

            request.AddParameter("application/json", body, ParameterType.RequestBody);
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback
                    (
                       delegate { return true; }
                    );

            IRestResponse<T> response = client.Execute<T>(request);

          LogManager.LogMessage( client.BaseUrl,body, response.StatusCode.ToString(),response.Content);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                Put(url, headers, body, referenceNo, userId);
            }

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception("StatusCode " + response.StatusCode + "  -  ErrorMessage : " + response.ErrorMessage);
            }
            return response.Data;

        }




        public T Get(string url, Dictionary<string, string> headers, Dictionary<string, string> parameters, string body, string authData = "")
        {

            var baseurl = ConfigurationManager.AppSettings["sallaIntegBaseUrl"].ToString();

            var client = new RestClient(baseurl+url);
            var request = new RestRequest(Method.GET);
            if (string.IsNullOrEmpty(authData))
            {
                request.AddHeader("Authorization", "Bearer " + Token);
            }
            else
            {
                request.AddHeader("Authorization", "Bearer " + authData);
            }
           // request.AddHeader("Accept-Encoding", "gzip, deflate, br");
            //request.AddHeader("Content-Type", "application/json");
            if (headers != null)
            {
                foreach (var item in headers)
                {
                    request.AddHeader(item.Key, item.Value);
                }
            }
            if (parameters != null)
            {
                foreach (var item in parameters)
                {
                    request.AddParameter(item.Key, item.Value);
                }
            }

            if (!string.IsNullOrEmpty(body))
                request.AddParameter("application/json", body, ParameterType.RequestBody);

            

            IRestResponse<T> response = client.Execute<T>(request);


            // initiate log
          // LogManager.LogMessage( client.BaseUrl,body, response.StatusCode.ToString(),response.Content);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {

                throw new Exception("Unauthorized");
            }

            

            return response.Data;

        }

        public string GetResponse(string url, Dictionary<string, string> headers, Dictionary<string, string> parameters, string body, string authData = "")
        {

            var baseurl = ConfigurationManager.AppSettings["sallaIntegBaseUrl"].ToString();

            var client = new RestClient(baseurl + url);
            var request = new RestRequest(Method.GET);
            if (string.IsNullOrEmpty(authData))
            {
                request.AddHeader("Authorization", "Bearer " + Token);
            }
            else
            {
                request.AddHeader("Authorization", "Bearer " + authData);
            }
            // request.AddHeader("Accept-Encoding", "gzip, deflate, br");
            //request.AddHeader("Content-Type", "application/json");
            if (headers != null)
            {
                foreach (var item in headers)
                {
                    request.AddHeader(item.Key, item.Value);
                }
            }
            if (parameters != null)
            {
                foreach (var item in parameters)
                {
                    request.AddParameter(item.Key, item.Value);
                }
            }

            if (!string.IsNullOrEmpty(body))
                request.AddParameter("application/json", body, ParameterType.RequestBody);



            IRestResponse<T> response = client.Execute<T>(request);


            // initiate log
            // LogManager.LogMessage( client.BaseUrl,body, response.StatusCode.ToString(),response.Content);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {

                throw new Exception("Unauthorized");
            }



            return response.Content;

        }
        public IRestResponse SendTokenRequest(string url, Method method, Dictionary<string, string> parameters = null)
        {
           // var baseurl = ConfigurationManager.AppSettings["sallaIntegBaseUrl"].ToString();
            //if (url.Contains("http"))
            //    baseurl = "";

            var client = new RestClient(url);
            var request = new RestRequest(method);

 

            if (parameters != null)
            {
                foreach (var item in parameters)
                {
                    request.AddParameter(item.Key, item.Value);
                }
            }
            request.AddParameter("client_id", "469a6753-af97-4af5-ab5d-59e29ead9553");
            request.AddParameter("client_secret", "28c9fd50fdaad89a8a1c4bee061b67c8");
            request.AddParameter("refresh_token", "ory_rt_bJEVHI528XXrWh-L7hIR5MwoAEMfkzLakyN1x5cId28.XyVVpPjIEJML25RWNvsFedD8lafupG2tFFuiRxyCcwI");
            request.AddParameter("grant_type", "refresh_token");

            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
           // request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse<T> response = client.Execute<T>(request);

            LogManager.LogMessage(client.BaseUrl, null, response.StatusCode.ToString(), response.Content);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {

                throw new Exception("Unauthorized");
            }

            return response;

        }

        public IRestResponse SendRequest(string url, Method method, string body, string authData = "", Dictionary<string, string> parameters = null)
        {
            var baseurl = ConfigurationManager.AppSettings["sallaIntegBaseUrl"].ToString();
            if (url.Contains("http"))
                baseurl = "";

            var client = new RestClient(baseurl + url);
            var request = new RestRequest(method);


            if (string.IsNullOrEmpty(authData))
            {
                request.AddHeader("Authorization", "Bearer " + Token);
            }
            else
            {
                request.AddHeader("Authorization", "Bearer " + authData);
            }

            if (parameters != null)
            {
                foreach (var item in parameters)
                {
                    request.AddParameter(item.Key, item.Value);
                }
            }

            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");

            


            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse<T> response = client.Execute<T>(request);

          LogManager.LogMessage(client.BaseUrl,body, response.StatusCode.ToString(),response.Content);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {

                throw new Exception("Unauthorized");
            }

            return response;

        }
    }
}