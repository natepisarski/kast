using System;

using System.Collections.Generic;

namespace Kast.General
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
		/// <param name="assets">Assets.</param>
		public KastConfiguration(Dictionary<string, string> assets){
			Assets = assets;
		}

		/// <summary>
		/// Generate a list of assets from two lists of keys and values.
		/// </summary>
		/// <returns>The assets.</returns>
		/// <param name="keys">Keys.</param>
		/// <param name="values">Values.</param>
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
		/// <returns>The assets.</returns>
		/// <param name="builderList">Builder list.</param>
		public static Dictionary<string, string> BuildAssets(string[] builderList){
			Tuple<string[], string[]> unboundList = Misc.Unbind (builderList);

			return BuildAssets (unboundList.Item1, unboundList.Item2);
		}

		/// <summary>
		/// Build the assets from space-delimited strings, in the syntax of 
		/// "key1 value1 key2 value2 ..."
		/// </summary>
		/// <returns>The assets.</returns>
		/// <param name="builderLine">Builder line.</param>
		public static Dictionary<string, string> BuildAssets(string builderLine){
			return BuildAssets (builderLine.Split (' '));
		}

	}
}

