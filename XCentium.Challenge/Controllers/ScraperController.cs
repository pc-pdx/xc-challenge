using System;
using System.Web.Mvc;
using XCentium.Challenge.Services;

namespace XCentium.Challenge.Controllers
{
	public class ScraperController : Controller
	{
		private readonly IScraperService _scraper;

		public ScraperController(IScraperService scraperService)
		{
			_scraper = scraperService;
		}


		// GET: scraper
		public ActionResult Index()
		{
			return View();
		}


		public ActionResult Results(string url)
		{
			Uri targetUri;
			if ((string.IsNullOrWhiteSpace(url)) || (false == Uri.TryCreate(url, UriKind.Absolute, out targetUri)))
				return View("NoResults", (object)url);



			var viewModel = _scraper.BuildScrapedPageViewModel(targetUri);
			if (null == viewModel)
				return View("Error", (object)url);


			return View("Results", viewModel);
		}


	}
}