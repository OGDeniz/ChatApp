using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ChatApp.Models
{
    public enum MessageStatus
    {
        Sent,
        Delivered,
        Read
    }

    public class ChatMessage : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private MessageStatus _status;

        public string Text { get; set; }
        public string Sender { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsMyMessage { get; set; }

        public MessageStatus Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(StatusIcon));
                }
            }
        }

        // Für die Sichtbarkeit der beiden Templates
        public bool ShowMyMessage => IsMyMessage;
        public bool ShowOtherMessage => !IsMyMessage;

        public string FormattedTime => Timestamp.ToString("HH:mm");

        public string StatusIcon
        {
            get
            {
                return Status switch
                {
                    MessageStatus.Sent => "",
                    MessageStatus.Delivered => "✓",
                    MessageStatus.Read => "✓✓",
                    _ => ""
                };
            }
        }

        public ChatMessage(string text, string sender, bool isMyMessage = false)
        {
            Text = text;
            Sender = sender;
            Timestamp = DateTime.Now;
            IsMyMessage = isMyMessage;
            Status = isMyMessage ? MessageStatus.Sent : MessageStatus.Read;
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}