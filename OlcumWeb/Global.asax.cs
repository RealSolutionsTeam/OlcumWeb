using OlcumWeb.dbOlcum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OlcumWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        OlcumContext db = new OlcumContext();

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        //void Application_PostAuthenticateRequest()
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        var KullniciId = Convert.ToInt32(User.Identity.Name);

        //        PersonelTablo dbpersonel = db.PersonelTablo.Where(x => x.id == KullniciId).FirstOrDefault();
               
        //        var roles = new string[] { dbpersonel.Roller.RolAd };

        //        HttpContext.Current.User = Thread.CurrentPrincipal =  new GenericPrincipal(User.Identity, roles);
                    
        //    }
        //}
    }
}
