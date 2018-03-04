using System;

namespace LibraProgramming.Windows.Games.Towers.Core.ServiceContainer
{
    internal static class Throw
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramName"></param>
        public static void ArgumentNull(string paramName)
        {
            throw new ArgumentNullException(paramName, "");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="paramName"></param>
        public static void MissingServiceRegistration(Type serviceType, string paramName)
        {
            throw new ArgumentException(paramName, "");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceType"></param>
        public static void UnsupportedServiceType(Type serviceType)
        {
            var message = String.Format("Unsupported type:{0}", serviceType.Name);
            throw new ArgumentException(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="argumentName"></param>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        public static void CyclicServiceReference(Type type, string argumentName, Type serviceType, string key)
        {
            var message = String.IsNullOrEmpty(key)
                ? String.Format("Service: {0} has cyclic reference", serviceType)
                : String.Format("Service: {0} with key: {1} has cyclic reference", serviceType, key);

            throw new ServiceLocatorException(message);
        }
    }
}