using System;

namespace LibraProgramming.Windows.Games.Towers.Core.ServiceContainer
{
    /// <summary>
    /// 
    /// </summary>
    public interface IServiceLocator : IServiceProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        object GetInstance(Type serviceType, string key = null);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        TService GetInstance<TService>(string key = null);
    }
}