using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace SkautSIS.Core.CoreModel
{
    public partial class CoreContext
    {
        public string GetServiceUrl(string serviceName)
        {
            var uri = new Uri(string.Format("GetServiceUrl?serviceName='{0}'", serviceName), UriKind.Relative);

            return Execute<string>(uri).FirstOrDefault();
        }
    }
}