using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Helpers;
using Newtonsoft.Json;

using Chat.Repositories;
using Chat.Models;
using Chat.JsonStruct;

namespace Chat.Controllers
{
    public class CheckController : ApiController
    {
        public CheckController() { }
        [HttpGet]
        public HttpResponseMessage CheckBids()
        {
            int userId = -1;
            try
            {
                var cookie = Request.Headers.GetCookies("userId")
                    .First();
                if (cookie != null)
                {
                    userId = Int32.Parse(cookie["userId"].Value);
                }

                if (userId == -1) throw new Exception();
            }
            catch (Exception) { }

            Repository repo = new Repository();
            bool bidsAreExist = repo.IfBids(userId);
            var response = new HttpResponseMessage();
            if (bidsAreExist == true)
                response.StatusCode = HttpStatusCode.OK;
            else response.StatusCode = HttpStatusCode.NoContent;
            return response;
        }
        [HttpPost]
        public List<Int32> CheckChats([FromBody] List<JsonData> body)
        {
            int userId = -1;
            try
            {
                var cookie = Request.Headers.GetCookies("userId")
                    .First();
                if (cookie != null)
                {
                    userId = Int32.Parse(cookie["userId"].Value);
                }

                if (userId == -1) throw new Exception();
            }
            catch (Exception) { }
            Repository repo = new Repository();
            var changedIds = repo.IfChatsMustUpdate(userId,body);
            return changedIds.ToList();
        }
        [HttpGet]
        public HttpResponseMessage CheckMessages(Int32 chatId, Int32 lastMsgId)
        {
            int userId = -1;
            try
            {
                var cookie = Request.Headers.GetCookies("userId")
                    .First();
                if (cookie != null)
                {
                    userId = Int32.Parse(cookie["userId"].Value);
                }

                if (userId == -1) throw new Exception();
            }
            catch (Exception){}

            Repository repo = new Repository();
            repo.ChoseChatInitialize(chatId);

            var response = new HttpResponseMessage();
            IEnumerable<Message> items;
            try
            {
                items = repo.UpdatedMsgs(chatId, lastMsgId)
                .ToList();
            }
            catch (Exception)
            {
                response.StatusCode = HttpStatusCode.NoContent;
                return response;
            }
            
            response.StatusCode = HttpStatusCode.OK;
            
            String contentFull= "";
            foreach (Message msg in items)
            {
                contentFull += @"
                <div>
                    <p>
                        From: "+msg.OwnerName +@"
                        <span> </span>
                        Time: "+msg.Time +@"
                        <input type=""hidden""
                            value="+ msg.Message_Id + @" />
                    </p>
                    <p>
                        " +msg.Text +@"
                    </p>
                 </div>
                 ";
            }
            response.Content = new StringContent(contentFull);
            return response;
        }
    }
}
