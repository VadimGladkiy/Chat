using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Threading;
using Microsoft.AspNet.Identity;
using Chat.Models;
using Chat.Repositories;
using Chat.JsonStruct;
using Newtonsoft.Json;


namespace Chat.Controllers
{
    public class HomeController : Controller
    {
        static HomeController() { }
        
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            // check if TempData contains some error message and
            // if yes add to the model state.
            if (TempData["ServerValidError"] != null)
            {
                ModelState.AddModelError(string.Empty, TempData["ServerValidError"].ToString());
            }
            return View();
        }

        [HttpPost]
        public ActionResult Register(Customer customer)
        {
            if (ModelState.IsValid)
            {
                Repository repo = new Repository();
                // создаем нового пользователя
                Customer user = new Customer
                {
                    FullName = customer.FullName,
                    Email = customer.Email,
                    Login = customer.Login,
                    Password = customer.Password
                };
                // добавляем его в таблицу Customers
                repo.InsertCustomer(user);
                return View("RegisterSuccess");
            }
            else
            {
                // since you are redirecting store the error message in TempData
                TempData["ServerValidError"] = "The server validation did not pass your data";
                return RedirectToAction("Register", "Home");
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult IsAuthorizedUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                Int32 userId = -1;
                try
                {
                    userId = Int32.Parse(Request.Cookies["userId"].Value);
                    Session["UserId"] = userId;
                }
                catch (Exception){}

                Repository repo = new Repository();
                repo.ChatsInitialize(userId);
                var items = repo.GetCurrentChats();
                if (items == null) return PartialView("Chats");

                List<JsonData> chatsInfo = new List<JsonData>();
                foreach (var ch in items)
                {
                    JsonData obj = new JsonData();
                    obj.Id = ch.Chat_Id;
                    obj.Time = ch.Late_Update.ToString();
                    chatsInfo.Add(obj);
                }
                
                // 
                var JsonSettings = new JsonSerializerSettings();
                JsonSettings.Formatting = Formatting.None; 
                    JsonSettings.ContractResolver =
                    new Newtonsoft.Json.Serialization
                    .CamelCasePropertyNamesContractResolver();
                var chats_json = 
                    JsonConvert.SerializeObject(chatsInfo, JsonSettings);
                
                ViewData["chats_json"] = chats_json;
                return PartialView("Chats", items);
            }
            return new HttpUnauthorizedResult();
        }
    }
}