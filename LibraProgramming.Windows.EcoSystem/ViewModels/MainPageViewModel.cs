using System.Collections.ObjectModel;

namespace LibraProgramming.Windows.EcoSystem.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private int epoch;

        public int Epoch
        {
            get => epoch;
            set => SetProperty(ref epoch, value);
        }

        public ObservableCollection<int> Mutations
        {
            get;
        }

        public MainPageViewModel()
        {
            Mutations = new ObservableCollection<int>();
        }
    }
}