using System.ServiceModel;

namespace Echo.Diagnostics
{
    /// <summary>
    /// Service for getting diagnostic information of the CWDS service runtime.
    /// </summary>
    [ServiceContract]
    public interface IDiagnosticService
    {
        /// <summary>
        /// Performs the ping connection operation.
        /// </summary>
        [OperationContract]
        void Ping();

    }
}
