using System;
using System.Collections.Generic;
using System.Linq;

namespace XCentium.Challenge.Utility
{
	public static class StringUtility
	{
		private static readonly char[] WordSeparators = { ' ' };

		/// <summary>
		///     Returns the set of unique words from the set of all given strings, where each unique word is delimited by spaces (' ').
		/// </summary>
		public static IEnumerable<string> GetUniqueWords(params string[] words)
		{
			return GetUniqueWords((IEnumerable<string>)words);
		}

		/// <summary>
		///     Returns the set of unique words from the set of all given strings, where each unique word is delimited by spaces (' ').
		/// </summary>
		public static IEnumerable<string> GetUniqueWords(IEnumerable<string> words)
		{
			var wordHash = new HashSet<string>();
			foreach (string word in words)
				if (word != null)
					wordHash.UnionWith(word.Split(WordSeparators, StringSplitOptions.RemoveEmptyEntries));

			return wordHash;
		}


		/// <summary>
		///     Returns the set of words from the given string, where each unique word is delimited by spaces (' ').
		/// </summary>
		public static IEnumerable<string> GetWords(string words)
		{
			var wordList = new List<string>();
			wordList.AddRange(words.Split(WordSeparators, StringSplitOptions.RemoveEmptyEntries));
			return wordList;
		}


		/// <summary>
		///     Returns the set of words with corresponding count of occurances(frequency) within the given strings.
		/// </summary>
		public static IEnumerable<KeyValuePair<string, int>> AggregateWordFrequencyStats(IEnumerable<string> basis)
		{
			return (basis.GroupBy(x => x.ToLowerInvariant())
				.Select(grouping => new { grouping, count = grouping.Count() })
				.OrderByDescending(@t => @t.count)
				.Select(@t => new KeyValuePair<string, int>(@t.grouping.Key, @t.count)));
		}

	}
}