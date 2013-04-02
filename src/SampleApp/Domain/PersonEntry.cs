using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Linq;
using SampleApp.Services;

namespace SampleApp.Domain
{
    public sealed class PersonEntry : ValidationBase, IPersonEntry
    {
        #region Private fields

        private readonly ILocationService _locationService;

        private readonly ObservableCollection<ICountry> _availableCountries = new ObservableCollection<ICountry>();
        private readonly ReadOnlyObservableCollection<ICountry> _roAvailableCountries;
        private readonly IAddress _address;

        private string _givenName;
        private string _familyName;
        private DateTime _dateOfBirth;
        private ModelState _state = ModelState.Error("Not yet loaded");

        #endregion


        public PersonEntry(ILocationService locationService, IAddress address)
        {
            _locationService = locationService;
            _address = address;

            _roAvailableCountries = new ReadOnlyObservableCollection<ICountry>(_availableCountries);
        }

        public ReadOnlyObservableCollection<ICountry> AvailableCountries
        {
            get { return _roAvailableCountries; }
        }

        public ModelState State
        {
            get { return _state; }
            private set
            {
                if (_state != value)
                {
                    _state = value;
                    OnPropertyChanged("State");
                }

            }
        }

        public IAddress Address
        {
            get { return _address; }
        }

        public string GivenName
        {
            get { return _givenName; }
            set
            {
                if (_givenName != value)
                {
                    _givenName = value;
                    OnPropertyChanged("GivenName");
                }
            }
        }

        public string FamilyName
        {
            get { return _familyName; }
            set
            {
                if (_familyName != value)
                {
                    _familyName = value;
                    OnPropertyChanged("FamilyName");
                }
            }
        }

        public DateTime DateOfBirth
        {
            get { return _dateOfBirth; }
            set
            {
                if (_dateOfBirth != value)
                {
                    _dateOfBirth = value;
                    OnPropertyChanged("DateOfBirth");
                }
            }
        }

        public IObservable<ICountry> LoadCountries()
        {
            return _locationService.GetCountries();
        }
        public IDisposable AddCountries(IObservable<IList<ICountry>> countriesSequence)
        {
            State = ModelState.Processing;
            return countriesSequence.Subscribe(countries =>
                                            {
                                                foreach (var country in countries)
                                                {
                                                    _availableCountries.Add(country);
                                                }
                                            },
                                            () => State = ModelState.Idle);

        }

        public IObservable<Unit> Save()
        {
            return Observable.Create<Unit>(
                o =>
                {
                    State = ModelState.Processing;
                    return Observable.Timer(TimeSpan.FromSeconds(2))
                                .Select(_ => Unit.Default)
                                .Finally(() => State = ModelState.Idle)
                                .Subscribe(o);
                });
        }

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));

            Validate();
        }

        #endregion

        protected override void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(GivenName) || GivenName.Trim().Length == 1)
            {
                AddError("GivenName", "Given name is not valid");
            }
            if (string.IsNullOrWhiteSpace(FamilyName) || FamilyName.Trim().Length == 1)
            {
                AddError("FamilyName", "Family name is not valid");
            }
            if (DateOfBirth > DateTime.Today)
            {
                AddError("DateOfBirth", "Date of birth is not in the past");
            }
            if (Address.HasErrors)
            {
                AddError("Address", "Address is not valid");
            }
        }
    }
}