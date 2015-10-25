using System;

namespace Kast
{
	/// <summary>
	/// Sets the options for how Hooks should monitor
	/// input to see if it is appropriate to fire.
	/// </summary>
	public enum KastHookOption
	{
		/// <summary>
		/// Match if the name of the process is the first
		/// string in the input.
		/// </summary>
		First,

		/// <summary>
		/// Match if the name of the process is the last
		/// string in the input.
		/// </summary>
		Last,

		/// <summary>
		/// Match if it is found anywhere. Remove its occurence.
		/// </summary>
		InnerRemove,

		/// <summary>
		/// Match if it is found anywhere. Keep its occurence. 
		/// </summary>
		InnerKeep
	}
}

