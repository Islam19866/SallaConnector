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
            ViewBag.Title = "Integration Log";
           var logs= EdaraBLLManager.Getlogs();
            return View(logs);
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
