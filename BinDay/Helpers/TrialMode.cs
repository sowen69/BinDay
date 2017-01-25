namespace BinDay.Helpers
{
	public class TrialMode
	{
		public static bool IsTrial
		{ get; private set; }

		public static void DetermineIsTrail()
		{
			// for debugging
			IsTrial = true;
			
			//for production
			//var license = new Microsoft.Phone.Marketplace.LicenseInformation();
			//IsTrial = license.IsTrial();
		}
	}
}
