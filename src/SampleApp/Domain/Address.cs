using System.ComponentModel;

namespace SampleApp.Domain
{
    public sealed class Address : ValidationBase, IAddress
    {
        #region Private fields

        private string _line1;
        private string _line2;
        private string _line3;
        private string _line4;
        private string _zipCode;
        private ICity _city;

        #endregion

        public string Line1
        {
            get { return _line1; }
            set
            {
                if (_line1 != value)
                {
                    _line1 = value;
                    OnPropertyChanged("Line1");
                }
            }
        }
        
        public string Line2
        {
            get { return _line2; }
            set
            {
                if (_line2 != value)
                {
                    _line2 = value;
                    OnPropertyChanged("Line2");
                }
            }
        }

        public string Line3
        {
            get { return _line3; }
            set
            {
                if (_line3 != value)
                {
                    _line3 = value;
                    OnPropertyChanged("Line3");
                }
            }
        }

        public string Line4
        {
            get { return _line4; }
            set
            {
                if (_line4 != value)
                {
                    _line4 = value;
                    OnPropertyChanged("Line4");
                }
            }
        }
        
        public string ZipCode
        {
            get { return _zipCode; }
            set
            {
                if (_zipCode != value)
                {
                    _zipCode = value;
                    OnPropertyChanged("ZipCode");
                }
            }
        }

        public ICity City
        {
            get { return _city; }
            set
            {
                if (_city != value)
                {
                    _city = value;
                    OnPropertyChanged("City");
                }
            }
        }

        protected override void OnValidate()
        {
            if(string.IsNullOrWhiteSpace(Line1) || Line1.Length<1)
            {
                AddError("Line1", "Line1 is required");
            }
            if(City==null)
            {
                AddError("City", "City is required");
            }
            else if(City.IsValidZipCode(ZipCode))
            {
                AddError("ZipCode", "Zip code is not valid for the city");
            }
        }

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}