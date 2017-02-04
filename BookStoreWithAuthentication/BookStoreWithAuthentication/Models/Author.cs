using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookStoreWithAuthentication.Models
{
    public class Author
    {
        public int ID { get; set; }
        [Display(Name = "Imię")]
        public string FirstName { get; set; }
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }
        public virtual ICollection<Book> Books { get; set; }

        public override string ToString()
        {
            return FirstName + " " + LastName;
        }
    }
}