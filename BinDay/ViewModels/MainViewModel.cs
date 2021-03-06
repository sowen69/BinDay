using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using BinDay.Helpers;
using BinDay.Model;
using Caliburn.Micro;
using Microsoft.Phone.Tasks;
using SilverlightPhoneDatabase;

namespace BinDay.ViewModels
{
    public class MainViewModel : Screen
    {
    	readonly INavigationService _navigationService;
		private Database _database;

    	private string _versionText = "Version: 1.0.0.0";
    	public string VersionText
    	{
			get { return _versionText; }
    	}


		private NextCollection _nextCollection;
		public NextCollection NextCollection
		{
			get
			{
				var collection = new NextCollection();
				var bins = new BindableCollection<Bin>();

    			var closestDate = _database.Table<Bin>()
    			.Where(b => b.NextCollectionDate.Date >= DateTime.Now.Date)
    			.OrderBy(b => b.NextCollectionDate.Date)
    			.FirstOrDefault();

				if (closestDate == null)
				{
					collection.CollectionDate = new DateTime(1999, 1, 1);
				}
				else
				{
					collection.CollectionDate = closestDate.NextCollectionDate.Date;
					var nowbins = _database.Table<Bin>()
						.Where(b => b.NextCollectionDate.Date == closestDate.NextCollectionDate.Date);

					foreach (var nowbin in nowbins)
					{
						bins.Add(nowbin);
					}

					collection.BinsInCollection = bins;
				}

				_nextCollection = collection;

				return _nextCollection;
			}
		}

		private BindableCollection<Bin> _bins;
		public BindableCollection<Bin> Bins
		{
			get
			{
				var query = (from bin in _database.Table<Bin>()
							 select bin);

				var bins = new BindableCollection<Bin>();
				foreach (var bin in query)
				{
					bins.Add(bin);
				}

				_bins = bins;
				return _bins;
			}
		}

    	private bool _isControlVisible;
		public bool IsControlVisible
		{
			get { return _isControlVisible; }
			set
			{
				_isControlVisible = value;
				NotifyOfPropertyChange(()=>IsControlVisible);
			}

		}

    	private bool _isEmptyDataSetTemplateVisible;
		public bool IsEmptyDataSetTemplateVisible
    	{
    		get { return _isEmptyDataSetTemplateVisible; }
			set
			{
				_isEmptyDataSetTemplateVisible = value;
				NotifyOfPropertyChange(()=>IsEmptyDataSetTemplateVisible);
			}
    	}

		private Bin _selectedBin;
		public Bin SelectedBin
		{
			get { return _selectedBin; }
			set
			{
				_selectedBin = value;
				NotifyOfPropertyChange(() => SelectedBin);
			}

		}

		protected override void OnActivate()
		{
			SetupDb();
			NotifyOfPropertyChange(() => Bins);
			NotifyOfPropertyChange(() => NextCollection);
			IsControlVisible = UpdateControlVisibility();
			IsEmptyDataSetTemplateVisible = UpdateEmptyDataSetVisibility();
			//Debug.WriteLine("OnActivate Called:");
			//Debug.WriteLine("Bins: " + Bins.Count + "_bins: " + _bins.Count + "bins: " + bins.Count + "Query: " + query.Count());
		}
		
		// ctor
    	public MainViewModel(INavigationService navigationService)
    	{
    		_navigationService = navigationService;
			SetupDb();
    	}

		private bool UpdateControlVisibility()
		{
			if (Bins.Count() == 0)
				return false;
			return true;
		}
		private bool UpdateEmptyDataSetVisibility()
		{
			if (Bins.Count() == 0)
				return true;
			return false;
		}
		private void SetupDb()
		{
			if (!Database.DoesDatabaseExists("BinDayDb"))
			{
				_database = Database.CreateDatabase("BinDayDb");
				_database.CreateTable<Bin>();
				_database.CreateTable<NextCollection>();
				_database.Save();
			}
			else
			{
				_database = Database.OpenDatabase("BinDayDb", string.Empty, false);
			}
		}


		//Navigation methods
		public void GotoAddBin()
		{
			//var binCount = (from bin in _database.Table<Bin>()select bin).Count();

			if(TrialMode.IsTrial)
			{
				if (Bins.Count>=2)
				{
					_navigationService.UriFor<BuyNowViewModel>().Navigate();
				}
				_navigationService.UriFor<AddBinViewModel>().Navigate();
			}
			_navigationService.UriFor<AddBinViewModel>().Navigate();
		}

		public void GotoEditBin()
		{
			var bin = SelectedBin;
			if(bin!=null)
			{
				_navigationService.UriFor<AddBinViewModel>()
				   .WithParam(x => x.Id, bin.Id)
				   .Navigate();
			}
		}

		public void GotoReview()
		{
			var task = new MarketplaceReviewTask();
			task.Show();
		}
    	

		//Actions
		public void DeleteBin(Bin bin)
		{
			_database.Table<Bin>().Remove(bin);
			_database.Save();

			Bins.Remove(bin);
			NotifyOfPropertyChange(()=>Bins);
			NotifyOfPropertyChange(()=>NextCollection);
			IsControlVisible = UpdateControlVisibility();
			IsEmptyDataSetTemplateVisible = UpdateEmptyDataSetVisibility();
		}


