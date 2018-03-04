using LibraProgramming.Windows.EcoSystem.Core;

namespace LibraProgramming.Windows.EcoSystem.Views
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class HostPage
    {
        public HostPage()
        {
            InitializeComponent();
        }

        public IApplicationNavigator GetNavigator()
        {
            var provider = new FrameNavigationProvider(NavigationFrame);
            return new ApplicationNavigator(provider);
        }
    }
}
