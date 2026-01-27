using System.Globalization;

namespace CommsManager.Maui.Converters;

public class OrderStatusColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string status)
        {
            return status switch
            {
                "New" => Color.FromArgb("#2196F3"),       // Синий
                "InProgress" => Color.FromArgb("#FF9800"), // Оранжевый
                "Pending" => Color.FromArgb("#9C27B0"),    // Фиолетовый
                "Completed" => Color.FromArgb("#4CAF50"),  // Зеленый
                "Cancelled" => Color.FromArgb("#F44336"),  // Красный
                _ => Color.FromArgb("#757575")             // Серый
            };
        }
        return Color.FromArgb("#757575");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}



