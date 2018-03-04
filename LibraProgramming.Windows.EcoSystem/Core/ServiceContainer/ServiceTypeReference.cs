using System;

namespace LibraProgramming.Windows.Games.Towers.Core.ServiceContainer
{
    /// <summary>
    /// The ServiceTypeReference class.
    /// </summary>
    public class ServiceTypeReference : IEquatable<ServiceTypeReference>
    {
        /// <summary>
        /// 
        /// </summary>
        public Type Type
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Key
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="key"></param>
        public ServiceTypeReference(Type type, string key = null)
        {
            Type = type;
            Key = key;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(ServiceTypeReference other)
        {
            if (null == other)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Type == other.Type && String.Equals(Key, other.Key);
        }
    }
}