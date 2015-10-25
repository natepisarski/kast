using System;
using System.Collections.Generic;

namespace Kast
{

	/// <summary>
	/// Kast Builder is a static Factory for building
	/// KastComponents from strings. These strings are expected
	/// to be in a certain format, that KastBuilder.Verify(string, Class)
	/// will ensure. Strings directly from the command line should be able to
	/// be put into the KastBuilder to have it build the component.
	/// </summary>
	public static class KastBuilder
	{

		public static class Box {
			/// <summary>
			/// Verify that the string is in the proper format to create a Box.
			/// The expectedformat for a Box is "box |named {name}| process args"
			/// </summary>
			/// <param name="toVerify">To verify.</param>
			public static bool Verify(string[] toVerify){
				return toVerify[0].Equals ("box") && toVerify.Length >= 2;
			}

			/// <summary>
			/// Creates a KastBox from the String
			/// </summary>
			/// <param name="source">Source.</param>
			public static KastComponent Build(string[] source) {
				string boxName = "";

				if(!Verify(source))
					throw new Exception("Mishapen Box");

				if (source[1].Contains ("named")) {
					// The name is everything after named
					boxName = source [2];
					return new KastBox (source [3], boxName);
				} else
					return new KastBox (source [2]);

			}
		}

		/// <summary>
		/// Builds a Hook from a String
		/// </summary>
		public static class Feed {
			/// <summary>
			/// Checks that the current string is formatted in a way which can build
			/// a Hook. The expected format is "feed named x "source arguments" "destination arguments"
			/// </summary>
			/// <param name="toVerify">To verify.</param>
			public static bool Verify(string[] toVerify){
				return toVerify [0].Equals ("feed") && toVerify.Length >= 3;
			}

			/// <summary>
			/// Create a new Feed given a list of arguments
			/// </summary>
			/// <param name="source">Source.</param>
			public static KastComponent Build(string[] source){
				string sourceName = "";
				int numericalOffset = 0;

				if (!Verify (source))
					throw new Exception ("Mishapen Feed");

				if (source [1].Equals ("named")) {

					sourceName = source [2];
					numericalOffset = 2;
				}

				var sourceWords = source [2 + numericalOffset].Split (' ');
				var sourceProgram = sourceWords[0];
				var sourceArgs = General.Subsequence (sourceWords, 1, sourceWords.Length);

				var destWords = source [3 + numericalOffset].Split (' ');
				var destProgram = destWords [0];
				var destArgs = General.Subsequence (destWords, 1, destWords.Length);

				// FIXME: Implement Option building
				return new KastFeed (new KastBox (sourceProgram, sourceArgs, sourceName),
					new KastBox (destProgram, destArgs, sourceName), KastFeedOption.Last, sourceName);
			}
		}

		public static class Hook  {
			/// <summary>
			/// Expects output in the form of "hook named name target "program args""
			/// </summary>
			/// <param name="source">Source.</param>
			public static bool Verify(string[] source){
				return source [0].Equals ("hook") && source.Length >= 3;
			}

			/// <summary>
			/// Build a Hook from a list of arguments
			/// </summary>
			/// <param name="source">Source.</param>
			public static KastComponent Build(string[] source){
				int numericalOffset = 0;
				string sourceName = "";

				if (!Verify (source))
					throw new Exception ("Mishapen Hook");

				if (source [1].Equals ("named")){
					numericalOffset = 2;
					sourceName = source [2];
				}

				//FIXME: Add Options

				string[] sourceWords = source [3 + numericalOffset].Split (' ');
				string sourceProcess = sourceWords [0];
				List<string> sourceArgs = General.Subsequence (sourceWords, 1, sourceWords.Length);
	
				return new KastHook (new KastBox (sourceProcess, sourceArgs, sourceName),
					source [2 + numericalOffset],
					KastHookOption.First);
			}
		}
	}
}

