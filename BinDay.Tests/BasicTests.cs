using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using BinDay.Model;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BinDay;

namespace BinDay.Tests
{
	[TestClass]
	public class BasicTests : SilverlightTest
	{
		private DateTime _today = new DateTime(2011, 7, 15);
		
		[TestMethod]
		public void AlwaysTrue()
		{
			Assert.IsTrue(_today==new DateTime(2011, 7, 15),"Today value is: "+ _today);
		}

		[TestMethod]
		[Description("Create Bin Object")]
		public void CreateBinObject()
		{
			var bin = CreateTestBin();
			
			Assert.IsNotNull(bin);
		}

		[TestMethod]
		[Description("Test that NextCollectionDate returns the correct value")]
		public void NextCollectionDateIsCorrect()
		{
			var bin = CreateTestBin();
			
			Assert.IsTrue(bin.NextCollectionDate == new DateTime(2011,3,15));
			bin.CollectionFrequency = 7;
			Assert.IsTrue(bin.NextCollectionDate ==new DateTime(2011,3,8));

			// check for leap year
			bin.LastCollectionDate = new DateTime(2012, 2, 22);
			Assert.IsTrue(bin.NextCollectionDate  == new DateTime(2012, 2, 29));

			bin.LastCollectionDate=new DateTime(2012,2,27);
			Assert.IsTrue(bin.NextCollectionDate  == new DateTime(2012, 3, 5));
		}

		[TestMethod]
		[Description("Test that ToString returns the correct value")]
		public void ToString_Emits_Correct_Text()
		{
			var bin = CreateTestBin();
			Debug.WriteLine(bin.ToString());

			Assert.IsTrue(bin.ToString() == "15/03/2011");

			bin.Type = BinType.Crate;
			Assert.IsTrue(bin.ToString() == "15/03/2011");
		}


		//[TestMethod]
		//[Description("Update Collection Date Logic Is Correct")]
		//public void NextCollectionDate_Calculates_Correctly()
		//{
		//    var bins = CreateUpdatedTestBins();
			

		//    var bin1 = bins[0].NextCollectionDate;
		//    var bin2 = bins[1].NextCollectionDate;
		//    var bin3 = bins[2].NextCollectionDate;

		//    Assert.IsTrue(bin1 == new DateTime(2011, 7, 8), "Date was: " + bin1.Date);
		//    Assert.IsTrue(bin2 == new DateTime(2011, 7, 15), "Date was: " + bin2.Date);
		//    Assert.IsTrue(bin3 == new DateTime(2011, 7, 20), "Date was: "+bin3.Date);

		//}


		// Test UpdateCollectionDate
		[TestMethod]
		[Description("Update Collection Date Logic Is Correct")]
		public void NCD_In_The_Past()
		{
			var bins = CreateUpdatedTestBins();


			var bin = bins[0].NextCollectionDate;
			Assert.IsTrue(bins[0].BinName == "Test Bin 1");

			/* Test Bin 1 (bins[0]) has a starting NextCollectionDate of 8th July 2011
			 * the update frequency is 7 days so UpdateNextCollectionDate should return
			 * 15th July 2011
			*/
			Assert.IsTrue(bin == new DateTime(2011, 7, 15), "Date was: " + bin);

		}

