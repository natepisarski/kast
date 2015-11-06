using System;

using System.Collections.Generic;
using System.Threading;
using System.Net;

using Kast.Server.Base;
using Kast.Server;

namespace Kast.Server
{
	/// <summary>
	/// The server program creates a servitor which
	/// listens to a specified port. Using output from the
	/// servitor, the relay of all running KastComponents is managed.
	/// </summary>
	public class Program
	{
		Servitor MainServitor {get; set;}
		KastRelay Relay { get; set; }
		int TickDelay { get; set; }

		/// <summary>
		/// Create a new Server, listening to the
		/// default port of 4206.
		/// </summary>
		public Program ()
		{
			MainServitor = new Servitor();
			Relay = new KastRelay();
			TickDelay = 1000;
		}

		/// <summary>
		/// Initialize the Program given an IP Adress and a port.
		/// </summary>
		/// <param name="address">The address to listen to</param>
		/// <param name="port">The port to listen to</param>
		public Program(IPAddress address, int port){
			MainServitor = new Servitor (address, port);
		}

		/// <summary>
		/// Starts the server.
		/// </summary>
		public void Start()
		{
			var servitorThread = new Thread(new ThreadStart(MainServitor.Start));
			servitorThread.Start();

			Console.WriteLine ("The server is now listening");
			for (;/*ever*/; )
			{
				Thread.Sleep(TickDelay);

				// Add all the new input from the client to the relay
				if (MainServitor.Changed) {
					var commands = MainServitor.Collect ();
					foreach (string command in commands) {
						var commandWords = command.Split (' ');

						if (commandWords [0].Equals ("unlist"))
							Relay.RemoveComponent (commandWords [1]);
						else
							Relay.AddComponent (commandWords);
					}
				}

				// Activate the relay
				Relay.Pulse ();
			}
		}
	}
}

