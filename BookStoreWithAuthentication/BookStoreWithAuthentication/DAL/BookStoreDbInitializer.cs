using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using BookStoreWithAuthentication.Models;

namespace BookStoreWithAuthentication.DAL
{
    public class BookStoreDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var categories = new List<Category>
            {
                new Category {Name = "Fantasy" },
                new Category {Name = "Literatura Piękna" },
                new Category {Name = "Przygodowa" },
                new Category {Name = "Kryminał" }
            };
            categories.ForEach(c => context.Categories.Add(c));
            context.SaveChanges();

            var series = new List<Series>
            {
                new Series {Name = "Harry Potter", Books = new List<Book>() },
                new Series {Name = "Wiedźmin", Books = new List<Book>() },
                new Series {Name = "Przygody Tomka Wilmowskiego", Books = new List<Book>()},
                new Series {Name = "Sherlock Holmes", Books = new List<Book>() }
            };
            series.ForEach(s => context.Series.Add(s));
            context.SaveChanges();

            var publishers = new List<Publisher>
            {
                new Publisher {Name = "Media Rodzina" },
                new Publisher {Name = "SuperNOWA" },
                new Publisher {Name = "Muza" },
                new Publisher {Name = "Prószyński i S-ka" },
                new Publisher {Name = "Algo" }
            };
            publishers.ForEach(p => context.Publishers.Add(p));
            context.SaveChanges();

            var books = new List<Book>
            {
                new Book {Title = "Harry Potter i kamień filozoficzny",NumOfPages = 320 ,CategoryID =  categories.Single(c => c.Name == "Fantasy").ID, Price = 30.00M,
                    Authors = new List<Author>(), SeriesID = series.Single(s => s.Name == "Harry Potter").ID, PublisherID = publishers.Single(p => p.Name == "Media Rodzina").ID},
                new Book {Title = "Harry Potter i komnata tajemnic", NumOfPages = 390, CategoryID = categories.Single(c => c.Name == "Fantasy").ID, Price = 32.00M,
                    Authors = new List<Author>(), SeriesID = series.Single(s => s.Name == "Harry Potter").ID, PublisherID = publishers.Single(p => p.Name == "Media Rodzina").ID},
                new Book {Title = "Harry Potter i więzień azkabanu", NumOfPages = 425, CategoryID = categories.Single(c => c.Name == "Fantasy").ID, Price = 35.00M,
                    Authors = new List<Author>(), SeriesID = series.Single(s => s.Name == "Harry Potter").ID, PublisherID = publishers.Single(p => p.Name == "Media Rodzina").ID},

                new Book {Title = "Ostatnie życzenie", NumOfPages = 285 , CategoryID = categories.Single(s => s.Name == "Fantasy").ID, Price = 35.00M,
                    Authors = new List<Author>(), SeriesID = series.Single(s => s.Name == "Wiedźmin").ID, PublisherID = publishers.Single(p => p.Name == "SuperNOWA").ID },
                new Book {Title = "Miecz przeznaczenia", NumOfPages = 342 , CategoryID = categories.Single(s => s.Name == "Fantasy").ID, Price = 36.00M,
                    Authors = new List<Author>(), SeriesID = series.Single(s => s.Name == "Wiedźmin").ID, PublisherID = publishers.Single(p => p.Name == "SuperNOWA").ID },
                new Book {Title = "Czas pogardy", NumOfPages = 327 , CategoryID = categories.Single(s => s.Name == "Fantasy").ID, Price = 36.00M,
                    Authors = new List<Author>(), SeriesID = series.Single(s => s.Name == "Wiedźmin").ID, PublisherID = publishers.Single(p => p.Name == "SuperNOWA").ID },
                new Book {Title = "Krew elfów", NumOfPages = 285 , CategoryID = categories.Single(s => s.Name == "Fantasy").ID, Price = 39.00M,
                    Authors = new List<Author>(), SeriesID = series.Single(s => s.Name == "Wiedźmin").ID, PublisherID = publishers.Single(p => p.Name == "SuperNOWA").ID },

                new Book {Title = "Tomek w krainie kangurów", NumOfPages = 270 , CategoryID = categories.Single(s => s.Name == "Przygodowa").ID, Price = 25.00M,
                    Authors = new List<Author>(), SeriesID = series.Single(s => s.Name == "Przygody Tomka Wilmowskiego").ID, PublisherID = publishers.Single(p => p.Name == "Muza").ID },
                new Book {Title = "Tomek na czarnym lądzie", NumOfPages = 321 , CategoryID = categories.Single(s => s.Name == "Przygodowa").ID, Price = 25.00M,
                    Authors = new List<Author>(), SeriesID = series.Single(s => s.Name == "Przygody Tomka Wilmowskiego").ID, PublisherID = publishers.Single(p => p.Name == "Muza").ID },
                new Book {Title = "Tomek na wojennej ścieżce", NumOfPages = 261 , CategoryID = categories.Single(s => s.Name == "Przygodowa").ID, Price = 25.00M,
                    Authors = new List<Author>(), SeriesID = series.Single(s => s.Name == "Przygody Tomka Wilmowskiego").ID, PublisherID = publishers.Single(p => p.Name == "Muza").ID },

                new Book {Title = "Studium w szkarłacie", NumOfPages = 270, CategoryID = categories.Single(c => c.Name == "Kryminał").ID, Price = 30.00M,
                    ImagePath = "Holmes-Studium_w_szkarlacie.jpg",
                    Authors = new List<Author>(), SeriesID = series.Single(s => s.Name == "Sherlock Holmes").ID, PublisherID = publishers.Single(p => p.Name == "Prószyński i S-ka").ID},
                new Book {Title = "Znak czterech", NumOfPages = 380, CategoryID = categories.Single(c => c.Name == "Kryminał").ID, Price = 30.00M,
                    ImagePath = "Holmes-Znak_czterech.jpg",
                    Authors = new List<Author>(), SeriesID = series.Single(s => s.Name == "Sherlock Holmes").ID, PublisherID = publishers.Single(p => p.Name == "Algo").ID},

            };
            books.ForEach(b => context.Books.Add(b));
            context.SaveChanges();



