using System.Globalization;

namespace CommsManager.Maui.Converters;

public class ActiveStatusConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isActive)
            return isActive ? "Активен" : "Неактивен";

        return "Неизвестно";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}