using Newtonsoft.Json.Linq;
using Rugal.Api.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Rugal.Api.Controller
{
    public interface IApiController<TGetAction, TPostAction, TDefaultResult>
        where TGetAction : Enum 
        where TPostAction : Enum
    {
        public string Domain { get; set; }
        public string Controller { get; set; }
        public IApiClient<TDefaultResult> Client { get; set; }
        public string GetUrl(Enum ActionName);
        public Task<TResult> GetApi<TResult>(TGetAction ActionName, object UrlParam = null);
        public Task<TDefaultResult> GetApi(TGetAction ActionName, object UrlParam = null);
        public Task<TResult> PostApi<TResult>(TPostAction ActionName, object PostModel = null);
        public Task<TDefaultResult> PostApi(TPostAction ActionName, object PostModel = null);
    }
}