using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net.Http;
using System.Security.Principal;


using System.Net;

namespace Chat.Filters
{
    public class MyAuthorizeAttribute :  AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.RequestContext.Principal.Identity.IsAuthenticated)
            {
                
            }
            else
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = actionContext.Request
                    .CreateResponse(HttpStatusCode.Unauthorized);
            }
            else
            {
                string autorizationToken = actionContext.Request
                    .Headers.Authorization.Parameter;
                string decodedAuthenToken = Encoding.UTF8
                    .GetString(Convert.FromBase64String(autorizationToken));

                string[] userNamePassArray = decodedAuthenToken.Split(':');
                string userName = userNamePassArray[0];
                string password = userNamePassArray[1];

                var user = ApiLogin.SecurityEnter(userName, password);
                if (user == null)
                {
                actionContext.Response = actionContext.Request
                    .CreateResponse(HttpStatusCode.Unauthorized);
                    
                } else{
                    actionContext.Request.Properties.Add("UserId", user.Customer_id);
                    IPrincipal principal = new GenericPrincipal(
                        new GenericIdentity(userName), roles: new string[] { password });
                    Thread.CurrentPrincipal = principal;
                    HttpContext.Current.User = principal;
                } 
            }
        }
    }
}