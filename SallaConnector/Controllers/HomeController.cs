using SallaConnector.Context;
using SallaConnector.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SallaConnector.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            //SallaManager.GetOAuthToken();
            return View();
        }

        public ActionResult IntegrationLog()
        {
           List<RequestLog> log = new List<RequestLog>();
            return View(log);
        }
             [HttpPost]
            public ActionResult IntegrationLog(string keyWord)
        {
            ViewBag.Title = "Integration Log";
            ViewBag.keyWord = keyWord;
            if (keyWord == null)
                keyWord = DateTime.Today.ToString();

           var logs = EdaraBLLManager.Getlogs(keyWord);
 
                return View(logs);
           
          }

        public ActionResult IntegrateOrder(string errorMessage = "")
        {
            ViewBag.merchantId = new SelectList(ConfigManager.getActiveStores("CutePets"), "SallaMerchantId", "SallaStoreName");
            ViewBag.errorMessage = errorMessage;
            return View();

        }
        [HttpPost]
        public ActionResult IntegrateOrder(string orderNo , int merchantId)
        {
            try
            {
                ViewBag.merchantId = new SelectList(ConfigManager.getActiveStores("CutePets"), "SallaMerchantId", "SallaStoreName",merchantId);
               

                var edaraAccount = ConfigManager.getLinkedEdara(merchantId);
          var result=  SallaManager.GetOrderDetails(orderNo, merchantId);
           var response =  EdaraBLLManager.createSalesOrder(result, edaraAccount);

              ViewBag.errorMessage = response.Content;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.errorMessage = ex.Message ;
                return View();
                
            }
        }

        public ActionResult RefreshToken(string errorMessage = "")
        {
            ViewBag.merchantId = new SelectList(ConfigManager.getActiveStores(), "SallaMerchantId", "SallaStoreName");
            ViewBag.errorMessage = errorMessage;
            return View();

        }
        [HttpPost]
        public ActionResult RefreshToken( int merchantId)
        {
            try
            {
                ViewBag.merchantId = new SelectList(ConfigManager.getActiveStores(), "SallaMerchantId", "SallaStoreName", merchantId);
                SallaManager.RefreshToken(merchantId);

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.errorMessage = ex.Message;
                return View();

            }
        }
        public ActionResult Details(int Id)
        {
            ViewBag.Title = "Integration Log details";
            var logs = EdaraBLLManager.GetlogDetails(Id);
            return View(logs);
        }

        public ActionResult PrivacyPolicy()
        {
            ViewBag.Title = "Privacy Policy";
          
            return View();
        }
        public ActionResult FAQs()
        {
            ViewBag.Title = "FAQs";
            
            return View( );
        }
        

    }
}
