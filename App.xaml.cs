namespace ChatApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();

            // Prüfe ob User bereits eingeloggt ist
            CheckLoginStatus();
        }

        private async void CheckLoginStatus()
        {
            await Task.Delay(100); // Simuliere Ladezeit

            string userName = Preferences.Get("Username", string.Empty);

            if (string.IsNullOrWhiteSpace(userName))
            {
                // Kein Name gespeichert, zeige LoginPage
                await Shell.Current.GoToAsync("///LoginPage");
            }
            else {

                // Name vorhanden, zeige ChatPage
                await Shell.Current.GoToAsync(nameof(Pages.ChatPage));
            }
        }

    }
}