using Rugal.Xamarin.BindingModel;
using System.Runtime.CompilerServices;

namespace Rugal.Xamarin.XModel
{
    public class XModelData : BaseObservableDictionary<string, object>
    {
        public XModel ParentModel { get; set; }
        public XModelData() { }
        public override void OnChange([CallerMemberName] string PropertyName = null)
        {
            ParentModel?.OnChange("XResult");
        }
    }
}