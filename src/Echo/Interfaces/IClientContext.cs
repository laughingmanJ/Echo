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

        /// <summary>
        /// Creates a service proxy factory for a service contract.
        /// </summary>
        /// <typeparam name="TContract">Service contract.</typeparam>
        /// <returns>Service proxy factory/returns>
        IClientFactory<TContract> CreateFactory<TContract>()
            where TContract : class;

        IClientFactory<TContract> CreateDuplexFactory<TContract, TCallback>(TCallback callback)
            where TContract : class
            where TCallback : class;
    }
}