		/// <summary>
		/// For Testing
		/// TODO: Comment out on Final build 
		/// </summary>
		public void CreateTestBins()
		{
			// US-GB
			_database.Table<Bin>().Add(new Bin { Id = Guid.NewGuid(), Colour = new SolidColorBrush(Color.FromArgb(255, 55, 55, 55)), CollectionFrequency = 7, LastCollectionDate = new DateTime(2011, 7, 8), Type = BinType.Wheelie, BinName = "General Rubbish" });
			_database.Table<Bin>().Add(new Bin { Id = Guid.NewGuid(), Colour = new SolidColorBrush(Colors.Green), CollectionFrequency = 28, LastCollectionDate = new DateTime(2011, 7, 8), Type = BinType.Wheelie, BinName = "Food and Garden waste" });
			_database.Table<Bin>().Add(new Bin { Id = Guid.NewGuid(), Colour = new SolidColorBrush(Colors.Orange), CollectionFrequency = 14, LastCollectionDate = new DateTime(2011, 7, 8), Type = BinType.Wheelie, BinName = "Glass plastic and cans" });
			_database.Table<Bin>().Add(new Bin { Id = Guid.NewGuid(), BinName = "Paper and Cardboard", Type = BinType.Wheelie, Colour = new SolidColorBrush(Colors.Blue), CollectionFrequency = 21, LastCollectionDate = new DateTime(2011, 6, 10) });
			_database.Table<Bin>().Save();

			//German

			//_database.Table<Bin>().Add(new Bin { Id = Guid.NewGuid(), Colour = new SolidColorBrush(Color.FromArgb(255, 55, 55, 55)), CollectionFrequency = 7, LastCollectionDate = new DateTime(2011, 7, 8), Type = BinType.Wheelie, BinName = "General Müll" });
			//_database.Table<Bin>().Add(new Bin { Id = Guid.NewGuid(), Colour = new SolidColorBrush(Colors.Green), CollectionFrequency = 28, LastCollectionDate = new DateTime(2011, 7, 8), Type = BinType.Wheelie, BinName = "Abfälle von Essen und Garten" });
			//_database.Table<Bin>().Add(new Bin { Id = Guid.NewGuid(), Colour = new SolidColorBrush(Colors.Orange), CollectionFrequency = 14, LastCollectionDate = new DateTime(2011, 7, 8), Type = BinType.Wheelie, BinName = "Glas, Plastik und Dosen" });
			//_database.Table<Bin>().Add(new Bin { Id = Guid.NewGuid(), BinName = "Papier und Pappe", Type = BinType.Wheelie, Colour = new SolidColorBrush(Colors.Blue), CollectionFrequency = 21, LastCollectionDate = new DateTime(2011, 6, 10) });
			//_database.Table<Bin>().Save();


			var query = (from bin in _database.Table<Bin>()
						 select bin);
			UpdateNextCollectionDate.Update(query);
			_database.Save();

			NotifyOfPropertyChange(() => Bins);
			NotifyOfPropertyChange(() => NextCollection);
			IsControlVisible = UpdateControlVisibility();
			IsEmptyDataSetTemplateVisible = UpdateEmptyDataSetVisibility();
		}

		//private void GetVersion()
		//{
		//    Uri manifest = new Uri("WMAppManifest.xml", UriKind.Relative);
		//    var si = Application.GetResourceStream(manifest);
		//    if (si != null)
		//    {
		//        using (StreamReader sr = new StreamReader(si.Stream))
		//        {
		//            bool haveApp = false;
		//            while (!sr.EndOfStream)
		//            {
		//                string line = sr.ReadLine();
		//                if (!haveApp)
		//                {
		//                    int i = line.IndexOf("AppPlatformVersion=\"", StringComparison.InvariantCulture);
		//                    if (i >= 0)
		//                    {
		//                        haveApp = true;
		//                        line = line.Substring(i + 20);

		//                        int z = line.IndexOf("\"");
		//                        if (z >= 0)
		//                        {
		//                            // if you're interested in the app plat version at all
		//                            // AppPlatformVersion = line.Substring(0, z);
		//                        }
		//                    }
		//                }

		//                int y = line.IndexOf("Version=\"", StringComparison.InvariantCulture);
		//                if (y >= 0)
		//                {
		//                    int z = line.IndexOf("\"", y + 9, StringComparison.InvariantCulture);
		//                    if (z >= 0)
		//                    {
		//                        // We have the version, no need to read on.
		//                        VersionText = "Version: "+line.Substring(y + 9, z - y - 9);
		//                        break;
		//                    }
		//                }
		//            }
		//        }
		//    }
		//    else
		//    {
		//        VersionText = "Version: Unknown";
		//    }
		//}

		/// <summary>
		/// For Testing
		/// TODO: Comment out on Final build  
		/// </summary>
		//public void DeleteAllBins()
		//{
		//    foreach (var bin in Bins)
		//    {
		//        _database.Table<Bin>().Remove(bin);
		//    }
		//    _database.Table<Bin>().Save();
		//    _database.Save();
		//    NotifyOfPropertyChange(() => Bins);
		//    NotifyOfPropertyChange(() => NextCollection);
		//    IsControlVisible = UpdateControlVisibility();
		//    IsEmptyDataSetTemplateVisible = UpdateEmptyDataSetVisibility();
		//}
    }
}
