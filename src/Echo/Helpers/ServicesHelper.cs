using Echo.Attributes;
using System;

namespace Echo.Helpers
{
    /// <summary>
    /// Helper functions for handling various internal service operations.
    /// </summary>
    static class ServicesHelper
    {
        #region Methods

        /// <summary>
        /// Checks to see the type has a streaming service attribute associated to it.
        /// </summary>
        /// <param name="type">Service contract type.</param>
        /// <returns>Value indicating that type uses streaming service attribute.</returns>
        public static bool IsStreamingService(Type type)
        {
            var attributes = type.GetCustomAttributes(typeof(StreamingServiceAttribute), true);
            return attributes.Length > 0;
        }

        /// <summary>
        /// Checks to see the type has a service timeout attribute associated to it.
        /// </summary>
        /// <param name="type">Service contract type.</param>
        /// <returns>Value indicating that type uses service timeout attribute.</returns>
        public static bool HasTimeOutAttribute(Type type)
        {
            var attributes = type.GetCustomAttributes(typeof(ServiceTimeoutAttribute), true);
            return attributes.Length > 0;
        }

        /// <summary>
        /// Gets the timeout value of a service contract type that has a service timeout attribute.
        /// </summary>
        /// <param name="type">Service contract type.</param>
        /// <returns>Timeout value.</returns>
        public static TimeSpan GetTimeout(Type type)
        {
            var attributes = type.GetCustomAttributes(typeof(ServiceTimeoutAttribute), true);
            if (attributes.Length > 0)
            {
                var attribute = (ServiceTimeoutAttribute)attributes[0];
                return new TimeSpan(attribute.Hours, attribute.Minutes, attribute.Seconds);
            }

            throw new InvalidOperationException("Type does not have a ServiceTimeoutAttribute associated to it.");
        }

        #endregion
    }
}
