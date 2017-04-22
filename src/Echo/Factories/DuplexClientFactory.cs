using Echo.Interfaces;
using Echo.Proxies;

namespace Echo.Factories
{
    /// <summary>
    /// Factory for creating duplex service proxies
    /// </summary>
    /// <typeparam name="T">Service contract.</typeparam>
    /// <typeparam name="TR">Callback contract.</typeparam>
    public sealed class DuplexClientFactory<T, TR> : IDuplexClientFactory<T, TR>
        where T : class
        where TR : class
    {
        #region Fields

        // Services context for creating the service bindings and getting the address
        private readonly IServicesContext _servicesContext;

        // Factory for creating a callback client.
        private readonly TR _callback;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes class with a service context. 
        /// </summary>
        /// <param name="servicesContext">Service context.</param>
        /// <param name="callback">Callback client factory.</param>
        public DuplexClientFactory(IServicesContext servicesContext, TR callback)
        {
            _servicesContext = servicesContext;
            _callback = callback;
        }

        #endregion

        #region IDuplexClientFactory<T,TR> Methods

        /// <summary>
        /// Creates a duplex service proxy for connecting and using a service.
        /// </summary>
        /// <returns>Service proxy.</returns>
        public IDuplexServiceProxy<T, TR> Create()
        {
            var serviceAddress = _servicesContext.GetServiceAddress<T>();
            var binding = _servicesContext.CreateClientBinding();
            var serviceProxy = new DuplexServiceProxy<T, TR>(binding, serviceAddress, _callback);
            serviceProxy.InitializeChannel();

            return serviceProxy;
        }

        #endregion
    }
}
