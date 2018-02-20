using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using XCentium.Challenge.Extensions;
using XCentium.Challenge.Models;
using XCentium.Challenge.Utility;

namespace XCentium.Challenge.Services
{
	public class ScraperService : IScraperService
	{

		public ScrapedPageViewModel BuildScrapedPageViewModel(Uri targetUri)
		{

			var viewModel = new ScrapedPageViewModel { TargetUri = targetUri };

			var scraper = new WebLoader();
			HtmlDocument htmlDoc;

			try
			{
				htmlDoc = scraper.LoadHtmlDocument(targetUri.ToString());
			}
			catch (Exception ex)
			{
				//very rare scenario where we actually just want to swallow this - no logging or further action necessary in this scenario
				//target site won't be scraped. Send user to scrape-specific error page suggesting to try another site. 
				return null;
			}


			IEnumerable<string> filteredNodes = scraper.GetDocumentText(htmlDoc).Select(x => x.InnerText);

			string normalizedPageString = string.Join(" ", filteredNodes)
				.Replace("\t", "") // ensure we don't include formatting as an word
				.NormalizeLineEndings("");


			List<string> wordList = StringUtility.GetWords(normalizedPageString.ToLowerInvariant()).ToList();
			var wordFrequencyStats = StringUtility.AggregateWordFrequencyStats(wordList.Where(w => w.Length > 2)).ToList();


			viewModel.TotalWordCount = wordList.Count;
			viewModel.UniqueWordCount = StringUtility.GetUniqueWords(wordList).Count();
			viewModel.WordFrequencyStats = AssembleTagCloudListForViewModel(wordFrequencyStats);
			viewModel.ImageList = AssembleImageListForViewModel(targetUri, scraper.PluckImageNodes(htmlDoc));

			return viewModel;
		}








		/// <summary>
		/// Builds TagCloud list with Prevelance weighting
		/// </summary>
		/// <param name="wordFrequencyStats"></param>
		/// <returns></returns>
		private List<TagCloudItem> AssembleTagCloudListForViewModel(IEnumerable<KeyValuePair<string, int>> wordFrequencyStats)
		{
			//get most prevelent word to set our 100% baseline 
			//--all other words will have a calculated size relative from this baseline
			double maxPrevelance = wordFrequencyStats.Max(words => (double)words.Value);


			return wordFrequencyStats.Select(wordStat => new TagCloudItem
			{
				Word = wordStat.Key,
				Count = wordStat.Value,
				Prevelance = (((double)wordStat.Value / maxPrevelance) * 100)
			}).ToList();
		}

		/// <summary>
		///  Ensure <paramref name="imageNodes"/> have valid src attributes, add FQDN if necessary
		/// </summary>
		/// <param name="targetUrl">FQDN where images originate from</param>
		/// <param name="imageNodes">Node collection to iterate</param>
		/// <returns></returns>
		private static IEnumerable<DisplayImage> AssembleImageListForViewModel(Uri targetUrl, HtmlNodeCollection imageNodes)
		{
			var imageList = new List<DisplayImage>();


			if ((null != imageNodes) && (imageNodes.Any()))
				foreach (var item in imageNodes)
				{
					if (null == item.Attributes["src"]) //this scenario can present if custom, client-side lazy-loaders use random data-attributes +/ have no src defined
						continue; //no need to go on


					var imgSrc = item.Attributes["src"].Value;

					//ensure all image src's are fully-qualified to their respective domain, 
					//otherwise they won't render because we're on a totally different domain
					if (Uri.IsWellFormedUriString(imgSrc, UriKind.Relative))
						imgSrc = targetUrl.GetLeftPart(UriPartial.Authority) + imgSrc;


					var displayImage = new DisplayImage { ImageUrl = imgSrc, AltText = item.Attributes["alt"]?.Value ?? string.Empty };
					imageList.Add(displayImage);

				}
			return imageList;
		}

	}
}