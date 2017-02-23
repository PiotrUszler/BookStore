using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BookStoreWithAuthentication.Models
{
    public class Rating
    {
        public int ID { get; set; }

        [Display(Name = "Ocena"), Required, Range(1,5)]
        public int Rate { get; set; }

        [Display(Name = "Użytkownik"), Required, ScaffoldColumn(false)]
        public string User { get; set; }

        [Display(Name = "Ksiażka")]
        public int BookId { get; set; }
        
        public virtual Book book { get; set; }
    }
}