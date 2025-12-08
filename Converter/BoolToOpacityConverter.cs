using System.Globalization;

namespace ChatApp.Converters
{
    public class BoolToOpacityConverter : IValueConverter
    {
        /// <summary>
        /// Konvertiert einen booleschen Wert in eine Opacity (Deckkraft).
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool enabled)
            {
                return enabled ? 1.0 : 0.5; // Aktiv = 100%, Inaktiv = 50%
            }
            return 0.5;
        }

        /// <summary>
        /// Konvertiert eine Opacity (Deckkraft) zurück in einen booleschen Wert.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}