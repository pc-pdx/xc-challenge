using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XCentium.Challenge.Models
{
	public class TagCloudItem
	{
		/// <summary>
		/// Text key
		/// </summary>
		public string Word { get; set; }

		/// <summary>
		/// Number of occurances
		/// </summary>
		public int Count { get; set; }

		/// <summary>
		///  Percentage of this words occurances in camparison to max prevelent word
		/// </summary>
		public double Prevelance { get; set; }


		/// <summary>
		/// Returns Prevelance weighted in scale between 1-10
		/// --items with prevelance under 10% are rounded up to ensure min display size 
		/// </summary>
		public double CalculatedDisplaySize
		{
			get
			{
				double decimator = ((Prevelance/10.0d) < 1.0d)
					? 1.0d //use 1 as Floor
					: (Prevelance/10.0d);


			return	Math.Round(decimator, 2, MidpointRounding.AwayFromZero);
			}
		}
	}
}