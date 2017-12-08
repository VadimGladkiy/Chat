using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq.Mapping;

namespace Chat.Models
{
    [Table(Name = "CustomersChats")]
    public class Conversation
    {
        [Column(IsPrimaryKey =true, IsDbGenerated = true)]
        public Int32 Conversation_Id { set; get; }
        [Column(Name ="Customer_Identifier")]
        public Int32 Customer_Identifier { get; set; }
        [Column(Name ="Chat_Identifier")]
        public Int32 Chat_Identifier { get; set; }
    }
}