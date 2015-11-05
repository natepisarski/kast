using System;

using System.Collections.Generic;

namespace Kast.Server.General
{
	public interface IKastComponent
	{
		/// <summary>
		/// Reacts to a Pulse supplied by the relay.
		/// </summary>
		void PulseReact();

		/// <summary>
		/// Sets the default Assets for a Component.
		/// </summary>
		void Defaults();

		/// <summary>
		/// Get the last element of the buffer of importance.
		/// </summary>
		string Latest();

		/// <summary>
		/// Get the name of the current KastComponent
		/// </summary>
		/// <returns>The name.</returns>
		string GetName();
	}
}

