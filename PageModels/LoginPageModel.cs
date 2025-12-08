using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ChatApp.PageModels
{
    public partial class LoginPageModel : ObservableObject
    {
        [ObservableProperty]
        private string userName = string.Empty;

        [RelayCommand]
        private async Task Login()
        {
            if (string.IsNullOrWhiteSpace(UserName))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Fehler",
                    "Bitte gib deinen Namen ein!",
                    "OK");
                return;
            }

            // Speichere den Namen für die App
            Preferences.Set("UserName", UserName);

            // Navigiere zur ChatPage
            await Shell.Current.GoToAsync(nameof(ChatPage));
        }
    }
}