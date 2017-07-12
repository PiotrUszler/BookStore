using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookStoreWithAuthentication.Controllers;
using BookStoreWithAuthentication.DAL;
using BookStoreWithAuthentication.Models;
using System.Web.Mvc;
using Moq;
using System.Data.Entity;
using Microsoft.CSharp;


namespace BookStoreTest
{
    [TestClass]
    public class BookControllerTest
    {
        [TestMethod]
        public void index_default_viewBag()
        {
           // Mock<IBookRepository> mock = new Mock<IBookRepository>();
           // mock.Setup(m => m.GetBooks()).Returns(new Book[] {
           //     new Book { ID = 1, Title = "Harry Potter i kamień filozoficzny", NumOfPages = 320, Price = 30, ImagePath = "no_cover.png", CategoryID = 1, PublisherID = 1, SeriesID = 1 }
           // }.AsEnumerable());
            var controller = new BookController();
            var result = controller.Index(null, null, null,null, null) as ViewResult;
            Assert.AreEqual("title_desc", result.ViewBag.NameSortParam);
        }

        [TestMethod]
        public void getting_book_rating()
        {
            Mock<IUnitOfWork> unitRepo = new Mock<IUnitOfWork>();
            Mock<IGenericRepository<Book>> bookRepo = new Mock<IGenericRepository<Book>>();           
            bookRepo.Setup(m => m.GetByID(1)).Returns(
               new Book { ID = 1, Title = "Harry Potter i kamień filozoficzny", NumOfPages = 320, Price = 30, ImagePath = "no_cover.png", CategoryID = 1, PublisherID = 1, SeriesID = 1 });
            unitRepo.Setup(u => u.BookRepository).Returns(bookRepo.Object);

            Rating rating = new Rating { BookId = 1, Rate = 3, User = "admin" };

            var controller = new BookController();
            var result = controller.RateBook(1, 3);
            Assert.AreEqual("Rating saved", result);
        }

        [TestMethod]
        public void details_test()
        {
            var unit = new Mock<IUnitOfWork>();
            var generic = new Mock<IGenericRepository<Book>>();
            var book = new Book { ID = 1, Title = "Harry Potter i kamień filozoficzny", NumOfPages = 320, Price = 30, ImagePath = "no_cover.png", CategoryID = 1, PublisherID = 1, SeriesID = 1 };

            generic.Setup(x => x.GetByID(It.IsAny<int>())).Returns(book);
            unit.Setup(x => x.BookRepository).Returns(generic.Object);
            unit.Setup(x => x.BookRepository.GetByID(It.IsAny<int>())).Returns(book);


            var controller = new BookController();
            var result = controller.Details(1);
            Assert.IsNotNull(result);
        }

    }
}
