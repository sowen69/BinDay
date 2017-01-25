using System;
using System.Windows;
using MemoryDiagnostics;

namespace BinDay
{
	public partial class App
	{
		/// <summary>
		/// Constructor for the Application object.
		/// </summary>
		public App()
		{
			// Standard Silverlight initialization
			InitializeComponent();
			//if (System.Diagnostics.Debugger.IsAttached)
			//{
			//// Display the current frame rate counters.
			//Current.Host.Settings.EnableFrameRateCounter = true;

			//// Show memory stats
			//MemoryDiagnosticsHelper.Start(TimeSpan.FromMilliseconds(500), true);

			//// Show the areas of the app that are being redrawn in each frame.
			////Application.Current.Host.Settings.EnableRedrawRegions = true;

			//// Enable non-production analysis visualization mode, 
			//// which shows areas of a page that are being GPU accelerated with a colored overlay.
			////Application.Current.Host.Settings.EnableCacheVisualization = true;
			//}
		}
	}
}