using System.Globalization;
using System.Windows.Data;

namespace WpfApp.Converters
{
    public class UtcToLocalTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime utcDateTime && utcDateTime.Kind == DateTimeKind.Utc)
            {
                return utcDateTime.ToLocalTime();
            }

            // If it's already local or unspecified, return as-is
            return value;
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture
        )
        {
            if (value is DateTime localDateTime)
            {
                return localDateTime.ToUniversalTime();
            }

            return value;
        }
    }
}
