using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace BinDay.Views
{
	public partial class NextCollection : UserControl
	{
		public static readonly DependencyProperty NextCollectionDateProperty = DependencyProperty.Register("CollectionDate", typeof(DateTime),
			typeof(NextCollection), new PropertyMetadata(NextCollectionDateChanged));

		public DateTime CollectionDate
		{
			get { return (DateTime)this.GetValue(NextCollectionDateProperty); }
			set { this.SetValue(NextCollectionDateProperty, value); }
		}

		private static void NextCollectionDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var collection = (NextCollection)d;
			var collectionDate = (DateTime) e.NewValue;
			var day = String.Format("{0:dddd}", collectionDate);
			var date = collectionDate.Day.ToString(CultureInfo.CurrentCulture);
			var month = String.Format("{0:MMMM}", collectionDate);

			collection.DayOfWeek.Text = day.ToUpper();
			collection.Day.Text = date;
			collection.Month.Text = month.ToUpper();
		}

		public NextCollection()
		{
			InitializeComponent();
		}
	}
}
