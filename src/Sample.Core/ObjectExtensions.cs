using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Sample.Core
{
    public static class ObjectExtensions
    {
        public static IObservable<TProperty> OnPropertyChanges<T, TProperty>(this T source, Expression<Func<T, TProperty>> property)
        {
            return Observable.Create<TProperty>(o =>
                                                    {
                                                        var propertyName = property.GetPropertyInfo().Name;
                                                        var propDesc = TypeDescriptor.GetProperties(source)
                                                            .Cast<PropertyDescriptor>()
                                                            .SingleOrDefault(pd => string.Equals(pd.Name, propertyName, StringComparison.Ordinal));
                                                        if (propDesc == null)
                                                        {
                                                            o.OnError(new InvalidOperationException("Can not register change handler for this property."));
                                                            return Disposable.Empty;
                                                        }
                                                        var propertySelector = property.Compile();
                                                        EventHandler handler = delegate { o.OnNext(propertySelector(source)); };
                                                        propDesc.AddValueChanged(source, handler);

                                                        return Disposable.Create(() => propDesc.RemoveValueChanged(source, handler));
                                                    });
        }
    }
}