using System.Diagnostics;
using System.Linq;
using BinDay.Helpers;
using BinDay.Model;
using BinDay.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using Caliburn.Micro;
using SilverlightPhoneDatabase;
using Telerik.Windows.Controls;

namespace BinDay
{
    public class AppBootstrapper : PhoneBootstrapper
    {
        PhoneContainer _container;
		private Database _database;
		protected override void Configure()
        {
			
            _container = new PhoneContainer(RootFrame);
			_container.Singleton<TrialMode>();
			_container.RegisterPhoneServices();
			_container.Singleton<MainViewModel>();
        	_container.PerRequest<AddBinViewModel>();
			_container.PerRequest<BuyNowViewModel>();
			//_container.RegisterInstance(typeof(INavigationService), null, new FrameAdapter(RootFrame));
            AddCustomConventions();
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

		protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
		{
			base.OnStartup(sender, e);
			TrialMode.DetermineIsTrail();
			if (Database.DoesDatabaseExists("BinDayDb"))
			{
				_database = Database.OpenDatabase("BinDayDb", string.Empty, false);
				// Check all Collection Dates are correct
				var query = (from bin in _database.Table<Bin>()
							 select bin);

				UpdateNextCollectionDate.Update(query);
				

				_database.Save();

				Debug.WriteLine("Bin collection dates updated from OnStartup");
			}
		}

		protected override void OnActivate(object sender, Microsoft.Phone.Shell.ActivatedEventArgs e)
		{
			base.OnActivate(sender, e);
			TrialMode.DetermineIsTrail();
			if (Database.DoesDatabaseExists("BinDayDb"))
			{
				_database = Database.OpenDatabase("BinDayDb", string.Empty, false);
				// Check all Collection Dates are correct
				var query = (from bin in _database.Table<Bin>()
							 select bin);

				UpdateNextCollectionDate.Update(query);
				_database.Save();

				Debug.WriteLine("Bin collection dates updated from OnActivate");
			}
		}

        static void AddCustomConventions()
        {
			ConventionManager.AddElementConvention<MenuItem>(ItemsControl.ItemsSourceProperty, "DataContext", "Click");

			ConventionManager.AddElementConvention<Pivot>(Pivot.ItemsSourceProperty, "SelectedItem", "SelectionChanged").ApplyBinding =
				(viewModelType, path, property, element, convention) =>
				{
					if (ConventionManager
						.GetElementConvention(typeof(ItemsControl))
						.ApplyBinding(viewModelType, path, property, element, convention))
					{
						ConventionManager
							.ConfigureSelectedItem(element, Pivot.SelectedItemProperty, viewModelType, path);
						ConventionManager
							.ApplyHeaderTemplate(element, Pivot.HeaderTemplateProperty, viewModelType);
						return true;
					}

					return false;
				};

			ConventionManager.AddElementConvention<Panorama>(Panorama.ItemsSourceProperty, "SelectedItem", "SelectionChanged").ApplyBinding =
				(viewModelType, path, property, element, convention) =>
				{
					if (ConventionManager
						.GetElementConvention(typeof(ItemsControl))
						.ApplyBinding(viewModelType, path, property, element, convention))
					{
						ConventionManager
							.ConfigureSelectedItem(element, Panorama.SelectedItemProperty, viewModelType, path);
						ConventionManager
							.ApplyHeaderTemplate(element, Panorama.HeaderTemplateProperty, viewModelType);
						return true;
					}

					return false;
				};
        	ConventionManager.AddElementConvention<RadNumericUpDown>(RadNumericUpDown.ValueProperty, "Value", "ValueChanged");
			ConventionManager.AddElementConvention<RadDatePicker>(RadDatePicker.ValueProperty, "Value", "ValueChanged");
			//ConventionManager.AddElementConvention<RadListPicker>(RadListPicker.ItemsSourceProperty, "SelectedItem", "SelectionChanged");
			ConventionManager.AddElementConvention<RadListPicker>(RadListPicker.ItemsSourceProperty, "SelectedItem", "SelectionChanged")
				.ApplyBinding = (viewModelType, path, property, element, convention) => 
				{ 
					if (ConventionManager
						.GetElementConvention(typeof(ItemsControl))
						.ApplyBinding(viewModelType, path, property, element, convention)) 
					{ ConventionManager
						.ConfigureSelectedItem(element, RadListPicker.SelectedItemProperty, viewModelType, path);
						return true; 
					} 
					
					return false; 
				};
        }
    }
}