            var authors = new List<Author>
            {
                new Author {FirstName = "J.K", LastName = "Rowling", Books = new List<Book>() },
                new Author {FirstName = "Andrzej", LastName = "Sapkowski", Books = new List<Book>() },
                new Author {FirstName = "Alfred", LastName = "Szklarski", Books = new List<Book>() },
                new Author {FirstName = "Arthur Conan", LastName = "Doyle", Books = new List<Book>() }
            };
            authors.ForEach(a => context.Authors.Add(a));
            context.SaveChanges();


            AddOrUpdateAuthors(context, "Harry Potter i kamień filozoficzny", "Rowling");
            AddOrUpdateAuthors(context, "Harry Potter i kamień filozoficzny", "Sapkowski");
            AddOrUpdateAuthors(context, "Harry Potter i komnata tajemnic", "Rowling");
            AddOrUpdateAuthors(context, "Harry Potter i więzień azkabanu", "Rowling");
            AddOrUpdateAuthors(context, "Ostatnie życzenie", "Sapkowski");
            AddOrUpdateAuthors(context, "Miecz przeznaczenia", "Sapkowski");
            AddOrUpdateAuthors(context, "Czas pogardy", "Sapkowski");
            AddOrUpdateAuthors(context, "Krew elfów", "Sapkowski");
            AddOrUpdateAuthors(context, "Tomek w krainie kangurów", "Szklarski");
            AddOrUpdateAuthors(context, "Tomek na czarnym lądzie", "Szklarski");
            AddOrUpdateAuthors(context, "Tomek na wojennej ścieżce", "Szklarski");
            AddOrUpdateAuthors(context, "Studium w szkarłacie", "Doyle");
            AddOrUpdateAuthors(context, "Znak czterech", "Doyle");

            context.SaveChanges();

        }
        void AddOrUpdateAuthors(ApplicationDbContext context, string bookTitle, string authorLastName)
        {
            var book = context.Books.SingleOrDefault(b => b.Title == bookTitle);
            var author = book.Authors.SingleOrDefault(a => a.LastName == authorLastName);
            if (author == null)
            {
                book.Authors.Add(context.Authors.Single(a => a.LastName == authorLastName));
            }
        }
    }
}