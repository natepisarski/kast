using System;
using System.Collections;
using System.Collections.Generic;

namespace Kast.General
{
	/// <summary>
	/// General functions. These are functions that are used throughout the Kast
	/// project but do not belong in any of the other files.
	/// </summary>
	public static class Misc
	{
		/// <summary>
		/// Create a Subsequences of the given list. Collection
		/// starts at the starting index (inclusive) and will collect length
		/// elements
		/// </summary>
		/// <param name="list">List.</param>
		/// <param name="start">Start.</param>
		/// <param name="length">Length.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static List<T> Subsequence<T>(List<T> list, int start, int length){
			var collection = new List<T>();

			for (int i = start; i < list.Count && length > 0; i++)
				collection.Add (list [i]);

			return collection;
		}

		//TODO: Implement for IEnumerable<T: Sized>

		/// <summary>
		/// Subsequence implemented for arrays
		/// </summary>
		/// <param name="list">List.</param>
		/// <param name="start">Start.</param>
		/// <param name="length">Length.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static List<T> Subsequence<T>(T[] list, int start, int length){
			return Misc.Subsequence (new List<T> (list), start, length);
		}

		/// <summary>
		/// Test to see if anything in this list satisfies the predicate. This is a 
		/// built-in function of .NET's Enumerable; however, Mono does not support it yet.
		/// </summary>
		/// <param name="list">List.</param>
		/// <param name="predicate">Predicate.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static bool Any<T>(List<T> list, Predicate<T> predicate){
			return list.FindAll (predicate).Count > 0;
		}

		/// <summary>
		/// Any implemented for arrays
		/// </summary>
		/// <param name="list">List.</param>
		/// <param name="predicate">Predicate.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static bool Any<T>(T[] list, Predicate<T> predicate){
			return Misc.Any (new List<T> (list), predicate);
		}
	}
}

