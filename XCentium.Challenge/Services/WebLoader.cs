using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace XCentium.Challenge.Services
{
	internal class WebLoader
	{

		public HtmlDocument LoadHtmlDocument(string url)
		{
			var web = new HtmlWeb
			{
				UsingCache = false //this cache can be unpredictable, leave disabled
			};
			return web.Load(url);
		}


		public HtmlNodeCollection PluckImageNodes(HtmlDocument basis)
		{
			return basis.DocumentNode.SelectNodes("//img");
		}


		public IEnumerable<HtmlNode> GetDocumentText(HtmlDocument basis)
		{
			return basis.DocumentNode.Descendants().Where(node => (node.NodeType.Equals(HtmlNodeType.Text)) &&
																(node.ParentNode.Name != "script") &&
																(node.ParentNode.Name != "style"));

		}


	}
}