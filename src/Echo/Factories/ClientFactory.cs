using Echo.Interfaces;
using Echo.Proxies;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Echo.Factories
{
    /// <summary>
    /// Factory for creating service proxies of a specific service contract. 
    /// </summary>
    /// <typeparam name="T">Service contract (Interface).</typeparam>
    class ClientFactory<T> : IClientFactory<T>
        where T : class
    {
        #region Fields

        private readonly Binding _binding;

        private readonly string _address;

        #endregion

        #region Constructors


        public ClientFactory(Binding binding, string address)
        {
            _binding = binding;
            _address = address;
        }

        #endregion'

        #region IClientFactory<T> Methods

        public IServiceProxy<T> Create()
        {
            var channelFactory = new ChannelFactory<T>(_binding, new EndpointAddress(_address));
            var serviceProxy = new ServiceProxy<T, ChannelFactory<T>>(channelFactory);

            // Setup direct service channel proxy to make service calls.
            return serviceProxy;
        }

        #endregion
    }
}
