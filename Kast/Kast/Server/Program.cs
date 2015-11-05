using System;

using System.Collections.Generic;
using System.Threading;

using Kast.Server.Base;
using Kast.Server.Feed;
using Kast.Server.General;
using Kast.Server;

namespace Kast.Server
{
	public class Program
	{
		Servitor MainServitor {get; set;}
		KastRelay Relay { get; set; }
		int TickDelay { get; set; }

		/// <summary>
		/// Create a new Server, listening on the default port
		/// to listen on is 4206
		/// </summary>
		public Program ()
		{
			MainServitor = new Servitor();
			Relay = new KastRelay();
			TickDelay = 1000;
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
					commands.ForEach (x => Relay.AddComponent (x.Split (' ')));
				}

				// Activate the relay
				Relay.Pulse ();
			}
		}
	}
}

