using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleEShop;
using ConsoleEShop.Pages;
using Moq;
using NUnit.Framework;

namespace Console.EShop.UnitTests.Pages
{
    [TestFixture()]
    public class ProductsPageTests
    {
        private ProductsPage productsPage;
        private ConsoleEShop.EShop context;
        
        [SetUp]
        public void Setup()
        {
            
            var dataService = new Mock<IDataService>();
            var client = new Mock<IClient>();
            productsPage = new ProductsPage( dataService.Object, client.Object);
            context = new ConsoleEShop.EShop( dataService.Object, client.Object);
        }

        [Test]
        [TestCase(new[]{ "register", "login", "product", "products"}, Roles.Guest)]
        [TestCase(new[] { "product", "products", "cart", "orders", "logout", "user info", "byu" }, Roles.RegisteredUser)]
        [TestCase(new[] { "product", "products", "cart", "orders", "logout", "user info", "m users", "m orders", "m products", "byu" }, Roles.Administrator)]
        public void SetCommands_ReturnsCorrectCommands_Test(string[] expectedCommands, Roles role)
        {
            context.CurrentUser = new User() {Role = role};
            productsPage.SetContext(context);
            

            var keys = productsPage.Commands.Keys.ToList();
            
            
            Assert.AreEqual(keys, expectedCommands);
        }

        [Test]
        [TestCase("TestName-1", "TestDescription-1")]
        [TestCase("TestName-2", "TestDescription-1")]
        public void ShowPageData_ReturnsCorrectData_Test(string productName, string productDescription)
        {
            var dataService = new Mock<IDataService>();
            dataService.Setup(d => d.GetProducts())
                .Returns(GetTestProducts());
            var client = new Mock<IClient>();


           
            var pPage = new ProductsPage(dataService.Object, client.Object);
            var prodViewResult = pPage.ShowPageData().ShowViewData();

            StringAssert.Contains("TestName-1", prodViewResult);
        }

        private IEnumerable<Product> GetTestProducts()
        {
            return new List<Product>()
            {
                new Product() {Name = "TestName-1", CategoryId = 1, Description = "TestDescription-1", Price = 1},
                new Product() {Name = "TestName-2", CategoryId = 2, Description = "TestDescription-1", Price = 2}
            };
        }
    }
}