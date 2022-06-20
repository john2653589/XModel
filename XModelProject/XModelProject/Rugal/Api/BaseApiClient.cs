using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rugal.Xamarin.XModel;
using System.Linq;
using System.Net.Http;
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
        public virtual async Task<string> GetStringAsync(string Url, object UrlParam)
        {
            Url += $"{ConvertUrlParam(UrlParam)}";
            var ApiRet = await Client.GetAsync(Url);
            var ReadString = await ApiRet.Content.ReadAsStringAsync();
            return ReadString;
        }
        public virtual async Task<string> PostStringAsync(string Url, object PostModel = null)
        {
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