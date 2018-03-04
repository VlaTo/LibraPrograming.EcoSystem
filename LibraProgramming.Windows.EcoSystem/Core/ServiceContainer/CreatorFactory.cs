using System;
using System.Collections.Generic;

namespace LibraProgramming.Windows.Games.Towers.Core.ServiceContainer
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    public class CreatorFactory<TService> : Factory
    {
        private readonly Func<TService> creator;

        public CreatorFactory(IInstanceProvider provider, Func<TService> creator)
            : base(provider)
        {
            this.creator = creator;
        }

        public override object Create(Stack<ServiceTypeReference> types)
        {
            return creator();
        }
    }
}