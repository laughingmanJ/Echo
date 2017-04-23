using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Echo.Interfaces;
using Moq;
using System.ServiceModel.Channels;

namespace UnitTestEcho.Factories
{
    [TestClass]
    public class ClientFactoryTest
    {
        [TestMethod]
        public void Create_ServiceProxy()
        {
            var bindingMock = new Mock<Binding>();
            

            var contextMock = new Mock<IServicesContext>();

            contextMock
                .Setup(mock => mock.CreateClientBinding())
                .Returns(bindingMock.Object);
        }
    }
}
