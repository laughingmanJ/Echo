using Microsoft.VisualStudio.TestTools.UnitTesting;
using Echo.Hosting;
using Echo.Contexts;

namespace Echo.IntegrationTests
{
    [TestClass]
    public class DiagnosticServiceTest
    {
        private const string BaseAddress = @"net.pipe://localhost/Testing/DiagnosticServiceTest/";

        private static ServicesHostManager _servicesHostManager;

        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            var context = new ServicesContext(BaseAddress);
            _servicesHostManager = new ServicesHostManager(context);
            _servicesHostManager.StartServices();
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            _servicesHostManager.StopServices();
        }

        [TestMethod]
        public void IsServicesRunning()
        {
            var context = new ServicesContext(BaseAddress);
            Assert.IsTrue(context.IsServicesRunning(), "Diagnostic service is not running.");
        }
    }
}
