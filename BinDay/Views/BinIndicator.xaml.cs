using System.Windows;
using System.Windows.Media;

namespace BinDay
{
	public partial class BinIndicator
	{
		public static readonly DependencyProperty BinBaseColourProperty = DependencyProperty.Register("BinColour", typeof(SolidColorBrush),
			typeof(BinIndicator), new PropertyMetadata(BinColourChanged));

		public static readonly DependencyProperty BinTypeProperty = DependencyProperty.Register("TypeOfBin", typeof(string),
			typeof(BinIndicator), 
			new PropertyMetadata(TypeOfBinChanged));

		public SolidColorBrush BinColour
		{
			get 
			{
				//Debug.WriteLine("BinColour getter= " + (SolidColorBrush)this.GetValue(BinBaseColourProperty)); 
				return (SolidColorBrush)GetValue(BinBaseColourProperty); 
			}
			set
			{
				//Debug.WriteLine("BinColour setter= "+value); 
				SetValue(BinBaseColourProperty, value);
			}
		}

		public string TypeOfBin
		{
			get
			{
				//Debug.WriteLine("BinType getter= " + (string)this.GetValue(BinTypeProperty));
				return (string)GetValue(BinTypeProperty);
			}
			set
			{
				//Debug.WriteLine("BinType Setter= " + value);
				SetValue(BinTypeProperty, value);
			}
		}

		private static void BinColourChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			//Debug.WriteLine("Colour DepPropChanged= " + e.NewValue);
			var indicator = (BinIndicator)sender;
			indicator.BinBaseColour.Fill = e.NewValue as SolidColorBrush;
			indicator.BagBaseColour.Fill = e.NewValue as SolidColorBrush;
			indicator.CrateBaseColour.Fill = e.NewValue as SolidColorBrush;
		}

		private static void TypeOfBinChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			//Debug.WriteLine("Type DepPropChanged= " + e.NewValue.GetType().ToString());
			var indicator = (BinIndicator)sender;
			var bt = (string)e.NewValue;
			//Debug.WriteLine(bt.ToString());
			switch (bt)
			{
				case "Bag":
					indicator.BagGraphic.Visibility = Visibility.Visible;
					indicator.CrateGraphic.Visibility = Visibility.Collapsed;
					indicator.WheelieBinGraphic.Visibility = Visibility.Collapsed;
					break;
				case "Crate":
					indicator.BagGraphic.Visibility = Visibility.Collapsed;
					indicator.CrateGraphic.Visibility = Visibility.Visible;
					indicator.WheelieBinGraphic.Visibility = Visibility.Collapsed;
					break;
				case "Wheelie":
					indicator.BagGraphic.Visibility = Visibility.Collapsed;
					indicator.CrateGraphic.Visibility = Visibility.Collapsed;
					indicator.WheelieBinGraphic.Visibility = Visibility.Visible;
					break;
			}
		}

		//ctor
		public BinIndicator()
		{
			// Required to initialize variables
			InitializeComponent();
		}
	}
}