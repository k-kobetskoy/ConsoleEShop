using System;
using ConsoleEShop;
using ConsoleEShop.Pages;
using Moq;
using NUnit.Framework;

namespace Console.EShop.UnitTests
{
    [TestFixture]
    public class EShopTests
    {
        private ConsoleEShop.EShop eShop;
        
        [SetUp]
        public void Setup()
        {
            var ioService = new Mock<IIOService>();
            var dataService = new Mock<IDataService>();
            var client = new Mock<IClient>();

           

            eShop = new ConsoleEShop.EShop(ioService.Object, dataService.Object, client.Object);
        }

        [Test]
        public void SetCart_ReturnsNewCart_Test()
        {
            eShop.SetCart();
            Assert.That(eShop.Cart, Is.Not.Null);
        }

        [Test]
        public void SetCurrentPage_ThrowsExceptionIfArgumentIsNull_Test()
        {
            Assert.Throws<ArgumentNullException>(() => eShop.SetCurrentPage(null));
        }

        [Test]
        public void SetCurrentPage_WorksCorrect_Test()
        {
            var productPage = new Mock<IPage>();
            productPage.SetupProperty()


        }



        [Test]
        public void SetCurrentUserWithNoArguments_ReturnsGuestUser_Test()
        {
            eShop.SetCurrentUser();
            var user = eShop.CurrentUser;

            Assert.That(user.Role, Is.EqualTo(Roles.Guest));
            Assert.That(user.Name, Is.Null);
            Assert.That(user.Password, Is.Null);
        }
        [Test]
        public void SetCurrentUser_SetsCorrectUser_Test()
        {
            const string expectedUserName = "MockUser";
            const Roles expectedUserRole = Roles.Administrator;
            const string expectedUserPassword = "123";
            var expectedUser = new User
            {
                Name = expectedUserName,
                Role = expectedUserRole,
                Password = expectedUserPassword
            };

            eShop.SetCurrentUser(expectedUser);
            var user = eShop.CurrentUser;
            Assert.Multiple(() =>
            {
                Assert.That(user.Role, Is.EqualTo(expectedUserRole));
                Assert.That(user.Name, Is.EqualTo(expectedUserName));
                Assert.That(user.Password, Is.EqualTo(expectedUserPassword));
            });
        }


        [Test]
        public void SetCurrentUser_ClearsCart_Test()
        {

            eShop.SetCart();
            var cart = eShop.Cart;

            const string expectedUserName = "MockUser";
            const Roles expectedUserRole = Roles.Administrator;
            const string expectedUserPassword = "123";
            var user = new User
            {
                Name = expectedUserName,
                Role = expectedUserRole,
                Password = expectedUserPassword
            };

            eShop.SetCurrentUser(user);

            var newCart = eShop.Cart;

            Assert.AreNotEqual(cart, newCart);
        }

    }
}