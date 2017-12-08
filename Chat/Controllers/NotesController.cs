using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

using Chat.Repositories;
using Chat.Models;

namespace Chat.Controllers
{
    [Authorize]
    public class NotesController : Controller
    {
        
        public NotesController(){}

        public ActionResult CreateChat(Int32 friend_id)
        {
            var userId = Int32.Parse(Request.Cookies["userId"].Value);
            Repository repo = new Repository();
            repo.CreateChat(userId, friend_id);
            return RedirectToAction("Index","Home");
        }
        public ActionResult LoadChat(Int32 chat_id)
        {
            var userId = Int32.Parse(Request.Cookies["userId"].Value);
            Repository repo = new Repository();
            repo.ChoseChatInitialize(chat_id);
            var items = repo.GetMessages();
            ViewData["ChatId"] = chat_id;
            return PartialView("Chat", items);
        }
        [HttpPost]
        public ActionResult WriteMessage()
        {
            int chat;
            try
            {
                Int32.TryParse(Request.Form["chat_id_curr"].ToString(), out chat);
            }
            catch (Exception){
                return new EmptyResult();
            }

            var userId = Int32.Parse(Request.Cookies["userId"].Value);
            var userName = Request.Cookies["userName"].Value;

            var msg = new Message()
            {
                Time = DateTime.Now,
                Text = Request.Form["new_message"].ToString(),
                OwnerId = userId,
                OwnerName = userName,
                Chat_Iden = chat
            };
            
            Repository repo = new Repository();
            repo.AddMessage(msg, chat);
            return PartialView("Message",msg);
        }
    }
}
