using Echo.Diagnostics;
using Echo.Helpers;
using Echo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Echo.Hosting
{
    /// <summary>
    /// Basic Service Host Manager for WCF Services.
    /// </summary>
    public class ServicesHostManager
    {
        #region Fields

        // Services context.
        protected readonly IServicesContext _servicesContext;

        /// <summary>
        /// Collection of ServiceHosts
        /// </summary>
        private readonly Dictionary<Type, ServiceHost> _hostedServices = new Dictionary<Type, ServiceHost>();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes class with dependency context.
        /// </summary>
        /// <param name="servicesContext">Context of running services.</param>
        public ServicesHostManager(IServicesContext servicesContext)
        {
            if (servicesContext == null)
            {
                throw new ArgumentNullException("servicesContext");
            }

            _servicesContext = servicesContext;

            // Add the DiagnosticService for internal use.
            AddService<IDiagnosticService, DiagnosticService>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Add a new service based on blueprint class(factory implementation).
        /// </summary>
        /// <typeparam name="T">Service contract.</typeparam>
        /// <typeparam name="TR">Class implementing service contract.</typeparam>
        // Cannot be parameter T like ReSharper wants because parameter T is used for the class.
        public void AddService<T, TR>()
            where T : class
            where TR : T
        {
            // Create a service address based off the binding and unique identifier.
            var serviceAddress = _servicesContext.GetServiceAddress<T>();

            var serviceHost = new ServiceHost(typeof(TR), new Uri(serviceAddress));
            AddService<T>(serviceHost);
        }

        /// <summary>
        /// Add a new service based on implemented class(singleton implementation).
        /// </summary>
        /// <typeparam name="T">Service contract.</typeparam>
        /// <typeparam name="TR">Class implementing service contract.</typeparam>
        /// <param name="serviceInstance">Instantiated service.</param>
        public void AddService<T, TR>(TR serviceInstance)
            where T : class
            where TR : T
        {
            // Create a service address based off the binding and unique identifier.
            var serviceAddress = _servicesContext.GetServiceAddress<T>();

            var serviceHost = new ServiceHost(serviceInstance, new Uri(serviceAddress));
            AddService<T>(serviceHost);
        }


        /// <summary>
        /// Add service host to manager.
        /// </summary>
        /// <typeparam name="TR">Service contract.</typeparam>
        /// <param name="serviceHost">Service host.</param>
        public void AddService<TR>(ServiceHost serviceHost) where TR : class
        {
            var serviceType = typeof(TR);

            if (_hostedServices.ContainsKey(serviceType))
                throw new Exception(string.Format("Services manager is already hosting service contract {0}.", typeof(TR).Name));

            // Get the appropriate binding based on if the service is a normal service or a streaming service.
            var binding = ServicesHelper.IsStreamingService(serviceType) ? _servicesContext.CreateStreamingServiceBinding()
                : _servicesContext.CreateServiceBinding();

            // If the service type is marked with timeout attribute, set binding’s receive timeout to that value.
            if (ServicesHelper.HasTimeOutAttribute(serviceType))
            {
                binding.ReceiveTimeout = ServicesHelper.GetTimeout(serviceType);
            }

            serviceHost.AddServiceEndpoint(serviceType, binding, string.Empty);
            _hostedServices.Add(serviceType, serviceHost);
        }

        /// <summary>
        /// Start all services.
        /// </summary>
        public void StartServices()
        {
            foreach (var host in _hostedServices)
            {
                if (CommunicationState.Opened == host.Value.State) continue;
                host.Value.Open();
            }
        }

        /// <summary>
        /// Stop all services.
        /// </summary>
        public void StopServices()
        {
            foreach (var host in _hostedServices)
            {
                if (CommunicationState.Opened == host.Value.State)
                {
                    host.Value.Close();
                }
                else if (CommunicationState.Faulted == host.Value.State)
                {
                    host.Value.Abort();
                    host.Value.Close();
                }
            }

            _hostedServices.Clear();
        }

        #endregion
    }
}
