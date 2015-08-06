using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using System.Data.Entity;
using CimenaCityProject.Models;
using CimenaCityProject.Logic;


namespace CimenaCityProject
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            try
            {

            }
            catch (Exception ex)
            {
                
               
            }

            //create user idemtity interaction 
            //for custom role and identity
            //ActionRole actionRole = new ActionRole();
            //actionRole.AddUserAndRole();
        }
    }
}
