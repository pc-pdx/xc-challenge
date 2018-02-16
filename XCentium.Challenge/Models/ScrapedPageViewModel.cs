using System;
using System.Collections.Generic;

namespace XCentium.Challenge.Models
{
	public class ScrapedPageViewModel
	{

		public IEnumerable<DisplayImage> ImageList { get; set; } //= new List<DisplayImage>();

		public IEnumerable<KeyValuePair<string, int>> WordFrequencyStats { get; set; } //= new List<KeyValuePair<string, int>>();

		public Uri TargetUri { get; set; }

		public int TotalWordCount { get; set; }
		public int UniqueWordCount { get; set; }

	}
}