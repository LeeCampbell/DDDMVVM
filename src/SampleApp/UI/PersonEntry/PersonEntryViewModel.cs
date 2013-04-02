using System;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Sample.Core;
using SampleApp.Domain;

namespace SampleApp.UI.PersonEntry
{
    public sealed class PersonEntryViewModel : INotifyPropertyChanged, IDisposable
    {
        private readonly IPersonEntry _person;
        private readonly ISchedulerProvider _schedulerProvider;
        private readonly SerialDisposable _countriesSubscription = new SerialDisposable();
        private readonly IDisposable _errorStateSubscription;
        private DelegateCommand _closeCommand;

        public PersonEntryViewModel(IPersonEntry person, ISchedulerProvider schedulerProvider)
        {
            _person = person;
            _schedulerProvider = schedulerProvider;

            SaveCommand = new DelegateCommand(Save, () => !Person.HasErrors && !Person.State.HasError);

            var errors = Observable.FromEventPattern<EventHandler<DataErrorsChangedEventArgs>, DataErrorsChangedEventArgs>(
                    h => Person.ErrorsChanged += h,
                    h => Person.ErrorsChanged -= h)
                .Select(_ => Unit.Default);
            var states = Person.OnPropertyChanges(p => p.State)
                               .Select(_ => Unit.Default);

            _errorStateSubscription = Observable
                .Merge(errors, states)
                .ObserveOn(_schedulerProvider.Dispatcher)
                .Subscribe(_ => SaveCommand.RaiseCanExecuteChanged());

            states.ObserveOn(_schedulerProvider.Dispatcher)
                 .Subscribe(_ => OnPropertyChanged("State"));
        }


        public IPersonEntry Person { get { return _person; } }
        public ModelState State { get { return Person.State; } }
        public DelegateCommand SaveCommand { get; private set; }

        public DelegateCommand CloseCommand
        {
            get { return _closeCommand; }
            set
            {
                if (_closeCommand != value)
                {
                    _closeCommand = value;
                    OnPropertyChanged("CloseCommand");
                }
            }
        }

        public void Load()
        {
            var countries = _person.LoadCountries()
                .Buffer(TimeSpan.FromMilliseconds(200), 50, _schedulerProvider.Concurrent)
                .SubscribeOn(_schedulerProvider.Concurrent)
                .ObserveOn(_schedulerProvider.Dispatcher);
            _countriesSubscription.Disposable = _person.AddCountries(countries);
        }

        private void Save()
        {
            Person.Save()
                .SubscribeOn(_schedulerProvider.Concurrent)
                .ObserveOn(_schedulerProvider.Dispatcher)
                .Subscribe(_ => CloseCommand.Execute());
        }

        public void Dispose()
        {
            _countriesSubscription.Dispose();
            _errorStateSubscription.Dispose();
        }

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
