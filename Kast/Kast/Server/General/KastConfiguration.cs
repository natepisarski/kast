using System;

using System.Collections.Generic;

namespace Kast.Server.General
{
	public class KastConfiguration
	{
		public Dictionary<string, string> Assets;

		/// <summary>
		/// Initialize a blank KastConfiguration instance
		/// </summary>
		public KastConfiguration ()
		{
			Assets = new Dictionary<string, string> ();
		}

		/// <summary>
		/// Create a new Configuration with an existing list of Assets and an Option
		/// </summary>
		/// <param name="assets">The configuration dictionary</param>
		public KastConfiguration(Dictionary<string, string> assets){
			Assets = assets;
		}

		/// <summary>
		/// Generate a list of assets from two lists of keys and values.
		/// </summary>
		/// <returns>The two arrays bound as a dictionary.</returns>
		/// <param name="keys">The keys for the dictionary</param>
		/// <param name="values">The values for the dictionary</param>
		public static Dictionary<string, string> BuildAssets(string[] keys, string[] values){
			var collection = new Dictionary<string, string> ();

			for (int index = 0; index < keys.Length && index < values.Length; index++)
				collection.Add (keys [index], values [index]);

			return collection;
		}

		/// <summary>
		/// Build the list of assets from an array of strings.
		/// This is similar to what BuildAssets(string) does.
		/// </summary>
		/// <returns>The array unbound as a dictionary</returns>
		/// <param name="builderList">A list of strings which contain assets</param>
		public static Dictionary<string, string> BuildAssets(string[] builderList){
			Tuple<string[], string[]> unboundList = Misc.Unbind (builderList);

			return BuildAssets (unboundList.Item1, unboundList.Item2);
		}

		/// <summary>
		/// Build the assets from space-delimited strings, in the syntax of 
		/// "key1 value1 key2 value2 ..."
		/// </summary>
		/// <returns>The assets as a dictionary.</returns>
		/// <param name="builderLine">The string that contains the assets</param>
		public static Dictionary<string, string> BuildAssets(string builderLine){
			if (builderLine.Equals ("null"))
				return new Dictionary<string, string> ();

			return BuildAssets (builderLine.Split (' '));
		}

	}
}

