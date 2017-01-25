using BinDay.Helpers;

namespace BinDay.Views
{
	public partial class MainView
	{
		// Constructor
		public MainView()
		{
			InitializeComponent();
			if (TrialMode.IsTrial==true)
				textBlock1.Text = Localization.AppResources.TrialMode;
		}
	}
}