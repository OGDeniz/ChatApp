using Microsoft.AspNetCore.SignalR.Client;

namespace ChatApp.Services
{
    public class ChatService
    {
        private HubConnection _hubConnection;

        // Event für Empfangene Nachrichten
        public event Action<string, string>? MessageReceived;

        // Typing-Benachrichtigung Event
        public event Action<string, bool>? UserTypingChanged;

        // Verbindung zum Server herstellen
        public async Task ConnectAsync(string serverUrl)
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl($"{serverUrl}/chathub")
                .WithAutomaticReconnect()
                .Build();

            // Empfangene Nachrichten verarbeiten
            _hubConnection.On<string, string>("ReceiveMessage", (sender, message) =>
            {
                MessageReceived?.Invoke(sender, message);
            });

            // Typing-Benachrichtigungen verarbeiten
            _hubConnection.On<string, bool>("UserTyping", (userName, isTyping) =>
            {
                UserTypingChanged?.Invoke(userName, isTyping);
            });

            await _hubConnection.StartAsync();
        }

        // Nachricht senden
        public async Task SendMessageAsync(string sender, string message)
        {
            if (_hubConnection != null)
            {
                await _hubConnection.InvokeAsync("SendMessage", sender, message);
            }
        }

        // Typing-Benachrichtigung senden
        public async Task SendTypingNotificationAsync(string userName, bool isTyping)
        {
            if (_hubConnection?.State == HubConnectionState.Connected)
            {
                await _hubConnection.SendAsync("SendTypingNotification", userName, isTyping);
            }
        }

        // Verbindung trennen
        public async Task DisconnectAsync()
        {
            if (_hubConnection != null)
            {
                await _hubConnection.StopAsync(); // die Stop-Methode beendet die Verbindung zum Hub
                await _hubConnection.DisposeAsync(); // die Dispose-Methode bewirken, dass alle Ressourcen freigegeben werden
            }
        }
    }
}
