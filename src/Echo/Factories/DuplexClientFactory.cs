using Echo.Interfaces;
using Echo.Proxies;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Echo.Factories
{
    /// <summary>
    /// Factory for creating duplex service proxies
    /// </summary>
    /// <typeparam name="T">Service contract.</typeparam>
    /// <typeparam name="TR">Callback contract.</typeparam>
    sealed class DuplexClientFactory<T, TR> : IClientFactory<T>
        where T : class
        where TR : class
    {
        #region Fields

        private readonly Binding _binding;

        private readonly string _address;

        // Factory for creating a callback client.
        private readonly TR _callback;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes class with a service context. 
        /// </summary>
        /// <param name="servicesContext">Service context.</param>
        /// <param name="callback">Callback client factory.</param>
        public DuplexClientFactory(Binding binding, string address, TR callback)
        {
            _binding = binding;
            _address = address;
            _callback = callback;
        }

        #endregion

        #region IDuplexClientFactory<T,TR> Methods

        /// <summary>
        /// Creates a duplex service proxy for connecting and using a service.
        /// </summary>
        /// <returns>Service proxy.</returns>
        public IServiceProxy<T> Create()
        {

            var channelFactory = new DuplexChannelFactory<T>(_callback, _binding, new EndpointAddress(_address));
            var serviceProxy = new ServiceProxy<T, DuplexChannelFactory<T>>(channelFactory);

            return serviceProxy;
        }

        #endregion
    }
}
