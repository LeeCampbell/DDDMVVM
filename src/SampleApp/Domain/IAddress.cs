using System.ComponentModel;

namespace SampleApp.Domain
{
    public interface IAddress : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        string Line1 { get; set; }
        string Line2 { get; set; }
        string Line3 { get; set; }
        string Line4 { get; set; }

        string ZipCode { get; set; }

        ICity City { get; set; }
    }
}