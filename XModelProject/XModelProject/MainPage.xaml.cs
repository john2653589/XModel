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

            Model = App.Model
                .AddX_Text(ColumnA)
                //.AddX_Text(ColumnB)
                //.AddX_Text(ColumnC)
                .AddX_TextMult(null, ColumnB, ColumnC)
                .AddX_Click(ColumnB, () =>
                {
                    var C = 1;
                    var GetResult = App.Model.GetStorage<XModelData>();
                    GetResult["ColumnA"] = "Test";
                    //App.Model.SetStorage(new
                    //{
                    //    ColumnA = "欸欸欸欸",
                    //});
                    var A = App.Model.GetStorage();
                    var B = 1;
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
            var Dic = App.Model.CreateDictionary();
            Dic.Add("ColumnA", "Hi");
            Dic.Add("ColumnB", "Is");
            Dic.Add("ColumnC", "Rugal");
            return Dic;
        }
    }
}