using ChatApp.Models;
using ChatApp.PageModels;

namespace ChatApp.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageModel model)
        {
            InitializeComponent(); 
            BindingContext = model;
        }
    }
}