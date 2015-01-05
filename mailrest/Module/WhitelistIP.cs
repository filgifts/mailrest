using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace mailrest.Module
{
    public class WhitelistIP : IHttpModule
    {
        private EventHandler onBeginRequest;

        public WhitelistIP()
        {
            onBeginRequest = new EventHandler(this.ProcessIP);
        }

        public void Init(HttpApplication application)
        {
            application.BeginRequest += ProcessIP;
        }

        private void ProcessIP(Object source, EventArgs e)
        {
            HttpApplication application = (HttpApplication)source;

            var whitelistIPConfig = (string)ConfigurationManager.AppSettings["whiteListIP"];
            var whiteListIPs = whitelistIPConfig.Split(',');

            string remoteIPAddress = application.Context.Request.ServerVariables["REMOTE_ADDR"];

            if (!whiteListIPs.Contains(remoteIPAddress))
            {
                application.Context.Response.StatusCode = 404;
                application.Context.Response.SuppressContent = true;
                application.Context.Response.End();
            }
        }

        public void Dispose() { }
    }
}