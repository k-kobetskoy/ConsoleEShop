using NUnit.Framework;
using ConsoleEShop;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;

namespace ConsoleEShop.Tests
{
    [TestFixture]
    public class EShopTests
    {
        private EShop eShop;
        [SetUp]
        public void Setup()
        {
            var ioService = new Mock<IIOService>();
            var dataService = new Mock<IDataService>();
            var client = new Mock<IClient>();

            eShop = new EShop(ioService.Object, dataService.Object, client.Object);
        }

        [Test]
        public void SetCartReturnsNewCartTest()
        {
            eShop.SetCart();
            Assert.That(eShop.Cart, Is.Not.Null);
        }

        [Test]
        public void SetCurrentPageThrowsExceptionIfArgumentIsNullTest()
        {

            Assert.Throws<ArgumentNullException>(() => eShop.SetCurrentPage(null));
        }

        [Test]
        public void SetCurrentUserWithNoArgumentsReturnsGuestUserTest()
        {
            eShop.SetCurrentUser();
            var user = eShop.CurrentUser;

            Assert.That(user.Role, Is.EqualTo(Roles.Guest));
            Assert.That(user.Name, Is.Null);
            Assert.That(user.Password, Is.Null);
        }
        [Test]
        public void SetCurrentUserSetsCorrectUserTest()
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

    }
}