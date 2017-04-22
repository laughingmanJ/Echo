namespace Echo.Interfaces
{
    /// <summary>
    /// Represents an address resolver for resolving service addresses.
    /// </summary>
    public interface IAddressResolver
    {
        /// <summary>
        /// Resolves an address for a service name.
        /// </summary>
        /// <param name="serviceName">Service name.</param>
        /// <returns>Full address of service.</returns>
        string ResolveAddress(string serviceName);
    }
}
