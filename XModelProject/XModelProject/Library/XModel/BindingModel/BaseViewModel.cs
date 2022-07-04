using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Rugal.Xamarin.BindingModel
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        internal Dictionary<string, object> Property { get; set; }
        public BaseViewModel()
        {
            Property = new Dictionary<string, object> { };
        }
        public dynamic PropertyGet([CallerMemberName] string PropertyName = null)
        {
            if (PropertyName == null)
                return null;

            if (!Property.ContainsKey(PropertyName))
                return null;

            return Property[PropertyName];
        }
        public void PropertySet(object SetValue, [CallerMemberName] string PropertyName = null)
        {
            Property[PropertyName] = SetValue;
            OnChange(PropertyName);
        }
        public void OnChange([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}