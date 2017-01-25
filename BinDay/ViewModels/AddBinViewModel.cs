using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using BinDay.Model;
using Caliburn.Micro;
using SilverlightPhoneDatabase;

namespace BinDay.ViewModels
{
	public class AddBinViewModel : Screen
	{
		readonly INavigationService _navigationService;
		private Database _database;
		private Bin addEditBin = new Bin();

		private Guid _id;
		private BinType _binType;

		private ColourSwatch _binColour;
		private string _description;
		private int _frequency;
		private DateTime _nextCollection;

		private bool _editMode;
		public bool EditMode
		{
			get { return _editMode; }
			set
			{
				_editMode = value;
				NotifyOfPropertyChange(()=>EditMode);
				NotifyOfPropertyChange(() => PageTitle);
			}
		}


		public string PageTitle
		{
			get
			{
				if (EditMode)
					return Localization.AppResources.EditBin;
				return Localization.AppResources.AddBin;
			}
		}

		

		public Guid Id
		{
			get { return _id; }
			set
			{
				_id = value;
				NotifyOfPropertyChange(()=>Id);
			}
		}
		public BinType BinType
		{
			get { return _binType; }
			set 
			{ 
				_binType = value;
				//Debug.WriteLine("BinType is now: " + value.ToString());
				NotifyOfPropertyChange(() => BinType);
			}
		}
		
		public ColourSwatch BinColour
		{
			get { return _binColour; }
			set
			{
				_binColour = value;
				NotifyOfPropertyChange(() => BinColour);
			}
		}
		
		public string Description
		{
			get { return _description; }
			set
			{
				_description = value; 
				NotifyOfPropertyChange(() => Description);
				NotifyOfPropertyChange(() => CanSave);
			}
		}
		public int Frequency
		{
			get { return _frequency; }
			set
			{
				_frequency = value; 
				NotifyOfPropertyChange(() => Frequency);
			}
		}
		public DateTime NextCollection
		{
			get { return _nextCollection.Date; }
			set
			{
				_nextCollection = value; 
				NotifyOfPropertyChange(() => NextCollection);
			}
		}

		public ObservableCollection<ColourSwatch> Colours { get; set; }
		public ObservableCollection<BinType> BinTypePicker { get; set; }
		
		
		public AddBinViewModel(INavigationService navigationService)
		{
			_navigationService = navigationService;
			CreatePickers();
			SetupDb();
		}


		protected override void OnActivate()
		{
			if(Id==Guid.Empty)
			{
				EditMode = false;
				Id = Guid.NewGuid();
				BinColour = new ColourSwatch{ColourName = "Green", ColourLabel = Localization.AppResources.Green, Brush = new SolidColorBrush(Colors.Green)};
				Frequency = 7;
				BinType = BinType.Wheelie;
				NextCollection = DateTime.Now.Date;
			}
			else
			{
				EditMode = true;
				PopulateControls();
			}
			base.OnActivate();
		}
		
		private void PopulateControls()
		{
			var query = (from b in _database.Table<Bin>()
			    where b.Id == Id
			    select b).FirstOrDefault();

			if (query != null)
			{
				BinType = query.Type;
				BinColour = ColourToColourSwatch(query.Colour);
				Description = query.BinName;
				Frequency = query.CollectionFrequency;
				NextCollection = query.NextCollectionDate;
			}
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
				_database = Database.OpenDatabase("BinDayDb", string.Empty, true);
			}
		}

