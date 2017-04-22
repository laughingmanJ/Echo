using Echo.Interfaces;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Echo.Proxies
{
    /// <summary>
    /// Common single channel service proxy.
    /// </summary>
    /// <typeparam name="T">Service contract.</typeparam>
    class ServiceProxy<T> : ServiceProxyBase<T, ChannelFactory<T>>, IServiceProxy<T>
        where T : class
    {
        /// <summary>
        /// Initializes class with binding and service address. 
        /// </summary>
        /// <param name="binding">Service binding.</param>
        /// <param name="address">Service address</param>
        public ServiceProxy(Binding binding, string address)
            : base(binding, address)
        {
        }

        /// <summary>
        /// Method for creating a channel factory that is specific to single channels.
        /// </summary>
        /// <param name="binding">Service binding.</param>
        /// <param name="address">Service address.</param>
        /// <returns>Single channel factory.</returns>
        protected override ChannelFactory<T> CreateChannelFactory(Binding binding, string address)
        {
            return new ChannelFactory<T>(binding, new EndpointAddress(address));
        }
    }
}
