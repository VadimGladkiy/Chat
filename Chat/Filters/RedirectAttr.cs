using System;
using System.Net.Http;
using System.Web.Http.Filters;
using System.Net;
using System.Threading.Tasks;
using System.Threading;

namespace Chat.Filters
{
    public class RedirectAttr : ActionFilterAttribute
    {
        public RedirectAttr(){}
        
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var uri = actionExecutedContext.Request.RequestUri.Authority;
            var response = new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.Moved;
            response.Headers.Location = new Uri("http://" + uri + "/Home/Index");
            actionExecutedContext.Response = response;
            
        }
    }
}
    