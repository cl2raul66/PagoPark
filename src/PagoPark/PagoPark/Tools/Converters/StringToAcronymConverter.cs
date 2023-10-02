using System.Globalization;

namespace PagoPark.Tools;

public class StringToAcronymConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string str && !string.IsNullOrEmpty(str))
        {
            return string.Concat(str.Split(' ').Select(x => x.FirstOrDefault())).ToUpper();
        }
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

