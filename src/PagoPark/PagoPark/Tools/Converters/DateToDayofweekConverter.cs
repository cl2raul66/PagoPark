using System.Globalization;

namespace PagoPark.Tools;

public class DateToDayofweekConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DateTime fecha && fecha.Year > DateTime.Now.Year - 1)
        {
            return fecha.ToString("dddd", CultureInfo.CurrentCulture).ToUpper();
        }
        //throw new InvalidOperationException("El valor debe ser una fecha");
        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
