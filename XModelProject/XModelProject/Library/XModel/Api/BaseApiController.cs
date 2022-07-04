using Newtonsoft.Json.Linq;
using Rugal.Api.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Rugal.Api.Controller
{
    public abstract class BaseApiController<TGetAction, TPostAction, TDefaultResult> :
        IApiController<TGetAction, TPostAction, TDefaultResult>
        where TGetAction : Enum where TPostAction : Enum
    {
        public string Domain { get; set; }
        public string Controller { get; set; }
        public IApiClient<TDefaultResult> Client { get; set; }
        public BaseApiController(string _Domain, string _Controller, IApiClient<TDefaultResult> _Client) : this()
        {
            Domain = _Domain.TrimEnd('/');
            Controller = _Controller.Trim('/');
            Client = _Client;
        }
        public BaseApiController() { }

        #region Get Post Base Method
        public virtual async Task<string> PostFileAsync(TPostAction ActionName, object UrlParam, string FileName)
        {
            var FullUrl = GetUrl(ActionName);
            return await Client.PostFileAsync(FullUrl, UrlParam, FileName);
        }
        public virtual async Task<TResult> PostFileAsync<TResult>(TPostAction ActionName, object UrlParam, string FileName)
        {
            var FullUrl = GetUrl(ActionName);
            var Ret = await Client.PostFileAsync<TResult>(FullUrl, UrlParam, FileName);
            return Ret;
        }

        public async Task<TResult> GetApi<TResult>(TGetAction ActionName, object UrlParam = null)
        {
            var FullUrl = GetUrl(ActionName);
            return await Client.GetJsonAsync<TResult>(FullUrl, UrlParam);
        }
        public async Task<TDefaultResult> GetApi(TGetAction ActionName, object UrlParam = null)
        {
            var FullUrl = GetUrl(ActionName);
            return await Client.GetJsonAsync(FullUrl, UrlParam);
        }

        public async Task<TResult> PostApi<TResult>(TPostAction ActionName, object PostModel = null)
        {
            var FullUrl = GetUrl(ActionName);
            return await Client.PostJsonAsync<TResult>(FullUrl, PostModel);
        }
        public async Task<TDefaultResult> PostApi(TPostAction ActionName, object PostModel = null)
        {
            var FullUrl = GetUrl(ActionName);
            return await Client.PostJsonAsync(FullUrl, PostModel);
        }
        #endregion

        #region Convert
        public virtual string GetUrl(Enum ActionName)
        {
            var GetFullUrl = string.Join("/", Domain, Controller, ActionName);
            return GetFullUrl;
        }
        #endregion
    }
}