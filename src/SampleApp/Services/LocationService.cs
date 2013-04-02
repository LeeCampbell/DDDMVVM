using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using SampleApp.Domain;

namespace SampleApp.Services
{
    public sealed class LocationService : ILocationService
    {
        public IObservable<ICountry> GetCountries()
        {
            return Observable.Create<ICountry>(o =>
                                                   {
                                                       o.OnNext(CreateUK());
                                                       o.OnNext(CreateAus());
                                                       o.OnNext(CreateNz());
                                                       o.OnCompleted();
                                                       return Disposable.Empty;
                                                   })
                                                   .Delay(TimeSpan.FromSeconds(2))
                                                   ;
        }

        private ICountry CreateUK()
        {
            var cities = new List<ICity>();
            //UK Postcode regex 
            //^(GIR ?0AA|[A-PR-UWYZ]([0-9]{1,2}|([A-HK-Y][0-9]([0-9ABEHMNPRV-Y])?)|[0-9][A-HJKPS-UW]) ?[0-9][ABD-HJLNP-UW-Z]{2})$
            var uk = new Country("United Kingdom", cities);

            cities.Add(new City("London", uk));
            cities.Add(new City("Manchester", uk));
            cities.Add(new City("Newcastle", uk));
            cities.Add(new City("Birmingham", uk));
            cities.Add(new City("Cardiff", uk));
            cities.Add(new City("Edinburgh", uk));
            cities.Add(new City("Glasgow", uk));
            cities.Add(new City("Belfast", uk));
            return uk;
        }
        private ICountry CreateAus()
        {
            var cities = new List<ICity>();

            //Aus post code regex
            //\d{4}
            var oz = new Country("Australia", cities);

            cities.Add(new City("Brisbane", oz));
            cities.Add(new City("Sydney", oz));
            cities.Add(new City("Canberra", oz));
            cities.Add(new City("Melbourne", oz));
            cities.Add(new City("Hobart", oz));
            cities.Add(new City("Adelaide", oz));
            cities.Add(new City("Perth", oz));
            cities.Add(new City("Darwin", oz));
            return oz;
        }

        private ICountry CreateNz()
        {
            var cities = new List<ICity>();

            var nz = new Country("New Zealand", cities);
            //NZ post code regex?
            //\d{4}
            cities.Add(new City("Whangarei", nz));
            cities.Add(new City("Auckland", nz));
            cities.Add(new City("Hamilton", nz));
            cities.Add(new City("Wellington", nz));
            cities.Add(new City("Christchurch", nz));
            cities.Add(new City("Omaru", nz));
            cities.Add(new City("Dunedin", nz));
            return nz;
        }
    }
}