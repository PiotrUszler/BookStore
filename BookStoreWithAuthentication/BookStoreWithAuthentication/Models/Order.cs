using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStoreWithAuthentication.Models
{
    public enum OrderStatus
    {
        IN_PROGRESS,
        READY_TO_SEND,
        SENT,
        CANCELED
    }

    public static class OrderStatusHelper
    {
        public static string GetName(this OrderStatus status)
        {
            switch (status)
            {
                case OrderStatus.IN_PROGRESS:
                    return "W realizacji";
                case OrderStatus.READY_TO_SEND:
                    return "Gotowe do wysłania";
                case OrderStatus.SENT:
                    return "Wysłano";
                case OrderStatus.CANCELED:
                    return "Anulowano";
                default:
                    return String.Empty;
            }
        }
    }

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

        public OrderStatus Status { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }

        public Order()
        {
            this.Status = OrderStatus.IN_PROGRESS;
        }

    }
}