using System;

namespace Kast.Server.Feed
{
	/// <summary>
	/// Represents options for Kast feeds. The source can either
	/// give the destination all of its output, the last piece of
	/// output.
	/// </summary>
	public enum KastFeedOption
	{
		/// <summary>
		/// Feed the entire output every time
		/// </summary>
		All,

		/// <summary>
		/// Feed just the last output
		/// </summary>
		Last
	}
}

