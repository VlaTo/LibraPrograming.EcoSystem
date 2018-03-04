using System;

namespace LibraProgramming.Windows.Games.Towers.Core.ServiceContainer
{
    /// <summary>
    /// 
    /// </summary>
    public class ServiceLocatorException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public ServiceLocatorException()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public ServiceLocatorException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public ServiceLocatorException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}