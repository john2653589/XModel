using Rugal.Xamarin.BindingModel;
using Rugal.Xamarin.XModel.BindingProperty;
using Xamarin.Forms;

namespace Rugal.Xamarin.XModel.Extention
{
    public static class XModelExtention
    {
        public static T AddX_Bind<T>(this T Obj, BindableProperty BindProperty, string BindKey = null) where T : Element
        {
            BindKey ??= Obj.StyleId;
            Obj.SetBinding(BindProperty, BindKey, BindingMode.TwoWay);
            return Obj;
        }
        public static T AddX_Bind<T>(this T Obj, string BindProperty, string BindKey = null) where T : Element
        {
            var GetProperty = Obj.GetBindableProperty(BindProperty);
            if (GetProperty is null)
                return null;

            return Obj.AddX_Bind(GetProperty, BindKey);
        }
        public static T AddX_Text<T>(this T Obj, string BindKey = null) where T : Element
            => Obj.AddX_Bind(XModelProperty.Text, BindKey);
        public static BindableProperty GetBindableProperty<T>(this T Obj, string PropertyName) where T : Element
        {
            var GetType = Obj.GetType();
            var GetProperty = GetType.GetField(PropertyName);
            var GetValue = GetProperty.GetValue(Obj) as BindableProperty;
            return GetValue;
        }
    }
}