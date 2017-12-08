using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq.Mapping;

namespace Chat.Models
{
    [Table(Name = "Messages")]
    public class Message
    {
        [Column(IsPrimaryKey = true,IsDbGenerated =true)]
        public Int32 Message_Id { set; get; }

        [Column]
        public Int32 Chat_Iden { set; get; }
        [Column]
        public DateTime Time { set; get; }
        [Required]
        [StringLength(512,ErrorMessage = "не больше 512 символов")]
        [Column]
        public String Text { set; get; }
        [Column]
        public String OwnerName { set; get; }
        [Column]
        public Int32 OwnerId { set; get; } 
    }
}