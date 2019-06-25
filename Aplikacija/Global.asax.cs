using Aplikacija.Models;
using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Aplikacija
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            Server.ClearError();

            //Logiranje greške u bazu
            Greske greska = new Greske
            {
                VrijemeGreske = DateTime.Now,
                Poruka = exception.Message,
                Izvor = exception.Source,
                Greska = exception.StackTrace
            };

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Greske.Add(greska);
                db.SaveChanges();
            }

            Response.Redirect("~/GlobalError/Index");
        }
    }
}
