using System.Collections.Generic;
using System.Text;
using XCentium.Challenge.Utility;

namespace XCentium.Challenge.Extensions
{
	public static class StringExtensions
	{
		/// <summary>
		///     Returns the set of unique words from the given string, where each unique word is delimited by spaces (' ').
		/// </summary>
		public static IEnumerable<string> GetUniqueWords(this string text)
		{
			return StringUtility.GetUniqueWords(text);
		}

		/// <summary>
		///     Ensure that <paramref name="newLineEnding" /> is used in place of each line ending present in the string.
		///     Use when unsure of the type of line ending present in a string ('\r\n' vs '\n' vs '\r').
		/// </summary>
		public static string NormalizeLineEndings(this string text, string newLineEnding)
		{
			if (string.IsNullOrEmpty(text) == false)
			{
				var sb = new StringBuilder(text);
				return sb.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", newLineEnding).ToString();
			}
			return text;
		}
	}
}