using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Echo.Interfaces
{
    public interface IClientContext
    {
        /// <summary>
        /// Checks to see if Services are running.
        /// </summary>
        /// <returns>Value indicating that services are up and running.</returns>
        bool IsServicesRunning();

        IClientFactory<T> CreateFactory<T>()
            where T : class;

        IClientFactory<T> CreateDuplexFactory<T, TCallback>(TCallback callback)
            where T : class
            where TCallback : class;
    }
}
