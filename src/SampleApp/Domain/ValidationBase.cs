using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SampleApp.Domain
{
    public abstract class ValidationBase : INotifyDataErrorInfo
    {
        private static readonly IEnumerable _noErrors = Enumerable.Empty<string>();
        private readonly Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();

        #region INotifyDataErrorInfo Members

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            var key = ToKey(propertyName);
            if (!_errors.ContainsKey(key))
                return _noErrors;
            return _errors[key];
        }

        public bool HasErrors
        {
            get { return _errors.Count > 0; }
        }

        #endregion

        public IEnumerable<string> PropertiesWithErrors
        {
            get { return _errors.Keys; }
        }

        protected void OnErrorsChanged(string propertyName)
        {
            var handler = ErrorsChanged;
            if (handler != null) handler(this, new DataErrorsChangedEventArgs(propertyName));
        }

        protected void AddError(string propertyName, string error)
        {
            var key = ToKey(propertyName);
            if (!_errors.ContainsKey(key))
                _errors[key] = new List<string>();

            if (!_errors[key].Contains(error))
            {
                _errors[key].Add(error);
            }
        }

        protected abstract void OnValidate();

        protected void Validate()
        {
            var oldErrors = CloneDictionary(_errors);
            _errors.Clear();

            OnValidate();

            RaisedErrorsChanged(oldErrors, _errors);
        }

        private void RaisedErrorsChanged(IDictionary<string, List<string>> oldErrors, IDictionary<string, List<string>> newErrors)
        {
            var keysWithModifiedErrors = new List<string>();

            foreach (var key in oldErrors.Keys)
            {
                if (newErrors.ContainsKey(key))
                {
                    var newPropErrors = newErrors[key].OrderBy(err => err).ToList();
                    var oldPropErrors = oldErrors[key].OrderBy(err => err).ToList();
                    if (oldPropErrors.Count != newPropErrors.Count)
                    {
                        keysWithModifiedErrors.Add(key);
                    }
                    else
                    {
                        if (oldPropErrors.Where((t, i) => t != newPropErrors[i]).Any())
                        {
                            keysWithModifiedErrors.Add(key);
                        }
                    }
                }
                else
                {
                    keysWithModifiedErrors.Add(key);
                }
            }

            foreach (var key in newErrors.Keys)
            {
                if (!keysWithModifiedErrors.Contains(key))
                {
                    keysWithModifiedErrors.Add(key);
                }
            }

            foreach (var key in keysWithModifiedErrors)
            {
                OnErrorsChanged(ToPropertyName(key));
            }
        }

        private static Dictionary<TKey, TValue> CloneDictionary<TKey, TValue>(Dictionary<TKey, TValue> original)
        {
            var clone = new Dictionary<TKey, TValue>(original.Count, original.Comparer);
            foreach (var entry in original)
            {
                clone.Add(entry.Key, entry.Value);
            }
            return clone;
        }

        private static string ToKey(string propertyName)
        {
            return propertyName ?? string.Empty;
        }

        private static string ToPropertyName(string key)
        {
            return key == string.Empty ? null : key;
        }
    }
}