using Echo.Diagnostics;
using Echo.Interfaces;
using System;
using System.Collections.Generic;
using System.ServiceModel;

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
            RegisterService<IDiagnosticService, DiagnosticService>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Add a new service based on blueprint class(factory implementation).
        /// </summary>
        /// <typeparam name="T">Service contract.</typeparam>
        /// <typeparam name="TR">Class implementing service contract.</typeparam>
        // Cannot be parameter T like ReSharper wants because parameter T is used for the class.
        public void RegisterService<T, TR>()
            where T : class
            where TR : T
        {
            var serviceType = typeof(T);

            if (_hostedServices.ContainsKey(serviceType))
                throw new Exception(string.Format("Services manager is already hosting service contract {0}.", typeof(T).Name));

            var serviceHost = _servicesContext.CreateServiceHost<T, TR>();
            _hostedServices.Add(serviceType, serviceHost);
        }

        /// <summary>
        /// Add a new service based on implemented class(singleton implementation).
        /// </summary>
        /// <typeparam name="T">Service contract.</typeparam>
        /// <typeparam name="TR">Class implementing service contract.</typeparam>
        /// <param name="serviceInstance">Instantiated service.</param>
        public void RegisterService<T, TR>(TR serviceInstance)
            where T : class
            where TR : T
        {
            var serviceType = typeof(T);

            if (_hostedServices.ContainsKey(serviceType))
                throw new Exception(string.Format("Services manager is already hosting service contract {0}.", typeof(T).Name));

            var serviceHost = _servicesContext.CreateServiceHost<T, TR>(serviceInstance);
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
