using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using BinDay.Model;

namespace BinDay.Converters
{
	public class ColourSwatchToSolidColorBrushConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var pickedSwatch = new ColourSwatch();
			pickedSwatch.Brush = new SolidColorBrush(Colors.Black);
			pickedSwatch.ColourName = "Black";
			
			if (value == null)
				return pickedSwatch;
			// gets a SolidColorBrush
			// Convert to ColourSwatch
			
			
			var brush = value as SolidColorBrush;
			pickedSwatch.Brush = brush;

			if (brush != null) pickedSwatch.ColourName = getColourName(brush.Color.ToString());

			return pickedSwatch;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var pickedSwatch = value as ColourSwatch;

			if (pickedSwatch != null)
			{
				SolidColorBrush brush = pickedSwatch.Brush;
			
				return brush;
			}
			return null;
		}


		private string getColourName(string hex)
		{
			var name="";
			switch (hex)
			{
				case "":
				case "#FF000000":
					name = "Black";
					break;
				case "#FF808080":
					name = "Grey";
					break;
				case "#FF0000FF":
					name = "Blue";
					break;
				case "#FF008000":
					name = "Green";
					break;
				case "#FFFF0000":
					name = "Red";
					break;
				case "#FFA52A2A":
					name = "Brown";
					break;

				case "#FFFFA500":
					name = "Orange";
					break;
				case "#FF800080":
					name = "Purple";
					break;
				case "#FFFFFF00":
					name = "Yellow";
					break;
				case "#FFFF00FF":
					name = "Magenta";
					break;


			}

			return name;
		}
	}
}