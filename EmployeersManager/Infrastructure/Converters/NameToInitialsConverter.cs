using System.Globalization;
using System.Windows.Data;

namespace EmployeersManager.Infrastructure.Converters;

public class NameToInitialsConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null) return string.Empty;

        string name = (string)value;
        if (string.IsNullOrWhiteSpace(name)) return string.Empty;

        string[] nameParts = name.Split([' '], StringSplitOptions.RemoveEmptyEntries);
        string initials = string.Empty;

        foreach (var part in nameParts)
        {
            if (!string.IsNullOrEmpty(part) && part.Length > 0)
            {
                initials += part[0];
            }

            if (initials.Length >= 2) break;
        }

        return initials.ToUpper();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
