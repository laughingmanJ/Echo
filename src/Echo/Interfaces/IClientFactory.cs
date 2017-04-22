namespace Echo.Interfaces
{
    /// <summary>
    /// Factory for creating a proxy client for standard service.
    /// </summary>
    /// <typeparam name="T">Service contract type.</typeparam>
    public interface IClientFactory<out T>
        where T : class
    {
        #region Methods

        /// <summary>
        /// Creates a service proxy for connecting and using a service.
        /// </summary>
        /// <returns>Service proxy.</returns>
        IServiceProxy<T> Create();

        #endregion
    }
}
