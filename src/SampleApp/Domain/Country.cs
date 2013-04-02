using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SampleApp.Domain
{
    public sealed class Country : ICountry
    {
        private readonly string _name;
        private readonly ReadOnlyCollection<ICity> _cities;

        public Country(string name, IList<ICity> cities)
        {
            _name = name;
            _cities = new ReadOnlyCollection<ICity>(cities);
        }

        public string Name
        {
            get { return _name; }
        }

        public ReadOnlyCollection<ICity> Cities
        {
            get { return _cities; }
        }
    }
}