using System.Globalization;
using System.Windows.Data;

namespace WinMix;

public class TimeSpanToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is TimeSpan ts)
            return ts.ToString(@"mm\:ss");
        return "00:00";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string s && TimeSpan.TryParseExact(s, @"mm\:ss", culture, out var ts))
            return ts;
        return TimeSpan.Zero;
    }
}
