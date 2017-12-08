using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Chat.Models
{
    public class LogModel
    {
        private String login;
        private String password;
        [Required]
        public String Login
        {
            set { login = value; }
            get { return login; }
        }
        [Required]
        public String Password
        {
            set { password = value; }
            get { return password; }
        }
    }
}