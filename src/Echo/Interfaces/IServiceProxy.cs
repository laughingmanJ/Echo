using System;

namespace Echo.Interfaces
{
    /// <summary>
    /// Service proxy interface for calling service operations.
    /// </summary>
    /// <typeparam name="T">Service contract.</typeparam>
    public interface IServiceProxy<out T> : IDisposable
        where T : class
    {
        #region Methods

        /// <summary>
        /// Service operations channel.
        /// </summary>
        T Channel { get; }

        #endregion
    }
}
