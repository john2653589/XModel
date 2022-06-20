using System;
using System.Collections.Generic;
using System.Text;

namespace Rugal.Xamarin.ServiceManager
{
    public interface IServiceManager
    {
        public TService GetService<TService>() where TService : class;
    }
}