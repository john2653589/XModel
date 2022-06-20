using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Rugal.Api.Client
{
    public interface IApiClient<TDefaultResult>
    {
        public Task<string> GetStringAsync(string Url, object UrlParam = null);
        public Task<string> PostStringAsync(string Url, object PostModel = null);
        public Task<T> GetJsonAsync<T>(string Url, object UrlParam = null);
        public Task<TDefaultResult> GetJsonAsync(string Url, object UrlParam = null);
        public Task<T> PostJsonAsync<T>(string Url, object PostModel = null);
        public Task<TDefaultResult> PostJsonAsync(string Url, object PostModel = null);
        public TResult ConvertObject<TResult>(string JsonString);
    }
}
