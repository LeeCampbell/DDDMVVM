namespace SampleApp.Domain
{
    public sealed class City : ICity
    {
        private readonly string _name;
        private readonly ICountry _country;

        public City(string name, ICountry country)
        {
            _name = name;
            _country = country;
        }

        public string Name
        {
            get { return _name; }
        }

        public ICountry Country
        {
            get { return _country; }
        }

        public bool IsValidZipCode(string zipCode)
        {
            return true;//TODO:
        }
    }
}