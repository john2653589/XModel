using Newtonsoft.Json.Linq;
using Rugal.Xamarin.XModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XModelProject.Api;

namespace XModelProject.Service
{
    public class CoprsService
    {
        private ApiController_A Api;
        public CoprsService(ApiController_A _Api)
        {
            Api = _Api;
        }

        public dynamic Search()
        {
            var Dic = App.Model.CreateDictionary();
            Dic.Add("ColumnA", "hello");
            Dic.Add("ColumnB", "I am");
            Dic.Add("ColumnC", "Rugal");
            return Dic;
        }
    }
}