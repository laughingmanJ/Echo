using Echo.Interfaces;
using Echo.Proxies;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Echo.Factories
{
    /// <summary>
    /// Factory for creating duplex service proxies
    /// </summary>
    /// <typeparam name="TContract">Service contract.</typeparam>
    /// <typeparam name="TCallback">Callback contract.</typeparam>
    sealed class DuplexClientFactory<TContract, TCallback> : IClientFactory<TContract>
        where TContract : class
        where TCallback : class
    {
        #region Fields

        private readonly Binding _binding;

        private readonly string _address;

        // Factory for creating a callback client.
        private readonly TCallback _callback;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes class with a service context. 
        /// </summary>
        /// <param name="servicesContext">Service context.</param>
        /// <param name="callback">Callback client factory.</param>
        public DuplexClientFactory(Binding binding, string address, TCallback callback)
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
        public IServiceProxy<TContract> Create()
        {

            var channelFactory = new DuplexChannelFactory<TContract>(_callback, _binding, new EndpointAddress(_address));
            var serviceProxy = new ServiceProxy<TContract, DuplexChannelFactory<TContract>>(channelFactory);

            return serviceProxy;
        }

        #endregion
    }
}
