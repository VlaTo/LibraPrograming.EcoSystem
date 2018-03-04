using System;
using Windows.UI.Xaml.Controls;

namespace LibraProgramming.Windows.EcoSystem.Core
{
    internal sealed class FrameNavigationProvider : INavigationProvider
    {
        private readonly Frame frame;

        public bool CanGoForward => frame.CanGoForward;

        public FrameNavigationProvider(Frame frame)
        {
            this.frame = frame;
        }

        public bool CanNavigate(Type targetPageType)
        {
            return true;
        }

        public void Navigate(Type targetPageType, object parameter)
        {
            frame.Navigate(targetPageType, parameter);
        }
    }
}