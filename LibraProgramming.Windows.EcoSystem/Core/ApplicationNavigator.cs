using System;

namespace LibraProgramming.Windows.EcoSystem.Core
{
    internal sealed class ApplicationNavigator : IApplicationNavigator
    {
        private readonly INavigationProvider provider;

        public ApplicationNavigator(INavigationProvider provider)
        {
            this.provider = provider;
        }

        public void Navigate(Type targetPageType, object parameter)
        {
            if (provider.CanNavigate(targetPageType))
            {
                provider.Navigate(targetPageType, parameter);
            }
        }
    }
}