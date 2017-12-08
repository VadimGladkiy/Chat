using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.Models
{
    public class Friend
    {
        public Int32 FriendId { set; get; }
        public String FullName { set; get; }
        public String Email { set; get; }
        public String Login { set; get; }
        public bool IsFriendAccepted { set; get; }
    }
}