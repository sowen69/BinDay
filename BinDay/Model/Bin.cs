using System;
using System.Windows.Media;
using System.Xml.Serialization;

namespace BinDay.Model
{
	public class Bin
	{
		public Guid Id { get; set; }
		public BinType Type { get; set; }
		[XmlIgnore]
		public SolidColorBrush Colour { get; set; }

		[XmlElement("Colour")]
		public string XmlColour
		{
			get { return SerializeColor(Colour); }
			set {Colour =  DeserializeColor(value); }
		}
		public string BinName { get; set; }

		// AddDays is a Double but I don't want any 'bits' of a day stored so int it is.
		public int CollectionFrequency { get; set; }
		public DateTime LastCollectionDate { get; set; }

		public DateTime NextCollectionDate
		{
			get { return LastCollectionDate.AddDays(CollectionFrequency).Date; }
		}

		public override string ToString()
		{
			return NextCollectionDate.ToShortDateString();
		}

		//public enum ColorFormat
		//{
		//    NamedColor,
		//    ARGBColor
		//}

		public string SerializeColor(SolidColorBrush color)
		{
			return color.Color.A+":"+color.Color.R+":"+ color.Color.G+":"+ color.Color.B;
		}

		public SolidColorBrush DeserializeColor(string color)
		{
			SolidColorBrush brush =new SolidColorBrush();
			byte a, r, g, b;

			string[] pieces = color.Split(new char[] { ':' });

			a = byte.Parse(pieces[0]);
			r = byte.Parse(pieces[1]);
			g = byte.Parse(pieces[2]);
			b = byte.Parse(pieces[3]);

			brush.Color = Color.FromArgb(a, r, g, b);
			return brush;
		}
	}
}
