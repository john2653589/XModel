using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Rugal.Xamarin.BindingModel
{
    public class OnChangeEventArg : EventArgs
    {
        public string PropertyName { get; set; }
        public OnChangeEventArg(string _PropertyName)
        {
            PropertyName = _PropertyName;
        }
    }
    public abstract class BaseOnChangeEvent : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public EventHandler<OnChangeEventArg> _OnChangeEvent;
        public event EventHandler<OnChangeEventArg> OnChangeEvent
        {
            add => _OnChangeEvent += value;
            remove => _OnChangeEvent -= value;
        }
        public virtual void OnChange([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
            _OnChangeEvent?.Invoke(this, new OnChangeEventArg(PropertyName));
        }
    }
}