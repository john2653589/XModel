using Rugal.Xamarin.ServiceManager;
using System;
using System.Collections.Generic;
using System.Text;
using XModelProject;

namespace XModelProject.Service
{
    public partial class ServiceManager : IServiceManager
    {
        public TService GetService<TService>() where TService : class
        {
            var Service = App.Services.GetService(typeof(TService)) as TService;
            return Service;
        }
    }
}
