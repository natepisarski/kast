using System;

namespace Kast.Server.General
{
	/// <summary>
	/// KastFuture is a class that names a box that
	/// should at some point enter the relay. The relay
	/// will then fill in the future with a KastBox.
	/// </summary>
	public class KastFuture : IKastComponent
	{
		/// <summary>
		/// The name that this KastFuture is waiting for
		/// </summary>
		public readonly string Name;

		/// <summary>
		/// Creates a new KastFuture, which the relay will promise to fill in
		/// when a KastBox with the name specified by waitingFor appears.
		/// </summary>
		public KastFuture (string waitingFor)
		{
			Name = waitingFor;
		}

		/**The following is to satiate IKastComponent*/
		public void PulseReact(){}
		public void Defaults(){}

		public string Latest(){
			return "";
		}

		public string GetName(){
			return "";
		}
	}
}

