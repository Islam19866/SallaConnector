﻿using SallaConnector.Authorization;
using System.Web;
using System.Web.Mvc;

namespace SallaConnector
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
          // filters.Add(new SallaAuth());

        }
    }
}
