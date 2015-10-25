using System;
using System.Collections.Generic;

using Kast.General;
using Kast.Feed;
using Kast.Base;

namespace Kast.Base
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

		/// <summary>
		/// Creates a KastBox from the String.
		/// Formatted: box program {assets}
		/// Accepted assets: args "comma,separated,args", name "name"
		/// </summary>
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
			///  Build the box itself
			/// </summary>
			/// <param name="source">Source.</param>
			public static IKastComponent Build(string[] source) {
				source = Sections.ParseSections (Sections.RepairString (source), '+');

				if(!Verify(source))
					throw new Exception("Mishapen Box");

				// program arg1 arg2...
				string[] programString = source [1].Split (' ');
				KastConfiguration config = new KastConfiguration (KastConfiguration.BuildAssets (source [2]));
				return new KastBox(programString[0], config); 

			}

			/// <summary>
			/// Build a box from raw components
			/// </summary>
			/// <returns>The build.</returns>
			/// <param name="source">Source.</param>
			public static IKastComponent RawBuild(string[] source){
				List<string> argList = new List<string>(source);
				argList.Insert (0, "box");
				return Build (argList.ToArray ());
			}
		}

		/// <summary>
		/// Builds a Feed from a String.
		/// Formatted: feed |"source assets"| |"destination assets"| |assets|
		/// Accepted assets: name option
		/// Accepted options: First, All
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
			public static IKastComponent Build(string[] source){
				source = Sections.ParseSections (Sections.RepairString(source), '|');
				if (!Verify (source))
					throw new Exception ("Mishapen Feed");

				var configuration = new KastConfiguration (KastConfiguration.BuildAssets(source [3]));

				return new KastFeed (
					(Box.Build (source [1].Split(' ')) as KastBox),
					(Box.Build (source [2].Split(' ')) as KastBox),
					configuration);
			}
		}

		/// <summary>
		/// Build a new hook from a string. The
		/// string's expected format is:
		/// hook |program args| target assets
		/// Accepted assets: option, name
		/// Accepted options: First, Last, InnerRemove, InnerKeep
		/// </summary>
		public static class Hook  {
			/// <summary>
			/// Expects output in the form of "hook named name target "program args""
			/// </summary>
			/// <param name="source">Source.</param>
			public static bool Verify(string[] source){
				return source [0].Equals ("hook") && source.Length >= 3;
			}

			/// <summary>
			/// Build the Hook itself
			/// </summary>
			/// <param name="source">Source.</param>
			public static IKastComponent Build(string[] source){
				source = Sections.ParseSections (Sections.RepairString (source), '|');

				if (!Verify (source))
					throw new Exception ("Mishapen Hook");

				var configuration = new KastConfiguration (KastConfiguration.BuildAssets (source [3]));
				return new KastHook (
					(Box.Build(source [1].Split (' ')) as KastBox),
					source [2], configuration);
			}
		}

		/// <summary>
		/// Builds a KastComponent, depending on 
		/// the first word supplied. Can be: box, hook, feed.
		/// </summary>
		/// <param name="source">Source.</param>
		public static IKastComponent Build(string[] source){
			if (source [0].Equals ("box"))
				return Box.Build (source);
			else if (source [0].Equals ("hook"))
				return Hook.Build (source);
			else if (source [0].Equals ("feed"))
				return Feed.Build (source);

			throw new Exception ("What are you building?");
		}
	}
}

