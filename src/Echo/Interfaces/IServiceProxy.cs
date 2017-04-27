using System;

namespace Echo.Interfaces
{
    /// <summary>
    /// Service proxy interface for calling service operations.
    /// </summary>
    /// <typeparam name="TContract">Service contract.</typeparam>
    public interface IServiceProxy<out TContract> : IDisposable
        where TContract : class
    {
        #region Methods

        /// <summary>
        /// Service operations channel.
        /// </summary>
        TContract Channel { get; }

        #endregion
    }
}
