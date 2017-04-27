using System;
using System.Threading;
using UnitTestEcho.Contracts;

namespace UnitTestEcho.Services
{
    public class StandardService : IStandardService
    {
        private static int value;

        public string GetStringValue()
        {
            return "TestValue";
        }

        public void SetValue(int x)
        {
            value = x;
        }

        public void SetValueLongRunning(int x)
        {
            // Wait 12 seconds before giving the ok.
            Thread.Sleep(12000);
        }

        public string SetValues(int x, double y, string value)
        {
            return string.Format("{0},{1},{2}", x, y, value);
        }

        public void ThrowFault()
        {
            throw new OperationCanceledException("This is an expected error.");
        }
    }
}
