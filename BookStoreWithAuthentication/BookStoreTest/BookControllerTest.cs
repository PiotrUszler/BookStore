using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookStoreWithAuthentication.Controllers;
using System.Web.Mvc;

namespace BookStoreTest
{
    [TestClass]
    public class BookControllerTest
    {
        [TestMethod]
        public void TestDetails()
        {
            var controller = new BookController();
            var result = controller.Details(5) as ViewResult;
            Assert.AreEqual("Details", result.ViewName);
        }
    }
}
