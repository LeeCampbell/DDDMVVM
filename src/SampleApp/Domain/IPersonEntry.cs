using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive;

namespace SampleApp.Domain
{
    public interface IPersonEntry : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        ReadOnlyObservableCollection<ICountry> AvailableCountries { get; }

        ModelState State { get; }

        IAddress Address { get; }
        string GivenName { get; set; }
        string FamilyName { get; set; }
        DateTime DateOfBirth { get; set; }

        IObservable<ICountry> LoadCountries();
        IDisposable AddCountries(IObservable<IList<ICountry>> countriesSequence);

        IObservable<Unit> Save();
    }
}
