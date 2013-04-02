namespace SampleApp.Domain
{
    public interface ICity
    {
        string Name { get; }
        ICountry Country { get; }
        bool IsValidZipCode(string zipCode);
    }
}