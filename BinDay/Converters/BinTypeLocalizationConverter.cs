using System;
using System.Globalization;
using System.Windows.Data;

namespace BinDay.Converters
{
	public class BinTypeLocalizationConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var bin = value.ToString();

			if (bin == "Wheelie")
				return Localization.AppResources.Wheelie;

			if (bin == "Crate")
				return Localization.AppResources.Crate;

			if (bin == "Bag")
				return Localization.AppResources.Bag;

			return "no bin";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
