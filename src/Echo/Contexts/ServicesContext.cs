using Echo.Addresses;
using Echo.Diagnostics;
using Echo.Factories;
using Echo.Helpers;
using Echo.Interfaces;
using Echo.Proxies;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Echo.Contexts
{
    /// <summary>
    /// Services context for WCF Named Piped Services.
    /// </summary>
    public sealed class ServicesContext : IServicesContext, IClientContext
    {
        #region Constants

        // Max buffer size for transmitting and receiving data in WCF services. 
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
        public ServicesContext(string baseAddress)
        {
            if(string.IsNullOrEmpty(baseAddress))
            {
                throw new ArgumentNullException("baseAddress");
            }

            var fullAddress = string.Format("net.pipe://localhost/{0}/", baseAddress);
            _addressResolver = new AddressResolver(fullAddress);
        }

        /// <summary>
        /// Initializes class with services base address and the CWDS.exe application path.
        /// </summary>
        /// <param name="addressResolver"></param>
        public ServicesContext(IAddressResolver addressResolver)
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
                var channelFactory = new ChannelFactory<IDiagnosticService>(binding, new EndpointAddress(serviceAddress));

                // Setup direct service channel proxy to make service calls.
                using (var serviceProxy = new ServiceProxy<IDiagnosticService, ChannelFactory<IDiagnosticService>>(channelFactory))
                {
                    serviceProxy.Channel.Ping();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Creates a service host based on the service contract and type.
        /// </summary>
        /// <typeparam name="TContract">A service contract.</typeparam>
        /// <typeparam name="TService">A service type that implements the contract.</typeparam>
        /// <param name="instance"></param>
        /// <returns>Service host.</returns>
        ServiceHost IServicesContext.CreateServiceHost<TContract, TService>()
        {
            var address = GetServiceAddress<TContract>();
            var serviceHost = new ServiceHost(typeof(TService), new Uri(address));
            ConfigureServiceHost<TContract>(serviceHost);

            return serviceHost;
        }

        /// <summary>
        /// Creates a service host based on the service contract and an service instance.
        /// </summary>
        /// <typeparam name="TContract">A service contract.</typeparam>
        /// <typeparam name="TService">A service type that implements the contract.</typeparam>
        /// <param name="instance"></param>
        /// <returns>Service host.</returns>
        ServiceHost IServicesContext.CreateServiceHost<TContract, TService>(TService instance)
        {
            var address = GetServiceAddress<TContract>();
            var serviceHost = new ServiceHost(instance, new Uri(address));
            ConfigureServiceHost<TContract>(serviceHost);

            return serviceHost;
        }

        #endregion

        #region IClientContext Methods


        /// <summary>
        /// Creates a service proxy factory for a service contract.
        /// </summary>
        /// <typeparam name="TContract">Service contract.</typeparam>
        /// <returns>Service proxy factory/returns>
        public IClientFactory<T> CreateFactory<T>()
            where T : class
        {
            var binding = ServicesHelper.IsStreamingService(typeof(T)) ? CreateStreamingClientBinding() : CreateClientBinding();
            var address = GetServiceAddress<T>();

            return new ClientFactory<T>(binding, address);
        }

        /// <summary>
        /// Creates a duplex (two-way calling) service proxy factory for a service contract.
        /// </summary>
        /// <typeparam name="TContract">Service contract.<</typeparam>
        /// <typeparam name="TCallback">Callback contract.</typeparam>
        /// <param name="callback"></param>
        /// <returns>Service proxy factory.</returns>
        public IClientFactory<T> CreateDuplexFactory<T, TCallback>(TCallback callback)
            where T : class
            where TCallback : class
        {
            var binding = CreateClientBinding();
            var address = GetServiceAddress<T>();

            return new DuplexClientFactory<T, TCallback>(binding, address, callback);
        }

        #endregion

        #region Private Methods

        private void ConfigureServiceHost<TContract>(ServiceHost serviceHost)
            where TContract : class
        {
            var serviceType = typeof(TContract);

            // Get the appropriate binding based on if the service is a normal service or a streaming service.
            var binding = ServicesHelper.IsStreamingService(serviceType) ? CreateStreamingServiceBinding()
                : CreateServiceBinding();

            // If the service type is marked with timeout attribute, set binding’s receive timeout to that value.
            if (ServicesHelper.HasTimeOutAttribute(serviceType))
            {
                binding.ReceiveTimeout = ServicesHelper.GetTimeout(serviceType);
            }

            serviceHost.AddServiceEndpoint(serviceType, binding, string.Empty);
        }


        private Binding CreateClientBinding()
        {
            var binding = new NetNamedPipeBinding();
            return binding;
        }

        private Binding CreateStreamingServiceBinding()
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

        private Binding CreateStreamingClientBinding()
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
        private Binding CreateServiceBinding()
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
