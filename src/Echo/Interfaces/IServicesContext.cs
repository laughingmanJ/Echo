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
        /// Gets the named pipe address of a service.
        /// </summary>
        /// <typeparam name="T">A service contract.</typeparam>
        /// <returns>Address of service.</returns>
        string GetServiceAddress<T>();


        ServiceHost CreateServiceHost<TContract, TServiceType>()
            where TContract : class
            where TServiceType : TContract;

        ServiceHost CreateServiceHost<TContract, TServiceType>(TServiceType instance)
            where TContract : class
            where TServiceType : TContract;

        #endregion
    }
}
