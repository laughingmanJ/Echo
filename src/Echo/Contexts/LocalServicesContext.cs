using Echo.Addresses;
using Echo.Diagnostics;
using Echo.Interfaces;
using Echo.Proxies;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Echo.Contexts
{
    /// <summary>
    /// Services context for WCF Named Piped Services.
    /// </summary>
    public sealed class LocalServicesContext : IServicesContext
    {
        #region Constants

        private const int MaxBufferSize = 2147483647;

        #endregion

        #region Fields

        // Reference to an address resolver.
        private readonly IAddressResolver _addressResolver;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes the context with a simple base address.
        /// </summary>
        /// <param name="baseAddress">Base address for services.</param>
        public LocalServicesContext(string baseAddress)
        {
            _addressResolver = new LocalAddressResolver(baseAddress);
        }

        /// <summary>
        /// Initializes class with services base address and the CWDS.exe application path.
        /// </summary>
        /// <param name="addressResolver"></param>
        public LocalServicesContext(IAddressResolver addressResolver)
        {
            _addressResolver = addressResolver;
        }

        #endregion

        #region IServicesContext Methods

        /// <summary>
        /// Checks to see if Services are running.
        /// </summary>
        /// <returns>Value indicating that services are up and running.</returns>
        public string GetServiceAddress<T>()
        {
            return _addressResolver.ResolveAddress(typeof(T).Name);
        }

        /// <summary>
        /// Checks to see if Services are running.
        /// </summary>
        /// <returns>Value indicating that services are up and running.</returns>
        public bool IsServicesRunning()
        {
            try
            {
                var binding = CreateServiceBinding();
                var serviceAddress = GetServiceAddress<IDiagnosticService>();

                // Setup direct service channel proxy to make service calls.
                using (var serviceProxy = new ServiceProxy<IDiagnosticService>(binding, serviceAddress))
                {
                    serviceProxy.InitializeChannel();
                    serviceProxy.Channel.Ping();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public Binding CreateClientBinding()
        {
            var binding = new NetNamedPipeBinding();
            return binding;
        }

        public Binding CreateStreamingServiceBinding()
        {
            var binding = new NetNamedPipeBinding
            {
                TransferMode = TransferMode.StreamedResponse,
                MaxReceivedMessageSize = MaxBufferSize,
                MaxBufferSize = MaxBufferSize
            };

            // Control the message buffer size.
            return binding;
        }

        public Binding CreateStreamingClientBinding()
        {
            var binding = new NetNamedPipeBinding
            {
                TransferMode = TransferMode.StreamedResponse,
                MaxReceivedMessageSize = long.MaxValue
            };

            return binding;
        }

        /// <summary>
        /// Creates a WCF service binding based on the CWDS services configuration.
        /// </summary>
        /// <returns>WCF Binding.</returns>
        public Binding CreateServiceBinding()
        {
            var binding = new NetNamedPipeBinding
            {
                MaxReceivedMessageSize = MaxBufferSize,
                MaxBufferSize = MaxBufferSize
            };
            return binding;
        }

        #endregion
    }
}
