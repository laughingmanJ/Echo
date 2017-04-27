using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Echo.Interfaces
{
    /// <summary>
    /// Context for service bindings and addresses.
    /// </summary>
    public interface IServicesContext
    {
        #region Methods

        /// <summary>
        /// Gets the named pipe address of a service based on its contract name.
        /// </summary>
        /// <typeparam name="TContract">A service contract.</typeparam>
        /// <returns>Address of service.</returns>
        string GetServiceAddress<TContract>();


        /// <summary>
        /// Creates a service host based on the service contract and type.
        /// </summary>
        /// <typeparam name="TContract">A service contract.</typeparam>
        /// <typeparam name="TService">A service type that implements the contract.</typeparam>
        /// <param name="instance"></param>
        /// <returns>Service host.</returns>
        ServiceHost CreateServiceHost<TContract, TService>()
            where TContract : class
            where TService : TContract;

        /// <summary>
        /// Creates a service host based on the service contract and an service instance.
        /// </summary>
        /// <typeparam name="TContract">A service contract.</typeparam>
        /// <typeparam name="TService">A service type that implements the contract.</typeparam>
        /// <param name="instance"></param>
        /// <returns>Service host.</returns>
        ServiceHost CreateServiceHost<TContract, TService>(TService instance)
            where TContract : class
            where TService : TContract;

        #endregion
    }
}
