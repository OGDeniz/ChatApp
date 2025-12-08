using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Hubs
{
    public class  ChatHub : Hub
    {
        private const int MAX_MESSAGE_LENGTH = 500;

        // diese Methode wird vom CLient aufgerufen, um eine Nachricht zu senden
        public async Task SendMessage(string sender, string message)
        {
            // Eingabevalidierung
            if ( string.IsNullOrWhiteSpace(message))
                return;
            
            
            if (message.Length > MAX_MESSAGE_LENGTH)
            {
                // optionale: sende eine Fehlermeldung zurück an den Absender
                await Clients.Caller.SendAsync("Error", "Nachricht ist zu lang.");
                return;
            }

            // Sanitize sender Name (sicherheit)
            sender = sender?.Trim().Substring(0, Math.Min(sender.Length, 50)) ?? "Unbekannt";

            // die Nachricht wird an alle verbundenen Clients gesendet
            await Clients.All.SendAsync("ReceiveMessage", sender, message);
        }

        public async Task SendTypingNotification(string userName, bool isTyping)
        {
            // Eingabevalidierung
            if (string.IsNullOrWhiteSpace(userName))
                return;

            userName = userName.Trim().Substring(0, Math.Min(userName.Length, 50));

            // Benachrichtigung an alle Clients außer dem Absender senden
            await Clients.Others.SendAsync("UserTyping", userName, isTyping);
        }

    }
}