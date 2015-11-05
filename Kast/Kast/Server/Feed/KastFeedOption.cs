using System;

namespace Kast.Feed
{
	/// <summary>
	/// Represents options for Kast feeds. The source can either
	/// give the destination all of its output, the last piece of
	/// output.
	/// </summary>
	public enum KastFeedOption
	{
		All,
		Last
	}
}

