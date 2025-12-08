using System.Globalization;

namespace ChatApp.Converters
{
    public class MessageBackgroundConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool isMyMessage)
            {
                // Meine Nachrichten = WhatsApp Grün, andere = Weiß
                return isMyMessage ? Color.FromArgb("#DCF8C6") : Colors.White;
            }
            return Colors.White;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}