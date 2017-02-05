using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStoreWithAuthentication.Models
{
    [Bind(Exclude = "OrderId")]
    public partial class Order
    {
        [ScaffoldColumn(false)]
        public int OrderId { get; set; }

        [ScaffoldColumn(false)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Imię jest wymagane")]
        [Display(Name = "Imię")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        [Display(Name = "Nazwisko")]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Adres jest wymagany")]
        [Display(Name = "Adres")]
        [StringLength(80)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Miasto jest wymagane")]
        [Display(Name = "Miasto")]
        [StringLength(50)]
        public string City { get; set; }
        
        [Required(ErrorMessage = "Kod pocztowy jest wymagany")]
        [Display(Name = "Kod pocztowy")]
        [StringLength(10)]
        public string PostalCode { get; set; }

        [Display(Name = "Numer telefonu")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Email jest wymagany")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Email jest niepoprawny")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [ScaffoldColumn(false)]
        public decimal Total { get; set; }

        [ScaffoldColumn(false)]
        public System.DateTime OrderDate { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }
    }
}