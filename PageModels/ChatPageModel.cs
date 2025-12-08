using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ChatApp.Models;
using ChatApp.Constants;
using ChatApp.Services;

namespace ChatApp.PageModels
{
    public partial class ChatPageModel : ObservableObject
    {
        private readonly ChatService _chatService;
        private readonly System.Timers.Timer? _typingTimer;
        private bool _isCurrentlyTyping = false;

        public ObservableCollection<ChatMessage> Messages { get; set; }

        [ObservableProperty]
        private string newMessageText = string.Empty;

        [ObservableProperty]
        private string userName = Preferences.Get("UserName", "Ich");

        [ObservableProperty]
        private string typingIndicatorText = string.Empty;

        [ObservableProperty]
        private bool isTypingIndicatorVisible = false;

        [ObservableProperty]
        private int characterCount = 0;

        [ObservableProperty]
        private string characterCountText = "0/500";

        [ObservableProperty]
        private Color characterCountColor = Colors.Gray;

        [ObservableProperty]
        private bool isSendButtonEnabled = false;

        // Konstruktor von ChatPageModel
        public ChatPageModel(ChatService chatService)
        {
            _chatService = chatService;
            Messages = new ObservableCollection<ChatMessage>();

            // Event-Handler für empfangene Nachrichten
            _chatService.MessageReceived += OnMessageReceived;

            //Typing-Status Event-Handler
            _chatService.UserTypingChanged += OnUserTypingChanged;

            // Timer für das Stoppen des Tippens (3sek)
            _typingTimer = new System.Timers.Timer(3000);
            _typingTimer.Elapsed += (s, e) => 
            {
                StopTyping();
            };
            _typingTimer.AutoReset = false;

            // Verbindung zum Server herstellen
            ConnectToServer();
        }

        private async void ConnectToServer()
        {
            try
            {
                await _chatService.ConnectAsync("https://localhost:7235");
                System.Diagnostics.Debug.WriteLine("✅ ERFOLGREICH mit Server verbunden!");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Verbindungsfehler: {ex.Message}");
            }
        }

        private void OnMessageReceived(string sender, string message)
        {
            // UI Thread verwenden für UI-Updates
            MainThread.BeginInvokeOnMainThread(() =>
            {
                bool isMyMessage = sender == UserName;
                Messages.Add(new ChatMessage(message, sender, isMyMessage));
            });
        }

        private void OnUserTypingChanged(string typingUser, bool isTyping)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (isTyping)
                {
                    TypingIndicatorText = $"{userName} schreibt...";
                    IsTypingIndicatorVisible = true;
                    System.Diagnostics.Debug.WriteLine($"✅ Set Visible: {TypingIndicatorText}");
                }
                else
                {
                    IsTypingIndicatorVisible = false;
                    TypingIndicatorText = string.Empty;
                    System.Diagnostics.Debug.WriteLine($"❌ Set Hidden");
                }
            });
        }

        // wird aufgerufen, wenn der Benutzer zu tippen beginnt
        partial void OnNewMessageTextChanged(string value)
        {
            if(!string.IsNullOrWhiteSpace(value))
            {
                StartTyping();
            }   
            else
            {
                StopTyping();
            }

            UpdateCharacterCount(value);
        }

        private void UpdateCharacterCount(string text)
        {
            CharacterCount = text?.Length ?? 0;
            CharacterCountText = $"{CharacterCount}/{ChatConstants.MAX_MESSAGE_LENGTH}";

            // Senden-Button nur aktivieren wenn Text vorhanden und unter Limit
            IsSendButtonEnabled = CharacterCount > 0 && CharacterCount <= ChatConstants.MAX_MESSAGE_LENGTH;

            // Farbe basierend auf Länge
            if (CharacterCount > ChatConstants.MAX_MESSAGE_LENGTH)
            {
                // Rot - Limit überschritten
                CharacterCountColor = Color.FromArgb("#FF3B30");
            }
            else if (CharacterCount >= ChatConstants.WARNING_THRESHOLD)
            {
                // Orange - Warnung
                CharacterCountColor = Color.FromArgb("#FF9500");
            }
            else
            {
                // Grau - Normal
                CharacterCountColor = Color.FromArgb("#8E8E93");
            }
        }

        private async void StartTyping()
        {
            if (!_isCurrentlyTyping)
            {
                _isCurrentlyTyping = true;
                await _chatService.SendTypingNotificationAsync(UserName, true);
            }

            // Timer zurücksetzen
            _typingTimer.Stop();
            _typingTimer.Start();
        }

        private async void StopTyping()
        {
            System.Diagnostics.Debug.WriteLine($"⏱️ StopTyping aufgerufen - _isCurrentlyTyping: {_isCurrentlyTyping}");

            if (_isCurrentlyTyping)
            {
                _isCurrentlyTyping = false;
                await _chatService.SendTypingNotificationAsync(UserName, false);
            }

            // Timer stoppen
            _typingTimer?.Stop();
        }

        [RelayCommand]
        private async Task SendMessage()
        {
            if (!string.IsNullOrWhiteSpace(NewMessageText))
            {
                if (string.IsNullOrWhiteSpace(NewMessageText))
                    return;

                if (NewMessageText.Length > ChatConstants.MAX_MESSAGE_LENGTH)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Nachricht zu lang",
                        $"Nachrichten dürfen maximal {ChatConstants.MAX_MESSAGE_LENGTH} Zeichen lang sein.",
                        "OK");
                    return;
                }

                try
                {
                    // Typing stoppen vor dem Senden
                    StopTyping();

                    // Nachricht an Server senden
                    await _chatService.SendMessageAsync(UserName, NewMessageText);
                    NewMessageText = string.Empty;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Fehler beim Senden: {ex.Message}");
                }
            }
        }
    }
}