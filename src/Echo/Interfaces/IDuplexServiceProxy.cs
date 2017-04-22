namespace Echo.Interfaces
{
    /// <summary>
    /// Interface for duplex service proxy.
    /// </summary>
    /// <typeparam name="T">Main service contract type.</typeparam>
    /// <typeparam name="TR">Callback service contract type.</typeparam>
    public interface IDuplexServiceProxy<out T, out TR> : IServiceProxy<T>
        where T : class
        where TR : class
    {
        #region Methods

        /// <summary>
        /// Callback handler
        /// </summary>
        TR CallbackInstance { get; }

        #endregion
    }
}
