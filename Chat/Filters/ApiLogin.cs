using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Linq;
using System.Web;
using Chat.Models;

namespace Chat.Filters
{
    public static class ApiLogin
    {
        private static readonly String connectionString =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public static Customer SecurityEnter(String userLogin, String userPassword)
        {
            DataContext db = new DataContext(connectionString);
            var userFound = db.GetTable<Customer>().Where(x => x.Login == userLogin &&
                x.Password == userPassword).OrderBy(x => x.Login).Take(1).FirstOrDefault();
            return userFound;         
        }
        public static Int32 GetUserId(Customer CustFound)
        {
            return CustFound.Customer_id;
        }
    }
    
}