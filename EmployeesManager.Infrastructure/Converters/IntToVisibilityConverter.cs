using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace EmployeesManager.Infrastructure.Converters;

public class IntToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        int compareValue = int.Parse(parameter.ToString());
        int intValue = (int)value;

        return intValue == compareValue ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
