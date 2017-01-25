using System;
using System.Globalization;
using System.Windows.Data;

namespace BinDay.Converters
{
	public class StringCaseConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var s = value as string;
			
			if (value == null||s=="")
				return value;


			if (s != null) return s.ToUpper();
			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var s = value as string;

			if (value == null || s == "")
				return value;

			if (s != null) return s.ToLower();
			return null;
		}
	}
}