﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BinDay.Converters
{
	public class BooleanToVisibilityConverter : IValueConverter
	{

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{

			if (value == null)

				return Visibility.Collapsed;



			var isVisible = (bool)value;

			return isVisible ? Visibility.Visible : Visibility.Collapsed;

		}



		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{

			var visiblity = (Visibility)value;

			return visiblity == Visibility.Visible;
		}

	}
}