using SallaConnector.Managers;
using SallaConnector.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace SallaIntegrationJob
{
    class Program
    {
        static void Main(string[] args)
        {
            string chaanel = "Injaz-23434";
            chaanel = chaanel.Split('-')[1].ToString() ;
            Console.WriteLine(chaanel);
            //var stores = ConfigManager.getActiveStores("").FirstOrDefault();
            //EdaraUserCreateRequest edaraUserCreateRequest = new EdaraUserCreateRequest();
            //edaraUserCreateRequest.user_name = stores.EdaraUser;
            //edaraUserCreateRequest.email = stores.EdaraEmail.ToString();
            //edaraUserCreateRequest.password = stores.edarap;
            //EdaraIntegration.CreateUser(,);
            //SallaManager.UpdateStock(500, "3182550704618");
        }
    }
}
