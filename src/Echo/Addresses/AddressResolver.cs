using Echo.Interfaces;

namespace Echo.Addresses
{
    /// <summary>
    /// Represents an address resolver for resolving service addresses.
    /// </summary>
    public class AddressResolver : IAddressResolver
    {
        #region Fields

        // Base service address for Services.
        private readonly string _baseAddress;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes class with base service address.
        /// </summary>
        /// <param name="baseAddress">Base service address.</param>
        public AddressResolver(string baseAddress)
        {
            _baseAddress = baseAddress;
        }

        #endregion

        #region IAddressResolver

        /// <summary>
        /// Resolves an address for a service name.
        /// </summary>
        /// <param name="serviceName">Service name.</param>
        /// <returns>Full address of service.</returns>
        public string ResolveAddress(string serviceName)
        {
            return string.Format("{0}/{1}", _baseAddress, serviceName);
        }

        #endregion
    }
}
