using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using ServiceStack.WebHost.Endpoints;
using System.Configuration;

namespace mailrest
{
    public class Global : System.Web.HttpApplication
    {

        public class MailMessageAppHost : AppHostBase
        {
            //Tell Service Stack the name of your application and where to find your web services
            public MailMessageAppHost() : base("Hello Web Services", typeof(MailService).Assembly) { }

            public override void Configure(Funq.Container container)
            {
                //register any dependencies your services use, e.g:
                //container.Register<ICacheClient>(new MemoryCacheClient());
            }
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            new MailMessageAppHost().Init();
            using (var dbFactory = new MailData.DataFactory(ConfigurationManager.ConnectionStrings["filgiftsMail"].ConnectionString))
            {
                dbFactory.Initialize();
            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}