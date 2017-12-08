using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Chat.Models;
using Chat.Repositories;

namespace Chat.Controllers
{
    [Authorize]
    public class FriendController : Controller
    {
        // ctor
        public FriendController(){}

        public ActionResult FriendList()
        {
            var userId = Int32.Parse(Request.Cookies["userId"].Value);

            Repository repo = new Repository();
            repo.SetFriendList(userId);
            var friends = repo.GetFriends();
            return View(friends);
        }
        [HttpPost]
        public ActionResult SearchFriend(String persName)
        {
            var userId = Int32.Parse(Request.Cookies["userId"].Value);
            Repository repo = new Repository();
            List<Models.Friend> foundPeople = repo.GetFoundPeople(persName);
            repo.SetFriendList(userId);
            var friends = repo.GetFriends();

            if(friends != null)
            foreach(Friend foundPerson in foundPeople)
            {
                int? Equal = friends.Select(x => x.FriendId)
                    .Where(y => y == foundPerson.FriendId)
                    .FirstOrDefault();
                if (Equal != null) foundPerson.IsFriendAccepted = true;
            }
            return PartialView("FoundPeople", foundPeople);
        }
        public ActionResult SetContact(Int32 cont_id)
        {
            var userId = Int32.Parse(Request.Cookies["userId"].Value);
            Repository repo = new Repository();
            repo.BidToPerson(userId, cont_id);
            return RedirectToAction("FriendList"); 
        }
        public ActionResult Bids()
        {
            var userId = Int32.Parse(Request.Cookies["userId"].Value);
            
            Repository repo = new Repository();
            repo.SetBidsList(userId);
            var bids = repo.GetFriends();
            return View(bids);
        }
        public ActionResult AcceptBid(Int32 applicant_id)
        {
            var userId = Int32.Parse(Request.Cookies["userId"].Value);
            Repository repo = new Repository();
            repo.AcceptBid(userId , applicant_id);
            return RedirectToAction("Bids");
        }
        public ActionResult DenyBid(Int32 applicant_id)
        {
            var userId = Int32.Parse(Request.Cookies["userId"].Value);
            Repository repo = new Repository();
            repo.DenyBid(userId, applicant_id);
            return RedirectToAction("Bids");
        }
        public ActionResult RemoveFriend(Int32 comrade_id)
        {
            var userId = Int32.Parse(Request.Cookies["userId"].Value);
            Repository repo = new Repository();
            repo.RemoveFriend(userId, comrade_id);
            return RedirectToAction("FriendList");
        }
    }
}