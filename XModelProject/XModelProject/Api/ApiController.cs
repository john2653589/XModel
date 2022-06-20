using Rugal.Api.Client;
using Rugal.Api.Controller;
using Rugal.Xamarin.XModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace XModelProject.Api
{
    public class ApiController_A : BaseApiController<ControllerA_Get, ControllerA_Post, XModelData>
    {
        public ApiController_A()
        {
            Domain = "http://domain.com.tw";
            Controller = "area/controller";
            Client = new ApiClient();
        }
    }
}