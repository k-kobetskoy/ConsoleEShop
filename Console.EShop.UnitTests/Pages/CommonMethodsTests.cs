using System;
using System.Collections.Generic;
using ConsoleEShop;
using ConsoleEShop.Pages;
using Moq;
using NUnit.Framework;

namespace Console.EShop.UnitTests.Pages
{
    [TestFixture()]
    public class CommonMethodsTests
    {

        private HomePage page;
        private ProductsPage pPage;

        [SetUp]
        public void Setup()
        {


        }

        [Test]
        public void Login_WithWrongUserName_WorksCorrect_Test()
        {
            const string input = "Wrong login";
            const string expected = "No users with this login";

            var ioService = new Mock<IIOService>();
            var dataService = new Mock<IDataService>();
            var client = new Mock<IClient>();

            dataService.Setup(r => r.GetUserByName(input))
                .Returns((User)null);
            client.Setup(c => c.AskForString(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(input);

            page = new HomePage(ioService.Object, dataService.Object, client.Object);

            var actual = page.Login();
            StringAssert.Contains(expected, actual);
        }

        [Test]
        public void Login_Abort_WorksCorrect_Test()
        {
            const string input = (string)null;
            const string expected = "Operation was canceled";

            var ioService = new Mock<IIOService>();
            var dataService = new Mock<IDataService>();
            var client = new Mock<IClient>();

            client.Setup(c => c.AskForString(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(input);

            page = new HomePage(ioService.Object, dataService.Object, client.Object);

            var actual = page.Login();
            StringAssert.Contains(expected, actual);
        }

        
        [Test]
        public void Register_Abort_WorksCorrect_Test()
        {
            const string input = (string)null;
            const string expected = "Operation was canceled";
            const string key = "TestKey";

            var ioService = new Mock<IIOService>();
            var dataService = new Mock<IDataService>();
            var client = new Mock<IClient>();

            client.Setup(c => c.AskForString(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(input);


            pPage = new ProductsPage(ioService.Object, dataService.Object, client.Object);
            pPage.Commands = new Dictionary<string, Func<string>>() {{key, It.IsAny<Func<string>>()}};
            var actual = pPage.Register();
            StringAssert.Contains(expected, actual);
        }
        [Test]
        public void SetQuantity_Abort_WorksCorrect_Test()
        {
            const int input = -1;

            const string expected = "Operation was canceled";

            var ioService = new Mock<IIOService>();
            var dataService = new Mock<IDataService>();
            var client = new Mock<IClient>();

            client.Setup(c => c.AskForNumber(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(input);

            pPage = new ProductsPage(ioService.Object, dataService.Object, client.Object);
            pPage.Commands = It.IsAny<Dictionary<string, Func<string>>>();
            var actual = pPage.SetQuantity(new Product());
            StringAssert.Contains(expected, actual);
        }

        [Test]
        public void ShowProductPage_Abort_ReturnsCorrectResult_Test()
        {
            const string input = (string)null;
            const string expected = "Operation was canceled";

            var ioService = new Mock<IIOService>();
            var dataService = new Mock<IDataService>();
            var client = new Mock<IClient>();

            client.Setup(c => c.AskForString(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(input);


            pPage = new ProductsPage(ioService.Object, dataService.Object, client.Object);
            pPage.Commands = It.IsAny<Dictionary<string, Func<string>>>();
            var actual = pPage.ShowProductPage();

            StringAssert.Contains(expected, actual);
        }


        [Test]
        public void ShowProductPage_WithInCorrectProductName_ReturnsCorrectResult_Test()
        {
            const string input = "WrongTestProductName";
     
            const string expected = "Can't find this product";

            var ioService = new Mock<IIOService>();
            var dataService = new Mock<IDataService>();
            var client = new Mock<IClient>();
            var context = new Mock<IEShop>();

            context.Setup(c => c.SetCurrentPage(It.IsAny<IPage>()));


            dataService.Setup(d => d.GetProductByName(input))
                .Returns((Product)null);


            pPage = new ProductsPage(ioService.Object, dataService.Object, client.Object)
            {
                Commands = It.IsAny<Dictionary<string, Func<string>>>()
            };

            var actual = pPage.ShowProductPage(input);

            StringAssert.Contains(expected, actual);
        }



    }
}