using Rugal.Xamarin.XModel.FuncModel;
using Rugal.Xamarin.ServiceManager;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Rugal.Xamarin.XModel.ServiceModel
{
    public partial class XModelService<TService> : XModel where TService : class
    {
        private XModel BaseModel => GetBaseModel();
        internal override IServiceManager ServiceManager => BaseModel.ServiceManager;
        internal override Dictionary<string, ApiFuncModel> StorageKeys => BaseModel.StorageKeys;
        public override XModelData XResult => BaseModel.XResult;
        public TService Service { get; set; }
        public XModelService(IServiceManager ServiceManager) : base(ServiceManager)
        {
            Service = ServiceManager.GetService<TService>();
        }
        public XModelService() { }

        #region Add Api
        public XModelService<TService> AddStorage<TResult>(string StorageKey, Func<TService, Task<TResult>> ApiFunc)
        {
            var FuncModel = new ApiFuncModel(true, async () =>
            {
                var GetModel = await ApiFunc.Invoke(Service);
                return GetModel;
            });
            StorageKeys[StorageKey] = FuncModel;
            return this;
        }
        public XModelService<TService> AddStorage<TResult>(Func<TService, Task<TResult>> ApiFunc, string StorageKey = DefaultStorageKey)
        {
            return AddStorage(StorageKey, ApiFunc);
        }
        public XModelService<TService> AddStorage<TResult>(string StorageKey, Func<TService, TResult> ApiFunc)
        {
            var FuncModel = new ApiFuncModel(true, () =>
            {
                var GetModel = ApiFunc.Invoke(Service) as object;
                return Task.FromResult(GetModel);
            });
            StorageKeys[StorageKey] = FuncModel;
            return this;
        }
        public XModelService<TService> AddStorage<TResult>(Func<TService, TResult> ApiFunc, string StorageKey = DefaultStorageKey)
        {
            return AddStorage(StorageKey, ApiFunc);
        }
        public static XModelService<TService> CreateModelService(XModel _Model)
        {
            var Service = new XModelService<TService>()
            {
                ParentXModel = _Model,
                Service = _Model.ServiceManager.GetService<TService>(),
            };
            return Service;
        }
        #endregion
    }
}