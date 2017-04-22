using Echo.Interfaces;
using Echo.Proxies;

namespace Echo.Factories
{
    public class StreamingClientFactory<T> : IClientFactory<T>
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
        public StreamingClientFactory(IServicesContext servicesContext)
        {
            _servicesContext = servicesContext;
        }

        #endregion

        #region IClientFactory<T> Methods

        public IServiceProxy<T> Create()
        {
            var binding = _servicesContext.CreateStreamingClientBinding();
            var serviceAddress = _servicesContext.GetServiceAddress<T>();
            var serviceProxy = new ServiceProxy<T>(binding, serviceAddress);
            serviceProxy.InitializeChannel();

            // Setup direct service channel proxy to make service calls.
            return serviceProxy;
        }

        #endregion
    }
}
