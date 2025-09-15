using System.Globalization;

namespace CommsManager.Converters;

public class DeadlineColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DateTime deadline)
        {
            var timeLeft = deadline - DateTime.Now;
            
            if (timeLeft.TotalDays < 0)
                return Color.FromArgb("#FF5252");
            if (timeLeft.TotalDays < 1)
                return Color.FromArgb("#FFB74D");
            if (timeLeft.TotalDays < 3)
                return Color.FromArgb("#FFD54F");
        }
        
        return Color.FromArgb("#66BB6A");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
