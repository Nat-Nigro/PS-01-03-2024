﻿using System.Web;
using System.Web.Mvc;

namespace PS_01_03_2024
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
