using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Chat.Models;
using Chat.Filters;
using System.Web.Security;
using System.Net.Http.Headers;
using System.IO;
using System.Web;
using System.Threading;


namespace Chat.Controllers
{
    public class HeaderController : ApiController
    {
        static String serverRoot;
        static Uri url;
        static HeaderController()
        {
            serverRoot = HttpContext.Current.Server.MapPath("~/Views/Shared/_Login.cshtml");
            url = HttpContext.Current.Request.Url;
        }
        public HeaderController(){}

        [HttpGet]
        public HttpResponseMessage GetLogin()
        {
            var response = new HttpResponseMessage();
            if (!User.Identity.IsAuthenticated){
                response.StatusCode = HttpStatusCode.PartialContent;
                StreamReader stream = new StreamReader(serverRoot);
                response.Content = new StringContent(stream.ReadToEnd());
                stream.Close();
            }
            else {
                response.StatusCode = HttpStatusCode.OK;
                response.Content = new StringContent("<div>You are logged in as "+
                    User.Identity.Name +"</div>");
            }
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }
        [HttpPost]
        public HttpResponseMessage PostLogin([FromBody] LogModel logModel)
        {
            var response = new HttpResponseMessage();
            if (ModelState.IsValid)
            {
                var userFound = ApiLogin.SecurityEnter(logModel.Login, logModel.Password);
                if (userFound != null)
                {
                    FormsAuthentication.SetAuthCookie(userFound.FullName, true);

                    var cookieId = new CookieHeaderValue("userId",
                        userFound.Customer_id.ToString()); 
                    cookieId.Expires = DateTimeOffset.Now.AddDays(2); 
                    cookieId.Domain = Request.RequestUri.Host; 
                    cookieId.Path = "/"; // путь куки

                    var cookieName = new CookieHeaderValue("userName",
                        userFound.FullName.ToString());
                    cookieName.Expires = DateTimeOffset.Now.AddDays(2);
                    cookieName.Domain = Request.RequestUri.Host;
                    cookieName.Path = "/"; 

                    response.StatusCode = HttpStatusCode.PartialContent;
                    
                    response.Headers.AddCookies(new CookieHeaderValue[]
                    { cookieId, cookieName });

                    response.Content = new StringContent("<div>You are logged in as "
                        + userFound.FullName + "</div>"); 
                }
                else
                {
                    response.StatusCode = HttpStatusCode.Unauthorized;
                    response.Content = new StringContent("<div>Unauthorized</div>");
                }
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            }
            return response;
        }
        
        [HttpGet]
        [MyAuthorize]
        [Route("api/Header/LogOff")]
        public HttpResponseMessage LogOff()
        {
            FormsAuthentication.SignOut();
            
            var response = new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.Moved;
            response.Headers.Location = new Uri("http://"+ url.Authority + "/Home/Index");
            return response;
        }
    }
}
