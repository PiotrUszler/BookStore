using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BookStoreWithAuthentication.Models
{
    public class Book
    {
        public int ID { get; set; }
        [Display(Name = "Tytuł")]
        public string Title { get; set; }
        [Display(Name = "Ilość stron")]
        public int NumOfPages { get; set; }
        [Display(Name = "Cena")]
        public decimal Price { get; set; }
        public string ImagePath { get; set; }
        [Display(Name = "Kategoria")]
        public int CategoryID { get; set; }
        [Display(Name = "Wydawca")]
        public int PublisherID { get; set; }
        [Display(Name = "Seria")]
        public int? SeriesID { get; set; }
        public virtual Category Category { get; set; }
        public virtual Publisher Publisher { get; set; }
        public virtual Series Series { get; set; }
        [Display(Name = "Autor")]
        public virtual ICollection<Author> Authors { get; set; }

        public string AuthorsToString()
        {
            return String.Join(",", Authors);
        }

        public Book()
        {
            this.ImagePath = "no_cover.png";
        }
    }
}