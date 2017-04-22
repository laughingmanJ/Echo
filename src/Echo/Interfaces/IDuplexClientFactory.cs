namespace Echo.Interfaces
{
    /// <summary>
    /// Factory for creating duplex service proxies
    /// </summary>
    /// <typeparam name="T">Service contract.</typeparam>
    /// <typeparam name="TR">Callback contract.</typeparam>
    public interface IDuplexClientFactory<out T, out TR>
        where T : class
        where TR : class
    {
        #region Methods

        /// <summary>
        /// Creates a duplex service proxy for connecting and using a service.
        /// </summary>
        /// <returns>Service proxy.</returns>
        IDuplexServiceProxy<T, TR> Create();

        #endregion
    }
}