		private ColourSwatch ColourToColourSwatch(SolidColorBrush brush)
		{
			ColourSwatch swatch = null;
			
			switch (brush.Color.ToString())
			{
				case "":
				case "#FF000000":
					swatch = new ColourSwatch { ColourName = "Black", ColourLabel = Localization.AppResources.Black, Brush = new SolidColorBrush(Colors.Black) };
					break;
				case "#FF808080":
					swatch = new ColourSwatch { ColourName="Gray", ColourLabel= Localization.AppResources.Grey, Brush = new SolidColorBrush(Colors.Gray) };
					break;
				case "#FF0000FF":
					swatch = new ColourSwatch { ColourName="Blue", ColourLabel= Localization.AppResources.Blue, Brush = new SolidColorBrush(Colors.Blue) };
					break;
				case "#FF008000":
					swatch = new ColourSwatch { ColourName="Green", ColourLabel= Localization.AppResources.Green, Brush = new SolidColorBrush(Colors.Green) };
					break;
				case "#FFFF0000":
					swatch = new ColourSwatch { ColourName="Red", ColourLabel= Localization.AppResources.Red, Brush = new SolidColorBrush(Colors.Red) };
					break;
				//case "#FFA52A2A":
				case "#FF694101":
					swatch = new ColourSwatch { ColourName = "Brown", ColourLabel = Localization.AppResources.Brown, Brush = new SolidColorBrush(Color.FromArgb(255, 105, 65, 1)) };
					//swatch = new ColourSwatch { ColourName = "Brown", ColourLabel = Localization.AppResources.Brown, Brush = new SolidColorBrush(Colors.Brown) };
					break;

				case "#FFFFA500":
					swatch = new ColourSwatch { ColourName="Orange", ColourLabel= Localization.AppResources.Orange, Brush = new SolidColorBrush(Colors.Orange) };
					break;
				case "#FF800080":
					swatch = new ColourSwatch { ColourName="Purple", ColourLabel= Localization.AppResources.Purple, Brush = new SolidColorBrush(Colors.Purple) };
					break;
				case "#FFFFFF00":
					swatch = new ColourSwatch { ColourName="Yellow", ColourLabel= Localization.AppResources.Yellow, Brush = new SolidColorBrush(Colors.Yellow) };
					break;
				case "#FFFF00FF":
					swatch = new ColourSwatch { ColourName="Magenta", ColourLabel= Localization.AppResources.Pink, Brush = new SolidColorBrush(Colors.Magenta) };
					break;
			}

			return swatch;
		}

		private void CreatePickers()
		{
			var colourPicker = new ObservableCollection<ColourSwatch>();
			colourPicker.Add(new ColourSwatch { ColourName="Black", ColourLabel= Localization.AppResources.Black, Brush = new SolidColorBrush(Colors.Black) });
			colourPicker.Add(new ColourSwatch { ColourName="Gray", ColourLabel= Localization.AppResources.Grey, Brush = new SolidColorBrush(Colors.Gray) });
			colourPicker.Add(new ColourSwatch { ColourName="Blue", ColourLabel= Localization.AppResources.Blue, Brush = new SolidColorBrush(Colors.Blue) });
			colourPicker.Add(new ColourSwatch { ColourName="Green", ColourLabel= Localization.AppResources.Green, Brush = new SolidColorBrush(Colors.Green) });
			colourPicker.Add(new ColourSwatch { ColourName="Red", ColourLabel= Localization.AppResources.Red, Brush = new SolidColorBrush(Colors.Red) });
			//colourPicker.Add(new ColourSwatch { ColourName = "Brown", ColourLabel = Localization.AppResources.Brown, Brush = new SolidColorBrush(Colors.Brown) });
			colourPicker.Add(new ColourSwatch { ColourName = "Brown", ColourLabel = Localization.AppResources.Brown, Brush = new SolidColorBrush(Color.FromArgb(255, 105, 65, 1)) });
														  				
			colourPicker.Add(new ColourSwatch { ColourName="Orange", ColourLabel= Localization.AppResources.Orange, Brush = new SolidColorBrush(Colors.Orange) });
			colourPicker.Add(new ColourSwatch { ColourName="Purple", ColourLabel= Localization.AppResources.Purple, Brush = new SolidColorBrush(Colors.Purple) });
			colourPicker.Add(new ColourSwatch { ColourName="Yellow", ColourLabel= Localization.AppResources.Yellow, Brush = new SolidColorBrush(Colors.Yellow) });
			colourPicker.Add(new ColourSwatch { ColourName="Magenta", ColourLabel= Localization.AppResources.Pink, Brush = new SolidColorBrush(Colors.Magenta) });
			Colours = colourPicker;
			
			var binTypePicker = new ObservableCollection<BinType>();
			binTypePicker.Add(BinType.Crate);
			binTypePicker.Add(BinType.Bag);
			binTypePicker.Add(BinType.Wheelie);
			BinTypePicker = binTypePicker;
		}

		//Guards
		public bool CanSave
		{
			get {return !string.IsNullOrEmpty(Description);}
		}

		//Actions
		public void Save()
		{
			if (EditMode)
			{

				addEditBin = (from b in _database.Table<Bin>()
				              where b.Id == Id
				              select b).FirstOrDefault();
			}
			if (addEditBin != null)
			{
				addEditBin.Id = Id;
				addEditBin.BinName = Description;
				addEditBin.Colour = BinColour.Brush;
				addEditBin.Type = BinType;
				addEditBin.LastCollectionDate = NextCollection.AddDays(-Frequency).Date;
				addEditBin.CollectionFrequency = Frequency;

				if(EditMode==false)
				{
					_database.Table<Bin>().Add(addEditBin);	
				}
			}

			_database.Save();
			
			_navigationService.GoBack();
			//_navigationService.UriFor<MainViewModel>().Navigate();
		}
		
		public void Delete()
		{
			
		}

	}
}