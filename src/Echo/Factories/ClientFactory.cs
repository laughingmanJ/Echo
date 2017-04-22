using Echo.Interfaces;
using Echo.Proxies;

namespace Echo.Factories
{
    /// <summary>
    /// Factory for creating service proxies of a specific service contract. 
    /// </summary>
    /// <typeparam name="T">Service contract (Interface).</typeparam>
    public class ClientFactory<T> : IClientFactory<T>
        where T : class
    {
        #region Fields

        // Services context for creating the service bindings and getting the address
        private readonly IServicesContext _servicesContext;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes class with a service context. 
        /// </summary>
        /// <param name="servicesContext">Service context.</param>
        public ClientFactory(IServicesContext servicesContext)
        {
            _servicesContext = servicesContext;
        }

        #endregion'

        #region IClientFactory<T> Methods

        public IServiceProxy<T> Create()
        {
            var binding = _servicesContext.CreateClientBinding();
            var serviceAddress = _servicesContext.GetServiceAddress<T>();
            var serviceProxy = new ServiceProxy<T>(binding, serviceAddress);
            serviceProxy.InitializeChannel();

            // Setup direct service channel proxy to make service calls.
            return serviceProxy;
        }

        #endregion
    }
}
