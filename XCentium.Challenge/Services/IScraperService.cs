using System;
using XCentium.Challenge.Models;

namespace XCentium.Challenge.Services
{
	public interface IScraperService
	{
		ScrapedPageViewModel BuildScrapedPageViewModel(Uri targetUri);
	}
}