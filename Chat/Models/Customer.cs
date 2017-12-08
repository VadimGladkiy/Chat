using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq.Mapping;

namespace Chat.Models
{
    [Table(Name = "Customers")]
    public class Customer
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        [ScaffoldColumn(false)]
        public Int32 Customer_id { set; get; }

        [Column]
        [Required, MaxLength(64, ErrorMessage ="Не больше 64")]
        [Display(Name ="Полное имя")]
        public String FullName { set; get; }

        [Column]
        [MaxLength(40, ErrorMessage ="Не больше 40 символов")]
        [RegularExpression(@"[\w\d]+@[\w\d]+[\.][\w\d]{2,3}", ErrorMessage = "Некорректный адрес")]
        [DataType(DataType.EmailAddress)]
        public String Email { set; get; }

        [Column]
        [Required, StringLength(20, ErrorMessage = "Не больше 20 символов")]
        public String Login { set; get; }

        [Column]
        [Required, StringLength(12,MinimumLength=6, ErrorMessage ="Минимум 6 символов, максимум 12")]
        [DataType(DataType.Password)]
        public String Password { set; get; }

        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}