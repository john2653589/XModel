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

        public async Task<dynamic> Search()
        {
            return await Api.PostApi(ControllerA_Post.Search, new
            {
                id = 1
            });
        }
    }
}