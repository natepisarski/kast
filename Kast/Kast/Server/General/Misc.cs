using System;
using System.Collections;
using System.Collections.Generic;

using System.IO;
namespace Kast.Server.General
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
		/// <param name="list">The list to operate on</param>
		/// <param name="start">The starting INDEX</param>
		/// <param name="length">How many elements to colect for.</param>
		/// <typeparam name="T">What type of list this is.</typeparam>
		public static List<T> Subsequence<T>(List<T> list, int start, int length){
			var collection = new List<T>();

			for (int i = start; i < list.Count && length > 0; i++)
				collection.Add (list [i]);

			return collection;
		}
			

		/// <summary>
		/// Subsequence implemented for arrays
		/// </summary>
		/// <param name="list">The list to operate on</param>
		/// <param name="start">The starting INDEX</param>
		/// <param name="length">How many elements to colect for.</param>
		/// <typeparam name="T">What type of list this is.</typeparam>
		public static List<T> Subsequence<T>(T[] list, int start, int length){
			return Misc.Subsequence (new List<T> (list), start, length);
		}

		/// <summary>
		/// Return the full array minus the first element
		/// </summary>
		/// <param name="list">The list to operate on</param>
		/// <typeparam name="T">The type of list to work on</typeparam>
		public static List<T> Tail<T>(T[] list){
			return Subsequence (list, 1, list.Length);
		}
			
		/// <summary>
		/// Test to see if anything in this list satisfies the predicate. This is a 
		/// built-in function of .NET's Enumerable; however, Mono does not support it yet.
		/// </summary>
		/// <param name="list">The list to test</param>
		/// <param name="predicate">The predicate to move over the list.</param>
		/// <typeparam name="T">The type of list that is being used.</typeparam>
		public static bool Any<T>(List<T> list, Predicate<T> predicate){
			return list.FindAll (predicate).Count > 0;
		}

		/// <summary>
		/// Any implemented for arrays
		/// </summary>
		/// <param name="list">The list to test</param>
		/// <param name="predicate">The predicate to move over the list.</param>
		/// <typeparam name="T">The type of list that is being used.</typeparam>
		public static bool Any<T>(T[] list, Predicate<T> predicate){
			return Misc.Any (new List<T> (list), predicate);
		}

		/// <summary>
		/// Create two arrays. The first array is made up of the 
		/// array's even elements (0, 2, ...) and the second is made up
		/// of the list's odd-indexed elements (1, 3, ...)
		/// </summary>
		/// <param name="originalList">The list to unbind from</param>
		/// <typeparam name="T">The type of this list.</typeparam>
		public static Tuple<T[], T[]> Unbind<T>(T[] originalList){
			var firstList = new List<T> ();
			var secondList = new List<T> ();

			// If this is true, add the item to the first list. Second otherwise.
			bool firstOrSecond = true;

			foreach (T item in originalList) {
				if (firstOrSecond)
					firstList.Add (item);
				else
					secondList.Add (item);

				firstOrSecond = !firstOrSecond;
			}

			return new Tuple<T[], T[]>(firstList.ToArray(), secondList.ToArray());
		}

		/// <summary>
		/// Flatten the specified list.
		/// </summary>
		/// <param name="list">The list to flatten</param>
		/// <typeparam name="T">The type of this list</typeparam>
		public static string Flatten(string[] list){//TODO: WORK WITH IENUMERABLE
			string coll = "";

			foreach (string item in list)
				coll += item;

			return coll;
		}
	}
}

