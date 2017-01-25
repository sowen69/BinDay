using System;
using System.Collections.ObjectModel;

namespace BinDay.Model
{
	public class NextCollection
	{
		public DateTime CollectionDate { get; set; }
		public ObservableCollection<Bin> BinsInCollection { get; set; }
	}
}