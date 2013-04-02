using System.Windows;

namespace SampleApp.UI.PersonEntry
{
    /// <summary>
    /// Interaction logic for PersonEntryView.xaml
    /// </summary>
    public partial class PersonEntryView : Window
    {
        private readonly PersonEntryViewModel _viewModel;

        public PersonEntryView(PersonEntryViewModel viewModel)
        {
            DataContext = _viewModel = viewModel;
            InitializeComponent();
        }

        public PersonEntryViewModel ViewModel
        {
            get { return _viewModel; }
        }
    }
}
