using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace EmployeersManager.Infrastructure.Converters;

public class BoolToColorConverter : IValueConverter
{
    public object TrueValue { get; set; } = Brushes.White;
    public object FalseValue { get; set; } = Brushes.Gray;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            if (parameter != null && parameter.ToString() == "Status")
            {
                return boolValue ? Brushes.Green : Brushes.Red;
            }

            return boolValue ? TrueValue : FalseValue;
        }

        return TrueValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
