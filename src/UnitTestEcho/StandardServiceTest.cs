using Echo.Contexts;
using Echo.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTestEcho.Contracts;
using UnitTestEcho.Services;

namespace UnitTestEcho
{
    [TestClass]
    public class StandardServiceTest
    {
        private const string BaseAddress = @"Testing/StandardServiceTest";

        private static ServicesHostManager _servicesHostManager;

        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            var context = new ServicesContext(BaseAddress);
            _servicesHostManager = new ServicesHostManager(context);

            _servicesHostManager.RegisterService<IStandardService, StandardService>();
            _servicesHostManager.StartServices();
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            _servicesHostManager.StopServices();
        }

        [TestMethod]
        public void Call_Single_Operation()
        {
            var context = new ServicesContext(BaseAddress);
            var factory = context.CreateFactory<IStandardService>();

            using (var proxy = factory.Create())
            {
                var value = proxy.Channel.GetStringValue();
                Assert.AreEqual("TestValue", value);
            }
        }

        [TestMethod]
        public void Call_Single_Operation_With_Parameters()
        {
            const int Value1 = 56;
            const double Value2 = 134.11;
            const string Value3 = "Foo";

            var context = new ServicesContext(BaseAddress);
            var factory = context.CreateFactory<IStandardService>();

            using (var proxy = factory.Create())
            {
                var value = proxy.Channel.SetValues(Value1, Value2, Value3);
                Assert.AreEqual(string.Format("{0},{1},{2}", Value1, Value2, Value3), value);
            }
        }

        [TestMethod]
        public void Call_Single_Operation_LongRunning()
        {
            var context = new ServicesContext(BaseAddress);
            var factory = context.CreateFactory<IStandardService>();

            using (var proxy = factory.Create())
            {
                proxy.Channel.SetValueLongRunning(34);
            }
        }

        [TestMethod]
        public void Call_Single_Operation_OneWay()
        {
            var context = new ServicesContext(BaseAddress);
            var factory = context.CreateFactory<IStandardService>();

            using (var proxy = factory.Create())
            {
                proxy.Channel.SetValue(11);
            }
        }
    }
}
