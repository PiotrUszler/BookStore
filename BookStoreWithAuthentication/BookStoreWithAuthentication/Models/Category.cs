﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookStoreWithAuthentication.Models
{
    public class Category
    {
        public int ID { get; set; }
        [Display(Name = "Kategoria")]
        public string Name { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}