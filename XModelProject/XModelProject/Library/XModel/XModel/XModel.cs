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
using System.Linq;

/*
 *  XModel v1.0.3
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
            XResult = CreateXModelData();
        }

        #region Create Binding Data
        public XModelData CreateXModelData()
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

        #region Storage Controller

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

        #region Get Storage
        public object GetStoragePath(string StoragePath)
        {
            var GetResult = RCS_GetStorage(StoragePath, XResult);
            return GetResult;
        }
        public TResult GetStoragePath<TResult>(string StoragePath) where TResult : class
        {
            var GetResult = RCS_GetStorage(StoragePath, XResult);
            return GetResult as TResult;
        }
        public object GetStorage(string GetPath = null, string StorageKey = DefaultStorageKey)
        {
            var FullStoragePath = StorageKey;
            if (GetPath != null)
                FullStoragePath = $"{FullStoragePath}.{GetPath.TrimStart('.')}";
            var GetResult = RCS_GetStorage(FullStoragePath, XResult);
            return GetResult;
        }
        public TResult GetStorage<TResult>(string GetPath = null, string StorageKey = DefaultStorageKey) where TResult : class
        {
            var GetResult = GetStorage(GetPath, StorageKey);
            return GetResult as TResult;
        }
        #endregion

        #region Set Sotrage
        public void SetStoragePath(object SetObject, string FullStoragePath)
        {
            RCS_SetSotrage(SetObject, FullStoragePath, XResult);
        }
        public void SetStorage(object SetObject, string SetPath = null, string StorageKey = DefaultStorageKey)
        {
            var FullStoragePath = StorageKey;
            if (SetPath != null)
                FullStoragePath = $"{FullStoragePath}.{SetPath.TrimStart('.')}";
            RCS_SetSotrage(SetObject, FullStoragePath, XResult);
        }
        #endregion

        #region Call Storage
        public async Task<XModel> CallStorage(string StorageKey = DefaultStorageKey)
        {
            var BaseModel = GetBaseModel();
            var GetFunc = StorageKeys[StorageKey];
            var GetModel = await GetFunc.ApiFunc.Invoke();
            if (GetModel is XModelData GetDictionary)
                GetDictionary.ParentModel = BaseModel;
            if (GetModel is List<XModelData> ListModel)
                foreach (var Item in ListModel)
                    Item.ParentModel = BaseModel;

            XResult[StorageKey] = RCS_ConvertXModel(GetModel);
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

        #region Use Storage
        public XModel UseSotragePath(string StoragePath, Action<XModelData> UseFunc)
        {
            var GetResult = GetStoragePath<XModelData>(StoragePath);
            UseFunc.Invoke(GetResult);
            return GetBaseModel();
        }
        public XModel UseStoragePath<TResult>(string StoragePath, Action<TResult> UseFunc) where TResult : class
        {
            var GetResult = GetStoragePath<TResult>(StoragePath);
            UseFunc.Invoke(GetResult);
            return GetBaseModel();
        }
        public XModel UseStorage(string GetPath, Action<XModelData> UseFunc, string StorageKey = DefaultStorageKey)
        {
            var GetResult = GetStorage<XModelData>(GetPath, StorageKey);
            UseFunc.Invoke(GetResult);
            return GetBaseModel();
        }
        public XModel UseStorage<TResult>(string GetPath, Action<TResult> UseFunc, string StorageKey = DefaultStorageKey) where TResult : class
        {
            var GetResult = GetStorage<TResult>(GetPath, StorageKey);
            UseFunc.Invoke(GetResult);
            return GetBaseModel();
        }
        #endregion

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
        public XModel AddX_TextMult(string StorageKey = null, string BindKey = null, params Element[] Objs)
        {
            StorageKey ??= DefaultStorageKey;
            var BaseModel = GetBaseModel();
            if (Objs.Length == 0)
                return BaseModel;

            foreach (var Item in Objs)
                BaseAddX_Bind(Item, XModelProperty.Text, BindKey, StorageKey);

            return BaseModel;
        }
        public XModel AddX_ItemsSource(Element Obj, string BindKey = null, string StorageKey = DefaultStorageKey)
        {
            BaseAddX_Bind(Obj, ItemsView.ItemsSourceProperty, BindKey, StorageKey);
            return GetBaseModel();
        }
        public XModel AddX_Click(Element Obj, EventHandler ClickEvent)
        {
            if (Obj is Button Btn)
                Btn.Clicked += ClickEvent;
            else if (Obj is ImageButton ImageBtn)
                ImageBtn.Clicked += ClickEvent;
            else if (Obj is IGestureRecognizers Operate)
            {
                var Tap = new TapGestureRecognizer();
                Tap.Tapped += ClickEvent;
                Operate.GestureRecognizers.Add(Tap);
            }

            return this;
        }
        public XModel AddX_Click(Element Obj, Action ClickEvent)
        {
            if (Obj is Button Btn)
                Btn.Clicked += (s, e) => ClickEvent.Invoke();
            else if (Obj is ImageButton ImageBtn)
                ImageBtn.Clicked += (s, e) => ClickEvent.Invoke();
            else if (Obj is IGestureRecognizers Operate)
            {
                var Tap = new TapGestureRecognizer();
                Tap.Tapped += (s, e) => ClickEvent.Invoke();
                Operate.GestureRecognizers.Add(Tap);
            }

            return this;
        }
        #endregion

        #region Base Method
        private XModel BaseAddX_Bind(Element Obj, BindableProperty BindProperty, string BindKey, string StorageKey)
        {
            BindKey ??= Obj.StyleId;
            var FullBindKey = ConvertBindPath(BindKey, StorageKey);
            Obj.AddX_Bind(BindProperty, FullBindKey);
            return GetBaseModel();
        }
        private XModel BaseAddX_Bind(Element Obj, string BindProperty, string BindKey = null, string StorageKey = DefaultStorageKey)
        {
            BindKey ??= Obj.StyleId;
            var FullBindKey = ConvertBindPath(BindKey, StorageKey);
            Obj.AddX_Bind(BindProperty, FullBindKey);
            return GetBaseModel();
        }
        private string ConvertBindPath(string BindKey, string StorageKey = DefaultStorageKey)
        {
            var JoinBindKey = new List<string>
            {
                "XResult",
                $"[{StorageKey}]",
            };

            if (BindKey != ".")
            {
                var ColumnKeys = BindKey.Split('.').Select(Item => $"[{Item}]");
                JoinBindKey.AddRange(ColumnKeys);
            }

            var FullBindKey = string.Join(".", JoinBindKey);
            return FullBindKey;
        }
        private object RCS_GetStorage(string FullStoragePath, IDictionary<string, object> FindResult)
        {
            var GetPath = FullStoragePath.Split('.')[0];
            var GetResult = FindResult[GetPath];
            if (FullStoragePath.Contains("."))
            {
                var NextPath = FullStoragePath.Replace($"{GetPath}.", "");
                return RCS_GetStorage(NextPath, GetResult as IDictionary<string, object>);
            }
            return GetResult;
        }
        private void RCS_SetSotrage(object SetObject, string FullStoragePath, IDictionary<string, object> SetResult)
        {
            var GetPath = FullStoragePath.Split('.')[0];
            if (!SetResult.ContainsKey(GetPath))
                SetResult.Add(GetPath, CreateXModelData());

            if (FullStoragePath.Contains("."))
            {
                var GetResult = SetResult[GetPath] as IDictionary<string, object>;
                if (GetResult is null)
                {
                    SetResult[GetPath] = CreateXModelData();
                    GetResult = SetResult[GetPath] as IDictionary<string, object>;
                }
                var NextPath = FullStoragePath.Replace($"{GetPath}.", "");
                RCS_SetSotrage(SetObject, NextPath, GetResult);
            }
            else
            {
                SetResult[GetPath] = IsBasicValue(SetObject) ? SetObject : RCS_ConvertXModel(SetObject);
            }
        }
        internal XModelData ConvertXModel(object ConvertObject)
        {
            var Ret = RCS_ConvertXModel(ConvertObject) as XModelData;
            return Ret;
        }
        private object RCS_ConvertXModel(object ConvertObject)
        {
            if (IsBasicValue(ConvertObject))
                return ConvertObject;

            if (ConvertObject is XModelData)
                return ConvertObject;

            var Ret = CreateXModelData();
            if (ConvertObject is IDictionary<string, object> ConvertDic)
            {
                foreach (var Item in ConvertDic)
                    Ret.Add(Item.Key, RCS_ConvertXModel(Item.Value));
            }
            else
            {
                var GetProperties = ConvertObject.GetType().GetProperties();
                foreach (var Item in GetProperties)
                    Ret.Add(Item.Name, RCS_ConvertXModel(Item.GetValue(ConvertObject)));
            }
            return Ret;
        }
        internal virtual bool IsBasicValue(object ConvertObject)
        {
            var Ret = ConvertObject is null ||
                ConvertObject is int ||
                ConvertObject is string ||
                ConvertObject is bool ||
                ConvertObject is DateTime ||
                ConvertObject is System.Collections.IEnumerable ||
                ConvertObject is double ||
                ConvertObject is float ||
                ConvertObject is char ||
                ConvertObject is long;

            return Ret;
        }



        internal XModel GetBaseModel() => ParentXModel ?? this;
        #endregion
    }
}