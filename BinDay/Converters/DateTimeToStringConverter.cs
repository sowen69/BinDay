using System;
using System.Globalization;
using System.Windows.Data;

namespace BinDay.Converters
{
	public class DateTimeToStringConverter : IValueConverter
	{
		/// <summary>
		/// Modifies the source DateTime before passing it to the target for display in the UI.
		/// </summary>
		/// <returns>
		/// The value as a 'Friendly'string to be passed to the target dependency property.
		/// </returns>
		/// <param name="value">The DateTime being passed to the target.</param>
		/// <param name="targetType">The <see cref="T:System.Type"/> of data expected by the target dependency property.</param>
		/// <param name="parameter">An optional parameter to be used in the converter logic.</param>
		/// <param name="culture">The culture of the conversion.</param>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string returnValue;
			DateTime DateIn= new DateTime();
			if(value is DateTime)
			{
				DateIn = (DateTime) value;
			}
			
			if(DateIn.Day == DateTime.Now.AddDays(1).Day)
			{
				returnValue = Localization.AppResources.Tomorrow;
			}
			else if (DateIn.Day == DateTime.Now.Day)
			{
				returnValue = Localization.AppResources.Today;
			}
			else
			{
				returnValue = DateIn.ToLongDateString();
			}
			return returnValue;
		}

		/// <summary>
		/// Modifies the target data before passing it to the source object.  This method is called only in <see cref="F:System.Windows.Data.BindingMode.TwoWay"/> bindings.
		/// </summary>
		/// <returns>
		/// The value to be passed to the source object.
		/// </returns>
		/// <param name="value">The target data being passed to the source.</param><param name="targetType">The <see cref="T:System.Type"/> of data expected by the source object.</param><param name="parameter">An optional parameter to be used in the converter logic.</param><param name="culture">The culture of the conversion.</param>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
