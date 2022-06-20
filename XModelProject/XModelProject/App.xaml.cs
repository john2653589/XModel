using Microsoft.Extensions.DependencyInjection;
using Rugal.Xamarin.XModel;
using Rugal.Xamarin.ServiceManager;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XModelProject.Api;
using XModelProject.Service;

namespace XModelProject
{
    public partial class App : Application
    {
        internal static ServiceProvider Services { get; set; }
        internal static IServiceManager ServiceManager { get; set; }
        internal static XModel Model { get; set; }
        public App()
        {
            InitializeComponent();

            InitDI();
            InitStaticModel();

            MainPage = new MainPage();
        }

        #region Invoker
        public static Task InvokerAsync(Action Action)
        {
            return Device.InvokeOnMainThreadAsync(Action);
        }
        public static void Invoker(Action Action)
        {
            Device.BeginInvokeOnMainThread(Action);
        }
        #endregion

        internal void InitStaticModel()
        {
            ServiceManager = new ServiceManager();
            Model = new XModel(ServiceManager);
        }
        internal void InitDI()
        {
            var ServiceList = new ServiceCollection();
            ServiceList.AddScoped<ApiController_A>();
            ServiceList.AddScoped<CoprsService>();
            ServiceList.AddScoped<ServiceManager>();

            Services = ServiceList.BuildServiceProvider();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
