using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfApp1.Converters
{
    public class BoolToFontWeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => (bool)value ? FontWeights.Normal : FontWeights.Bold;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => (FontWeight)value == FontWeights.Normal;
    }

    public class BoolToReadStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => (bool)value ? "Status: Przeczytane" : "Status: Nieprzeczytane";

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

    public class BoolToExpandedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => false;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => true;
    }
}
