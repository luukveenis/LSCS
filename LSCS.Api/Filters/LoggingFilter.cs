using System.Globalization;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using log4net;

namespace LSCS.Api.Filters
{
    public class LoggingFilter : ActionFilterAttribute
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (LoggingFilter));

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            string url = actionContext.Request.RequestUri.ToString();
            string method = actionContext.Request.Method.Method;
            string controller = actionContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string action = actionContext.ActionDescriptor.ActionName;

            Log.DebugFormat(CultureInfo.InvariantCulture,
                "{0} {1} Controller:{2} Action:{3}",
                method,
                url,
                controller,
                action
                );
        }
    }
}