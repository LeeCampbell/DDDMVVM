// Type: System.ComponentModel.INotifyDataErrorInfo
// Assembly: System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.dll

using System;
using System.Collections;

namespace System.ComponentModel
{
  /// <summary>
  /// Defines members that data entity classes can implement to provide custom synchronous and asynchronous validation support.
  /// </summary>
  [__DynamicallyInvokable]
  public interface INotifyDataErrorInfo
  {
    /// <summary>
    /// Gets a value that indicates whether the entity has validation errors.
    /// </summary>
    /// 
    /// <returns>
    /// true if the entity currently has validation errors; otherwise, false.
    /// </returns>
    [__DynamicallyInvokable]
    bool HasErrors { [__DynamicallyInvokable] get; }

    /// <summary>
    /// Occurs when the validation errors have changed for a property or for the entire entity.
    /// </summary>
    [__DynamicallyInvokable]
    event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

    /// <summary>
    /// Gets the validation errors for a specified property or for the entire entity.
    /// </summary>
    /// 
    /// <returns>
    /// The validation errors for the property or entity.
    /// </returns>
    /// <param name="propertyName">The name of the property to retrieve validation errors for; or null or <see cref="F:System.String.Empty"/>, to retrieve entity-level errors.</param>
    [__DynamicallyInvokable]
    IEnumerable GetErrors(string propertyName);
  }
}
