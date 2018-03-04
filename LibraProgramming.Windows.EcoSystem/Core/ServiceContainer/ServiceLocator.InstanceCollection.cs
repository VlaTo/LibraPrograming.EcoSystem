using System;
using System.Collections.Generic;

namespace LibraProgramming.Windows.Games.Towers.Core.ServiceContainer
{
    partial class ServiceLocator
    {
        private class InstanceCollection
        {
            private readonly Dictionary<string, InstanceLifetime> instances;

            /// <summary>
            /// Initializes a new instance of the <see cref="T:System.Object"/> class.
            /// </summary>
            public InstanceCollection()
            {
                instances = new Dictionary<string, InstanceLifetime>();
            }

            public InstanceLifetime this[string key]
            {
                get => instances[key ?? String.Empty];
                set => instances[key ?? String.Empty] = value;
            }
        }
    }
}