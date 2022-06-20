using Rugal.Xamarin.BindingModel;
using Rugal.Xamarin.XModel.BindingProperty;
using Rugal.Xamarin.XModel.Extention;
using Rugal.Xamarin.XModel.FuncModel;
using Rugal.Xamarin.ServiceManager;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Rugal.Xamarin.XModel.ServiceModel;

/*
 *  XModel v1.0.0
 *  From Rugal Tu
 *  MIT License
 */
namespace Rugal.Xamarin.XModel
{
    public partial class XModel : BaseOnChangeEvent
    {
        public const string DefaultStorageKey = "DefaultContent";
        internal XModel ParentXModel { get; set; }
        internal virtual IServiceManager ServiceManager { get; set; }
        internal virtual Dictionary<string, ApiFuncModel> StorageKeys { get; set; }
        public virtual XModelData XResult { get; set; }
        public XModel(IServiceManager _ServiceManager) : this()
        {
            ServiceManager = _ServiceManager;
        }
        internal XModel()
        {
            StorageKeys = new Dictionary<string, ApiFuncModel> { };
            XResult = CreateDictionary();
        }
        #region Create Bindin Dataa
        public XModelData CreateDictionary()
        {
            return new XModelData() { ParentModel = this };
        }
        #endregion

        #region Init
        public XModel InitBind(Page BindPage, Action<XModel> DoneAction = null)
        {
            var SetBaseModel = GetBaseModel();
            BindPage.BindingContext = SetBaseModel;
            DoneAction?.Invoke(SetBaseModel);
            return SetBaseModel;
        }
        #endregion

        #region Convert Extend
        public XModelService<TService> AsService<TService>() where TService : class
        {
            var ModelService = XModelService<TService>.CreateModelService(this);
            return ModelService;
        }

        public XModel AsClone()
        {
            var CloneModel = new XModel(ServiceManager);
            foreach (var Item in StorageKeys)
                CloneModel.StorageKeys.Add(Item.Key, Item.Value);

            foreach (var Item in XResult)
                CloneModel.XResult.Add(Item.Key, Item.Value);
            return CloneModel;
        }


        #endregion

        #region Add Storage
        public XModel AddStorage(string StorageKey, Func<object> ApiFunc)
        {
            var FuncModel = new ApiFuncModel(false, () =>
            {
                var GetModel = ApiFunc.Invoke();
                return Task.FromResult(GetModel);
            });
            StorageKeys[StorageKey] = FuncModel;
            return GetBaseModel();
        }
        public XModel AddStorage(Func<object> ApiFunc, string StorageKey = DefaultStorageKey)
        {
            return AddStorage(StorageKey, ApiFunc);
        }
        public XModel AddStorage<TResult>(string StorageKey, Func<Task<TResult>> ApiFunc)
        {
            var FuncModel = new ApiFuncModel(true, async () =>
            {
                var GetModel = await ApiFunc.Invoke();
                return GetModel;
            });
            StorageKeys[StorageKey] = FuncModel;
            return GetBaseModel();
        }
        public XModel AddStorage<TResult>(Func<Task<TResult>> ApiFunc, string StorageKey = DefaultStorageKey)
        {
            return AddStorage(StorageKey, ApiFunc);
        }
        #endregion

        #region Call Api
        public async Task<XModel> CallStorage(string StorageKey = DefaultStorageKey)
        {
            var BaseModel = GetBaseModel();
            var GetFunc = StorageKeys[StorageKey];
            var GetModel = await GetFunc.ApiFunc.Invoke();
            if (GetModel is XModelData GetDictionary)
                GetDictionary.ParentModel = BaseModel;

            XResult[StorageKey] = GetModel;
            OnChange("XResult");
            return BaseModel;
        }
        public XModel CallStorage(Action<XModel> CallAction)
        {
            var BaseModel = GetBaseModel();
            CallAction(BaseModel);
            return BaseModel;
        }
        #endregion

        #region Add Bind Method
        public XModel AddX_Bind(Element Obj, BindableProperty BindProperty, string BindKey = null, string StorageKey = DefaultStorageKey)
        {
            BaseAddX_Bind(Obj, BindProperty, BindKey, StorageKey);
            return GetBaseModel();
        }
        public XModel AddX_Text(Element Obj, string BindKey = null, string StorageKey = DefaultStorageKey)
        {
            BaseAddX_Bind(Obj, XModelProperty.Text, BindKey, StorageKey);
            return GetBaseModel();
        }
        #endregion

        #region Update Model Data
        public XModel UpdateXResult(object UpdateModel, string StorageKey = DefaultStorageKey)
        {
            XResult[StorageKey] = UpdateModel;
            OnChange("XResult");
            return GetBaseModel();
        }


        #endregion

        #region Base Method
        private XModel BaseAddX_Bind(Element Obj, BindableProperty BindProperty, string BindKey, string StorageKey)
        {
            BindKey ??= Obj.StyleId;
            var FullBindKey = ConvertBindKey(BindKey, StorageKey);
            Obj.AddX_Bind(BindProperty, FullBindKey);
            return GetBaseModel();
        }
        private XModel BaseAddX_Bind(Element Obj, string BindProperty, string BindKey = null, string StorageKey = DefaultStorageKey)
        {
            BindKey ??= Obj.StyleId;
            var FullBindKey = ConvertBindKey(BindKey, StorageKey);
            Obj.AddX_Bind(BindProperty, FullBindKey);
            return GetBaseModel();
        }
        private string ConvertBindKey(string BindKey, string StorageKey = DefaultStorageKey)
        {
            BindKey = BindKey.Contains(".") ? BindKey.Trim('.') : $"[{BindKey}]";
            var JoinBindKey = new[] {
                "XResult",
                $"[{StorageKey}]",
                BindKey
            };

            var FullBindKey = string.Join(".", JoinBindKey);
            return FullBindKey;
        }
        internal XModel GetBaseModel() => ParentXModel ?? this;
        #endregion
    }
}