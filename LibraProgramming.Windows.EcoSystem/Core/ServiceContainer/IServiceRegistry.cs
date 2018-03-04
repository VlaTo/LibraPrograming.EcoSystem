using System;

namespace LibraProgramming.Windows.Games.Towers.Core.ServiceContainer
{
    /// <summary>
    /// 
    /// </summary>
    public interface IServiceRegistry
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="lifetime"></param>
        /// <param name="key"></param>
        /// <param name="createimmediate"></param>
        void Register(Type service, Func<Factory, InstanceLifetime> lifetime = null, string key = null, bool createimmediate = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="impl"></param>
        /// <param name="lifetime"></param>
        /// <param name="key"></param>
        /// <param name="createimmediate"></param>
        void Register(Type service, Type impl, Func<Factory, InstanceLifetime> lifetime = null, string key = null, bool createimmediate = false);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="factory"></param>
        /// <param name="lifetime"></param>
        /// <param name="key"></param>
        /// <param name="createimmediate"></param>
        void Register<TService>(Func<TService> factory, Func<Factory, InstanceLifetime> lifetime = null, string key = null, bool createimmediate = false);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="lifetime"></param>
        /// <param name="key"></param>
        /// <param name="createimmediate"></param>
        void Register<TService>(Func<Factory, InstanceLifetime> lifetime = null, string key = null, bool createimmediate = false)
            where TService : class;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TConcrete"></typeparam>
        /// <param name="lifetime"></param>
        /// <param name="key"></param>
        /// <param name="createimmediate"></param>
        void Register<TService, TConcrete>(Func<Factory, InstanceLifetime> lifetime = null, string key = null, bool createimmediate = false)
            where TConcrete : class, TService;
    }
}