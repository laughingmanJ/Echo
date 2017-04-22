using System;

namespace Echo.Attributes
{
    /// <summary>
    /// Attribute for expressing timeout on a service contract (Interface). 
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public class ServiceTimeoutAttribute : Attribute
    {
        #region Properties

        /// <summary>
        /// Hours value for timeout.
        /// </summary>
        public int Hours { get; set; }

        /// <summary>
        /// Minutes value for timeout.
        /// </summary>
        public int Minutes { get; set; }

        /// <summary>
        /// Seconds value for timeout.
        /// </summary>
        public int Seconds { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes class with timeout hours, minutes, and seconds values.
        /// </summary>
        /// <param name="hours">Timeout hours.</param>
        /// <param name="minutes">Timeout minutes.</param>
        /// <param name="seconds">Timeout seconds.</param>
        public ServiceTimeoutAttribute(int hours, int minutes, int seconds)
        {
            Hours = hours;
            Minutes = minutes;
            Seconds = seconds;
        }

        #endregion
    }
}
