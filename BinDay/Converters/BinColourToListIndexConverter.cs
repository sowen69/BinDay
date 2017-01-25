using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using BinDay.Model;

namespace BinDay.Converters
{
	public class BinColourToListIndexConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			int index;

			if (value == null)
				return index = 0;

			var pickedSwath = value as ColourSwatch;
			
			var brush = pickedSwath.Brush.Color.ToString();

			index = getListIndex(brush);

			return index;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return 0;

			var index = (int)value;

			var pickedSwatch = getColourSwatch(index);
			return pickedSwatch;
		}

		private int getListIndex(string hex)
		{
			var index = 0;
			switch (hex)
			{
				case "":
				case "#FF000000":
					index =0;
					break;
				case "#FF808080":
					index =1;
					break;
				case "#FF0000FF":
					index =2;
					break;
				case "#FF008000":
					index = 3;
					break;
				case "#FFFF0000":
					index = 4;
					break;
				// MS Brown 
				//case "#FFA52A2A":
				case "#FF694101":
					index = 5;
					break;
				case "#FFFFA500":
					index = 6;
					break;
				case "#FF800080":
					index = 7;
					break;
				case "#FFFFFF00":
					index = 8;
					break;
				case "#FFFF00FF":
					index = 9;
					break;
			}

			return index;
		}

		private ColourSwatch getColourSwatch(int index)
		{
			ColourSwatch swatch = null;
			
			switch (index)
			{
				case 0:
					swatch = new ColourSwatch { ColourName = "Black", ColourLabel = Localization.AppResources.Black, Brush = new SolidColorBrush(Colors.Black) };
					break;
				case 1:
					swatch = new ColourSwatch { ColourName="Gray", ColourLabel= Localization.AppResources.Grey, Brush = new SolidColorBrush(Colors.Gray) };
					break;
				case 2:
					swatch = new ColourSwatch { ColourName="Blue", ColourLabel= Localization.AppResources.Blue, Brush = new SolidColorBrush(Colors.Blue) };
					break;
				case 3:
					swatch = new ColourSwatch { ColourName="Green", ColourLabel= Localization.AppResources.Green, Brush = new SolidColorBrush(Colors.Green) };
					break;
				case 4:
					swatch = new ColourSwatch { ColourName="Red", ColourLabel= Localization.AppResources.Red, Brush = new SolidColorBrush(Colors.Red) };
					break;
				case 5:
					swatch = new ColourSwatch { ColourName="Brown", ColourLabel= Localization.AppResources.Brown, Brush = new SolidColorBrush(Color.FromArgb(255,105,65,1)) };
					//swatch = new ColourSwatch { ColourName = "Brown", ColourLabel = Localization.AppResources.Brown, Brush = new SolidColorBrush(Colors.Brown) };
					break;

				case 6:
					swatch = new ColourSwatch { ColourName="Orange", ColourLabel= Localization.AppResources.Orange, Brush = new SolidColorBrush(Colors.Orange) };
					break;
				case 7:
					swatch = new ColourSwatch { ColourName="Purple", ColourLabel= Localization.AppResources.Purple, Brush = new SolidColorBrush(Colors.Purple) };
					break;
				case 8:
					swatch = new ColourSwatch { ColourName="Yellow", ColourLabel= Localization.AppResources.Yellow, Brush = new SolidColorBrush(Colors.Yellow) };
					break;
				case 9:
					swatch = new ColourSwatch { ColourName="Magenta", ColourLabel= Localization.AppResources.Pink, Brush = new SolidColorBrush(Colors.Magenta) };
					break;
			}

			return swatch;
}
	}
}
