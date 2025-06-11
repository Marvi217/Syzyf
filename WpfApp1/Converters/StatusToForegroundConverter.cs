using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using WpfApp1.Models;

namespace WpfApp1.Converters
{
    public class StatusToForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ProjectStatus status)
            {
                switch (status)
                {
                    case ProjectStatus.Planned:
                        return Brushes.Orange;
                    case ProjectStatus.InProgress:
                        return Brushes.DarkGreen;
                    case ProjectStatus.Completed:
                        return Brushes.DarkBlue;
                }
            }
            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
