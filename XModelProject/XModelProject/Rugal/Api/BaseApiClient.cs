using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rugal.Xamarin.XModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Rugal.Api.Client
{
    public abstract class BaseApiClient<TDefaultResult> : IApiClient<TDefaultResult>
    {
        private static HttpClient Client;
        public BaseApiClient()
        {
            ClientInit();
        }
        private static void ClientInit()
        {
            Client ??= new HttpClient();
        }

        #region Get Post Method
        public virtual async Task<string> PostFileAsync(string Url, object UrlParam, string FileName)
        {
            Url += $"{ConvertUrlParam(UrlParam)}";
             
            var Info = new FileInfo(FileName);

            var Buffer = File.ReadAllBytes(FileName);
            using var Memory = new MemoryStream(Buffer);
            var StreamContent = new StreamContent(Memory);
            StreamContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");

            using var FormContent = new MultipartFormDataContent
            {
                { StreamContent, "File", Info.Name }
            };
            var ApiRet = await Client.PostAsync(Url, FormContent);
            var ReadString = await ApiRet.Content.ReadAsStringAsync();
            return ReadString;
        }
        public virtual async Task<TResult> PostFileAsync<TResult>(string Url, object UrlParam, string FileName)
        {
            var ApiRet = await PostFileAsync(Url, UrlParam, FileName);
            var Ret = ConvertObject<TResult>(ApiRet);
            return Ret;
        }
        public virtual async Task<string> GetStringAsync(string Url, object UrlParam)
        {
            try
            {
                Url += $"{ConvertUrlParam(UrlParam)}";
                var ApiRet = await Client.GetAsync(Url);
                var ReadString = await ApiRet.Content.ReadAsStringAsync();
                return ReadString;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public virtual async Task<string> PostStringAsync(string Url, object PostModel = null)
        {
            PostModel ??= new { };
            var JsonString = JsonConvert.SerializeObject(PostModel);
            var JsonContent = new StringContent(JsonString, Encoding.UTF8, "application/json");
            var ApiRet = await Client.PostAsync(Url, JsonContent);
            var ReadString = await ApiRet.Content.ReadAsStringAsync();
            return ReadString;
        }
        public virtual async Task<TResult> GetJsonAsync<TResult>(string Url, object UrlParam = null)
        {
            var ApiRet = await GetStringAsync(Url, UrlParam);
            var Ret = ConvertObject<TResult>(ApiRet);
            return Ret;
        }
        public virtual async Task<TDefaultResult> GetJsonAsync(string Url, object UrlParam = null)
        {
            var ApiRet = await GetStringAsync(Url, UrlParam);
            var Ret = ConvertObject<TDefaultResult>(ApiRet);
            return Ret;
        }
        public virtual async Task<TResult> PostJsonAsync<TResult>(string Url, object PostModel = null)
        {
            var ApiRet = await PostStringAsync(Url, PostModel);
            var Ret = ConvertObject<TResult>(ApiRet);
            return Ret;
        }
        public virtual async Task<TDefaultResult> PostJsonAsync(string Url, object PostModel = null)
        {
            var ApiRet = await PostStringAsync(Url, PostModel);
            var Ret = ConvertObject<TDefaultResult>(ApiRet);
            return Ret;
        }
        #endregion

        #region Convert
        public virtual TResult ConvertObject<TResult>(string JsonString)
        {
            var GetObject = JsonConvert.DeserializeObject<TResult>(JsonString);
            return GetObject;
        }
        internal virtual string ConvertUrlParam(object UrlParam)
        {
            if (UrlParam == null)
                return "";

            var GetProperties = UrlParam.GetType().GetProperties();
            var GetKeyValue = GetProperties
                .Select(Item => $"{Item.Name}={Item.GetValue(UrlParam)}")
                .ToArray();

            var GetUrlParam = $"?{string.Join("&", GetKeyValue)}";
            return GetUrlParam;
        }

        #endregion
    }
}