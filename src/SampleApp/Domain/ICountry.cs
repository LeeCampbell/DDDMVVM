using System.Collections.ObjectModel;

namespace SampleApp.Domain
{
    public interface ICountry
    {
        string Name { get; }
        ReadOnlyCollection<ICity> Cities { get; }
    }
}