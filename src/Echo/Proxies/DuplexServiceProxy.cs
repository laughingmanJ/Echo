using Echo.Interfaces;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Echo.Proxies
{
    /// <summary>
    /// Duplex service proxy that included a callback object.
    /// </summary>
    /// <typeparam name="T">Service Contract.</typeparam>
    /// <typeparam name="TR">Type that has implemented callback contract.</typeparam>
    class DuplexServiceProxy<T, TR> : ServiceProxyBase<T, DuplexChannelFactory<T>>, IDuplexServiceProxy<T, TR>
        where T : class
        where TR : class
    {

        /// <summary>
        /// Callback object.
        /// </summary>
        public TR CallbackInstance { get; private set; }

        /// <summary>
        /// Initializes class with a binding and service address.
        /// </summary>
        /// <param name="binding">WCF Binding.</param>
        /// <param name="address">Service address.</param>
        /// <param name="callback">Callback client</param>
        public DuplexServiceProxy(Binding binding, string address, TR callback) :
            base(binding, address)
        {
            CallbackInstance = callback;
        }

        /// <summary>
        /// Creates a channel factory that the proxy can use to create service channels.
        /// </summary>
        /// <param name="binding">Service binding.</param>
        /// <param name="address">Service address.</param>
        /// <returns>Channel factory specific to a type of channel(Duplex).</returns>
        protected override DuplexChannelFactory<T> CreateChannelFactory(Binding binding, string address)
        {
            return new DuplexChannelFactory<T>(CallbackInstance, binding, new EndpointAddress(address));
        }
    }
}
