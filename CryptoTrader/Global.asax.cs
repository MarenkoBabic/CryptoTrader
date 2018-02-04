using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using CryptoTrader.Models.ViewModel;

namespace CryptoTrader
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AutoMapper.Mapper.Initialize(cfg => cfg.AddProfile<AutoMapperProfile>());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters( GlobalFilters.Filters );
            RouteConfig.RegisterRoutes( RouteTable.Routes );
            BundleConfig.RegisterBundles( BundleTable.Bundles );
        }

        protected void Application_AuthenticateRequest( Object sender, EventArgs e )
        {
            HttpCookie authCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];

            if( authCookie == null || authCookie.Value == "" )
                return;

            FormsAuthenticationTicket authTicket;

            try
            {
                authTicket = FormsAuthentication.Decrypt( authCookie.Value );
            }
            catch
            {
                return;
            }


            string userData = authTicket.UserData;
            string[] role = userData.Split( ',' );


            Context.User = new GenericPrincipal( new GenericIdentity( authTicket.Name ), role );
        }

    }
}
