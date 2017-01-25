using Caliburn.Micro;
using Microsoft.Phone.Tasks;

namespace BinDay.ViewModels
{
	public class BuyNowViewModel : Screen
	{
		readonly INavigationService _navigationService;
		// ctor
		public BuyNowViewModel(INavigationService navigationService)
		{
			_navigationService = navigationService;
		}

		public void GotoBuyNow()
		{
			MarketplaceDetailTask marketplaceDetailTask = new MarketplaceDetailTask();
			marketplaceDetailTask.Show();
		}
	}
}
