using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Media;

namespace BinDay.Model
{
	public static class UpdateNextCollectionDate
	{
		private static DateTime _today;

		static UpdateNextCollectionDate()
		{
			_today = DateTime.Now.Date;
		}

		public static IEnumerable<Bin> Update(IEnumerable<Bin> bins)
		{
			if (bins == null)
			{
				return null;
			}

			foreach (var bin in bins)
			{
				//V1.1 
				// Brown colour changed to be more 'Brown' 
				// So we now need to check if the user has the old Brown
				// and update it. Update Brown if old Brown exisits
				//if (bin.Colour == new SolidColorBrush(Colors.Brown))
				Debug.WriteLine(bin.Colour.Color.ToString());
				if (bin.Colour.Color.ToString() == "#FFA52A2A")
				{
					bin.Colour = new SolidColorBrush(Color.FromArgb(255, 105, 65, 1));
				}


				if(bin.NextCollectionDate.Date < _today)
				{
					while (bin.LastCollectionDate.Date < (_today.AddDays(-bin.CollectionFrequency)))
					{
						if (bin.LastCollectionDate.Date == (_today.AddDays(-bin.CollectionFrequency)))
						{
							break;
						}
						bin.LastCollectionDate = bin.LastCollectionDate.AddDays(bin.CollectionFrequency);
					} 
				}
			}
			return bins;
		}

		/// <summary>
		/// Test method that sets "TODAY" to a know value before updating the collection dates
		/// Enables consistant testing
		/// </summary>
		/// <param name="bins"></param>
		/// <returns></returns>
		//public static IEnumerable<Bin> TestUpdate(IEnumerable<Bin> bins)
		//{
			
		//    _today = new DateTime(2011, 7, 15);
			
		//    Update(bins);
		//    return bins;
		//}
	}
}