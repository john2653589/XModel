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
                .AddX_Text(ColumnB)
                .AddX_Text(ColumnC)
                .AsService<CoprsService>()
                    .AddStorage(Item=> Item.Search())
                .CallStorage(async Item =>
                {
                    await Item.CallStorage();
                })
                .InitBind(this);
        }

        public dynamic GetData()
        {
            var Dic = App.Model.CreateDictionary();
            Dic.Add("ColumnA", "hello");
            Dic.Add("ColumnB", "I am");
            Dic.Add("ColumnC", "Rugal");
            return Dic;
        }
    }
}