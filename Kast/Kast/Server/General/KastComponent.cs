using System;

using System.Collections.Generic;

namespace Kast.Server.General
{
	/// <summary>
	/// A KastComponent is the basic building block of the Kast system.
	/// The relay, and compound Kast types (Feeds, Hooks) work with KastComponents,
	/// which define a minimal set of functionality for Kast objects.
	/// </summary>
	public abstract class KastComponent
	{
		/// <summary>
		/// The name of this isntance
		/// </summary>
		public string Name;

		/// <summary>
		/// The master configuration for this kast instance
		/// </summary>
		protected KastConfiguration MasterConfig;

		/// <summary>
		/// The Kast log
		/// </summary>
		protected Logger Log;

		/// <summary>
		/// Whether or not this component is marked for pruning
		/// </summary>
		public bool Marked;

		/// <summary>
		/// Reacts to a Pulse supplied by the relay.
		/// </summary>
		public virtual void PulseReact(){}

		/// <summary>
		/// Sets the default Assets for a Component.
		/// </summary>
		public virtual void Defaults(){
			Name = "";
			Log = new Logger(MasterConfig.Get ("server_log"));
		}

		/// <summary>
		/// Get the last element of the buffer of importance.
		/// </summary>
		public virtual string Latest(){
			return "";
		}
	}
}

