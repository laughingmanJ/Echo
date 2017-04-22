using System.ServiceModel.Channels;

namespace Echo.Interfaces
{
    /// <summary>
    /// Context for service bindings and addresses.
    /// </summary>
    public interface IServicesContext
    {
        #region Methods

        /// <summary>
        /// Gets the named pipe address of a service.
        /// </summary>
        /// <typeparam name="T">A service contract.</typeparam>
        /// <returns>Address of service.</returns>
        string GetServiceAddress<T>();

        /// <summary>
        /// Checks to see if Services are running.
        /// </summary>
        /// <returns>Value indicating that services are up and running.</returns>
        bool IsServicesRunning();

        /// <summary>
        /// Creates a WCF client binding based on the CWDS services configuration.
        /// </summary>
        /// <returns>WCF Binding.</returns>
        Binding CreateClientBinding();

        /// <summary>
        /// Creates a WCF streaming service binding based on the CWDS services configuration.
        /// </summary>
        /// <returns>WCF Binding.</returns>
        Binding CreateStreamingServiceBinding();

        /// <summary>
        /// Creates a WCF streaming client binding based on the CWDS services configuration.
        /// </summary>
        /// <returns>WCF Binding.</returns>
        Binding CreateStreamingClientBinding();

        /// <summary>
        /// Creates a WCF service binding based on the CWDS services configuration.
        /// </summary>
        /// <returns>WCF Binding.</returns>
        Binding CreateServiceBinding();

        #endregion
    }
}
