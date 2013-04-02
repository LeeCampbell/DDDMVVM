using System;
using SampleApp.Domain;

namespace SampleApp.Services
{
    public interface ILocationService
    {
        IObservable<ICountry> GetCountries();
    }
}
