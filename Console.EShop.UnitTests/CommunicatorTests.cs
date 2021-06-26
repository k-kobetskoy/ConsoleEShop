using ConsoleEShop;
using Moq;
using NUnit.Framework;

namespace Console.EShop.UnitTests
{
    [TestFixture()]
    public class CommunicatorTests
    {
        private Communicator communicator;
        [SetUp]
        public void Setup()
        {
            var ioService = new Mock<IIOService>();
            ioService.Setup(r => r.ReadOrAbort()).Returns(string.Empty);
            communicator = new Communicator(ioService.Object);
        }

        [Test]
        public void AskForStringReturnsNullIfUserAbortedTest()
        {
            Assert.IsNull(communicator.AskForString(It.IsAny<string>(), It.IsAny<string>()));
        }

        [Test]
        public void AskForNumberReturnsExpectedValueIfUserAbortedTest()
        {
            Assert.That(communicator.AskForNumber(It.IsAny<string>()), Is.EqualTo(-1));
        }
    }
}