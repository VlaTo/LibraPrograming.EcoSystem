using System;

namespace LibraProgramming.Windows.Games.Towers.Core.ServiceContainer
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class ServiceAttribute : Attribute
    {
        public string Key
        {
            get;
            set;
        }
    }
}