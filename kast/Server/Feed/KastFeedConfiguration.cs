using System;
using System.Collections.Generic;
using System.Collections;

using Kast.General;

namespace Kast.Feed
{
	/// <summary>
	/// The configuration information for a Feed.
	/// Contains information such as the name and the current Option
	/// </summary>
	public class KastFeedConfiguration : KastConfiguration
	{
		public KastFeedOption Option;

		/// <summary>
		/// Create a new KastFeedConfiguration with a 
		/// </summary>
		/// <param name="assets">Assets.</param>
		/// <param name="option">Option.</param>
		public KastFeedConfiguration(Dictionary<string, string> assets, KastFeedOption option){
			Assets = assets;
			Option = option;
		}

		/// <summary>
		/// Creates a new KastFeedConfiguration
		/// </summary>
		/// <param name="assetString">Asset string.</param>
		/// <param name="option">Option.</param>
		public KastFeedConfiguration(string assetString, KastFeedOption option){
			Assets = BuildAssets (assetString);
			Option = option;
		}


	}
}