		[TestMethod]
		[Description("Update Collection Date Logic Is Correct")]
		public void NCD_Is_Today()
		{
			var bins = CreateUpdatedTestBins();


			Assert.IsTrue(bins[1].BinName == "Test Bin 2");
			var bin = bins[1].NextCollectionDate;

			/* Test Bin 2 (bins[1]) has a starting NextCollectionDate of 15th July 2011
			 * the update frequency is 7 days so UpdateNextCollectionDate should return
			 * 15th July 2011
			*/
			Assert.IsTrue(bin == new DateTime(2011, 7, 15), "Date was: " + bin);
		}
		[TestMethod]
		[Description("Update Collection Date Logic Is Correct")]
		public void NCD_In_The_Future()
		{
			var bins = CreateUpdatedTestBins();


			Assert.IsTrue(bins[2].BinName == "Test Bin 3");
			var bin = bins[2].NextCollectionDate;

			/* Test Bin 3 (bins[2]) has a starting NextCollectionDate of 20th July 2011
			 * the update frequency is 7 days so UpdateNextCollectionDate should return
			 * 20th July 2011
			*/
			Assert.IsTrue(bin == new DateTime(2011, 7, 20), "Date was: " + bin);
		}
		[TestMethod]
		[Description("Update Collection Date Logic Is Correct")]
		public void NCD_Is_Long_Time_In_The_Past()
		{
			var bins = CreateUpdatedTestBins();


			Assert.IsTrue(bins[3].BinName == "Test Bin 4");
			var bin = bins[3].NextCollectionDate;

			/* Test Bin 4 (bins[3]) has a starting NextCollectionDate of 3rd of June 2011
			 * the update frequency is 14 days so UpdateNextCollectionDate should return
			 * 15th July 2011
			*/
			Assert.IsTrue(bin == new DateTime(2011, 7, 15), "Date was: " + bin);
		}
		[TestMethod]
		[Description("Update Collection Date Logic Is Correct")]
		public void NCD_Is_Medium_Time_In_The_Past()
		{
			var bins = CreateUpdatedTestBins();


			Assert.IsTrue(bins[4].BinName == "Test Bin 5");
			var bin = bins[4].NextCollectionDate;

			/* Test Bin 5 (bins[4]) has a starting NextCollectionDate of 10th of June 2011
			 * the update frequency is 21 days so UpdateNextCollectionDate should return
			 * 22nd July 2011
			*/
			Assert.IsTrue(bin == new DateTime(2011, 7, 22), "Date was: " + bin);
		}


		private Bin CreateTestBin()
		{
			var bin = new Bin();
			bin.Id = new Guid();
			bin.Type = BinType.Wheelie;
			bin.Colour = new SolidColorBrush(Colors.Magenta);
			bin.BinName = "Test Bin";
			bin.CollectionFrequency = 14;
			bin.LastCollectionDate = new DateTime(2011, 3, 1);
			return bin;
		}

		private IEnumerable<Bin> CreateTestBins()
		{
			var bins = new ObservableCollection<Bin>();
			
			// NextCollectionDate in the past (2011, 7, 8)
			bins.Add(new Bin { Id = Guid.NewGuid(), BinName = "Test Bin 1", Type = BinType.Wheelie, Colour = new SolidColorBrush(Colors.Blue), CollectionFrequency = 7, LastCollectionDate = new DateTime(2011, 7, 1) });
			
			// NextCollectionDate is Today (2011, 7, 15)
			bins.Add(new Bin { Id = Guid.NewGuid(), BinName = "Test Bin 2", Type = BinType.Wheelie, Colour = new SolidColorBrush(Colors.Blue), CollectionFrequency = 7, LastCollectionDate = new DateTime(2011, 7, 8) });
			
			// NextCollectionDate is in the future (2011, 7, 20)
			bins.Add(new Bin { Id = Guid.NewGuid(), BinName = "Test Bin 3", Type = BinType.Wheelie, Colour = new SolidColorBrush(Colors.Blue), CollectionFrequency = 7, LastCollectionDate = new DateTime(2011, 7, 13) });

			// NextCollectionDate is a long time in the (2011, 6, 3)
			bins.Add(new Bin { Id = Guid.NewGuid(), BinName = "Test Bin 4", Type = BinType.Wheelie, Colour = new SolidColorBrush(Colors.Blue), CollectionFrequency = 14, LastCollectionDate = new DateTime(2011, 6, 3) });

			// NextCollectionDate is a medium time in the (2011, 7, 1)
			bins.Add(new Bin { Id = Guid.NewGuid(), BinName = "Test Bin 5", Type = BinType.Wheelie, Colour = new SolidColorBrush(Colors.Blue), CollectionFrequency = 21, LastCollectionDate = new DateTime(2011, 6, 10) });

			return bins;
		}

		private ObservableCollection<Bin> CreateUpdatedTestBins()
		{
			var bins = CreateTestBins();
			UpdateNextCollectionDate.TestUpdate(bins);

			return (ObservableCollection<Bin>) bins;
		}
	}
}