using System;

namespace LibraProgramming.Windows.Games.Towers.Core.ServiceContainer
{
    /// <summary>
    /// Marks preffered ctor for <see cref="ServiceLocator" /> class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Constructor)]
    public class PrefferedConstructorAttribute : Attribute
    {
    }
}