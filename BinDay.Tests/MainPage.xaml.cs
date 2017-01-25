﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Silverlight.Testing;

namespace BinDay.Tests
{
	public partial class MainPage : PhoneApplicationPage
	{
		// Constructor
		public MainPage()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, RoutedEventArgs e)
		{
			var basicTestsPage = UnitTestSystem.CreateTestPage();
			IMobileTestPage imobilePage = basicTestsPage as IMobileTestPage;
			BackKeyPress += (s, arg) =>
			                	{
			                		bool navigateBackSuccessfull = imobilePage.NavigateBack();
			                		arg.Cancel = navigateBackSuccessfull;
			                	};
			(Application.Current.RootVisual as PhoneApplicationFrame).Content = basicTestsPage;
		}
	}
}