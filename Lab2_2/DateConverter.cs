using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Lab2_2 {
	[ValueConversion(typeof(DateTime), typeof(string))]
	class DateConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			return ((DateTime) value).ToString();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			return DateTime.Parse(value as string);
		}
	}
}
