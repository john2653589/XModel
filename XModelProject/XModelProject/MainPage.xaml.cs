using Rugal.Xamarin.XModel;
using Rugal.Xamarin.XModel.Extention;
using System;
using Xamarin.Forms;
using XModelProject.Api;
using XModelProject.Service;

namespace XModelProject
{
    public partial class MainPage : ContentPage
    {
        private XModel Model;
        public dynamic AA { get; set; }
        public MainPage()
        {
            InitializeComponent();

            ColumnA.AddX_Text("XResult.[UserInfo].[Name2].[Name3]");
            //ColumnA.SetBinding(Label.TextProperty, "XResult.[UserInfo].[Name2].[Name3]");

            Model = App.Model
                //.AddX_Text(ColumnA, "Name2.Name3", "UserInfo")
                .AddX_TextMult("UserInfo", "Name", ColumnB, ColumnC)
                .AddX_Click(BtnSet, () =>
                {
                    App.Model.SetStorage("涂光", "Name", "UserInfo");
                    //var SetData = new
                    //{
                    //    Name = "劉辰弘",
                    //    Name2 = new
                    //    {
                    //        Name3 = "黃楷"
                    //    },
                    //};
                    //App.Model.SetStorage(SetData, null, "UserInfo");
                })
                .AddX_Click(BtnGet, () =>
                {
                    var GetData = App.Model.GetStoragePath<XModelData>("UserInfo");
                    GetData["Name2.Name3"] = "哈哈哈";
                    //App.Model.SetStorage("涂", "Name2.Name3", "UserInfo");

                    var GetHa = GetData["Name2.Name3"];
                    var S = 1;
                })
                //.AddStorage(() => SetData())
                .AsService<CoprsService>()
                    .AddStorage(Item => Item.Search())
                .CallStorage(async Item =>
                {
                    await Item.CallStorage();
                })
                .InitBind(this);
        }

        public dynamic SetData()
        {
            var Dic = App.Model.CreateXModelData();
            Dic.Add("ColumnA", "Hi");
            Dic.Add("ColumnB", "Is");
            Dic.Add("ColumnC", "Rugal");
            return Dic;
        }
    }
}