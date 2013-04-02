using System.Windows;
using Sample.Core;
using SampleApp.Services;

namespace SampleApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var locationService = new LocationService();
            var address = new Domain.Address();
            var person = new Domain.PersonEntry(locationService, address);
            var vm = new UI.PersonEntry.PersonEntryViewModel(person, new SchedulerProvider());
            vm.Load();

            var window = new UI.PersonEntry.PersonEntryView(vm);
            vm.CloseCommand = new DelegateCommand(window.Close);

            this.MainWindow = window;
            window.Show();
        }
    }
}
