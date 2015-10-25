using System;
using System.Collections.Generic;

using Kast.General;

namespace Kast.Hook
{
	public class KastHookConfiguration : KastConfiguration
	{
		public KastHookOption Option;

		/// <summary>
		/// Create a new KastHookConfiguration with a 
		/// </summary>
		/// <param name="assets">Assets.</param>
		/// <param name="option">Option.</param>
		public KastHookConfiguration(Dictionary<string, string> assets, KastHookOption option){
			Assets = assets;
			Option = option;
		}

		/// <summary>
		/// Creates a new KastHookConfiguration
		/// </summary>
		/// <param name="assetString">Asset string.</param>
		/// <param name="option">Option.</param>
		public KastHookConfiguration(string assetString, KastHookOption option){
			Assets = BuildAssets (assetString);
			Option = option;
		}
	}
}