using System;
using System.Collections.Generic;

namespace LibraProgramming.Windows.Games.Towers.Core.ServiceContainer
{
    internal class TypeFactory : Factory
    {
        private readonly Type type;

        public TypeFactory(IInstanceProvider provider, Type type)
            : base(provider)
        {
            this.type = type;
        }

        public override object Create(Stack<ServiceTypeReference> types)
        {
            return CreateInstance(types, type);
        }
    }
}