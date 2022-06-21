using System;
using System.Threading.Tasks;

namespace Rugal.Xamarin.XModel.FuncModel
{
    public class ApiFuncModel
    {
        public bool IsAsync { get; set; }
        public Func<Task<object>> ApiFunc { get; set; }
        public ApiFuncModel() { }
        public ApiFuncModel(bool _IsAsync, Func<Task<object>> _ApiFunc)
        {
            IsAsync = _IsAsync;
            ApiFunc = _ApiFunc;
        }
    }
}
