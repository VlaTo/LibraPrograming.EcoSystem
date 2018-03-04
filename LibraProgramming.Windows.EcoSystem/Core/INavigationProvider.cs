using System;

namespace LibraProgramming.Windows.EcoSystem.Core
{
    public interface INavigationProvider
    {
        bool CanGoForward
        {
            get;
        }

        bool CanNavigate(Type targetPageType);

        void Navigate(Type targetPageType, object parameter);
    }
}