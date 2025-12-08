using ChatApp.PageModels;

namespace ChatApp.Pages;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
        BindingContext = new LoginPageModel();
    }
}