using System.Globalization;

namespace CommsManager.Converters;

public class BooleanToTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue && parameter is string texts)
        {
            var parts = texts.Split('|');
            return boolValue ? parts[0] : (parts.Length > 1 ? parts[1] : parts[0]);
        }
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}