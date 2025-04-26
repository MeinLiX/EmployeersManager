using System.Globalization;
using System.Windows.Data;

namespace EmployeersManager.Infrastructure.Converters;

public class BoolToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue && parameter is string options)
        {
            var parts = options.Split(';');
            return boolValue ? parts[0] : (parts.Length > 1 ? parts[1] : string.Empty);
        }
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
