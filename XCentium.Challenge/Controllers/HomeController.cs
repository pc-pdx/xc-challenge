using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HtmlAgilityPack;
using XCentium.Challenge.Extensions;
using XCentium.Challenge.Models;
using XCentium.Challenge.Services;
using XCentium.Challenge.Utility;

namespace XCentium.Challenge.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}



		public ActionResult Results(string url)
		{

			if ((string.IsNullOrWhiteSpace(url)) || (false == Uri.IsWellFormedUriString(url, UriKind.Absolute)))
				return View("ScraperForm/NoResults", (object)url);


			Uri targetUri;
			Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out targetUri);



			var viewModel = new ScrapedPageViewModel();
			viewModel.TargetUri = targetUri;

			var scraper = new Scraper();
			HtmlDocument htmlDoc;

			try
			{
				htmlDoc = scraper.RetrieveDocument(url);
			}
			catch (Exception ex)
			{
				return View("ScraperForm/Error", (object)url);
			}


			HtmlNodeCollection imageNodes = scraper.PluckImageNodes(htmlDoc);
			viewModel.ImageList = AssembleImageList(targetUri, imageNodes);




			var filteredDocument = scraper.GetDocumentText(htmlDoc);
			IEnumerable<string> filteredNodes = filteredDocument.Select(x => x.InnerText);

			string normalizedPageString = string.Join(" ", filteredNodes)
				.Replace("\t", "") // ensure we don't include formatting as an word
				.NormalizeLineEndings("");


			List<string> wordList = StringUtility.GetWords(normalizedPageString).ToList();

			viewModel.TotalWordCount = wordList.Count();
			viewModel.WordFrequencyStats = StringUtility.GetWordFrequencyStats(wordList.Where(w => w.Length > 2));
			viewModel.UniqueWordCount = StringUtility.GetUniqueWords(wordList).Count();



			return View("ScraperForm/Results", viewModel);
		}



		private static IEnumerable<DisplayImage> AssembleImageList(Uri targetUrl, HtmlNodeCollection imageNodes)
		{
			var imageList = new List<DisplayImage>();


			if ((null != imageNodes) && (imageNodes.Any()))
			{
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
			}
			return imageList;
		}
	}
}