using System.Globalization;

namespace PagoPark.Tools;

public class NumberMonthToNameMonthConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int month)
        {
            DateTime date = new(DateTime.Now.Year, month, 1);
            return date.ToString("MMMM", CultureInfo.CurrentCulture).ToUpper();
        }
        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
