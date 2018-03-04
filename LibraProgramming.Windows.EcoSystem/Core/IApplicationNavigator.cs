using System;

namespace LibraProgramming.Windows.EcoSystem.Core
{
    public interface IApplicationNavigator
    {
        void Navigate(Type targetPageType, object parameter);
    }
}