using Rugal.Xamarin.BindingModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Rugal.Xamarin.XModel
{
    public class XModelData : BaseObservableDictionary<object>
    {
        public XModel ParentModel { get; set; }
        public override object this[string Key]
        {
            get => RCS_GetValue(Key, Model);
            set => RCS_SetValue(Key, value, Model);
        }
        public XModelData() { }
        public override void OnChange([CallerMemberName] string PropertyName = null)
        {
            ParentModel?.OnChange("XResult");
        }
        private object RCS_GetValue(string FullPath, IDictionary<string, object> FindResult)
        {
            var SplitArray = FullPath.Split('.');
            var GetPath = SplitArray[0];
            if (!FindResult.ContainsKey(GetPath))
            {
                FindResult.Add(GetPath, CreateNewData());
                OnChange(GetPath);
            }

            if (FullPath.Contains("."))
            {
                var NextResult = FindResult[GetPath] as IDictionary<string, object>;
                var NextPath = string.Join(".", SplitArray.Skip(1));
                return RCS_GetValue(NextPath, NextResult);
            }
            else
                return FindResult[GetPath];
        }
        private void RCS_SetValue(string SetPath, object SetValue, IDictionary<string, object> FindResult)
        {
            var SplitArray = SetPath.Split('.');
            var GetPath = SplitArray[0];

            if (SetPath.Contains("."))
            {
                if (!FindResult.ContainsKey(GetPath))
                    FindResult.Add(GetPath, CreateNewData());

                if (FindResult[GetPath] is not IDictionary<string, object>)
                    FindResult[GetPath] = CreateNewData();

                var NextResult = FindResult[GetPath] as IDictionary<string, object>;
                var NextPath = string.Join(".", SplitArray.Skip(1));
                RCS_SetValue(NextPath, SetValue, NextResult);
            }
            else
            {
                FindResult[GetPath] = RCS_ConvertXModel(SetValue);
                OnChange(GetPath);
            }
        }
        internal object RCS_ConvertXModel(object ConvertObject)
        {
            if (IsBasicValue(ConvertObject))
                return ConvertObject;

            if (ConvertObject is XModelData)
                return ConvertObject;

            if (IsArray(ConvertObject, out var OutArray))
                return OutArray;

            var Ret = CreateNewData();
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
                ConvertObject is double ||
                ConvertObject is float ||
                ConvertObject is char ||
                ConvertObject is long;

            return Ret;
        }
        internal virtual bool IsArray(object ConvertObject, out IEnumerable<object> OutArray)
        {
            OutArray = null;
            if (ConvertObject is System.Collections.IEnumerable ArrayObject)
            {
                var OutModel = new List<object>();
                foreach (var item in ArrayObject)
                    OutModel.Add(RCS_ConvertXModel(item));

                OutArray = OutModel;
                return true;
            }
            return false;
        }

        private XModelData CreateNewData()
        {
            if (ParentModel is not null)
                return ParentModel.CreateXModelData();

            return new XModelData();
        }
    }
}