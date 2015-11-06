using System;

using System.Collections.Generic;

namespace Kast.Server.General
{
	/// <summary>
	/// A KastComponent is the basic building block of the Kast system.
	/// The relay, and compound Kast types (Feeds, Hooks) work with KastComponents,
	/// which define a minimal set of functionality for Kast objects.
	/// </summary>
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

