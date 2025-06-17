using System;
using System.Globalization;
using System.Windows.Data;
using WpfApp1.Model;  // lub tam, gdzie masz klasę Employee

namespace WpfApp1.Converters
{
    public class EmployeeToFullNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Employee employee)
            {
                return $"{employee.FirstName} {employee.LastName}";
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
