using System;

using System.Collections.Generic;
using System.Threading;

using Kast.Server.Base;
using Kast.Server.Feed;
using Kast.Server.General;
using Kast.Server;

namespace Kast
{
	public class Program
	{
		private Servitor MainServitor {get; set;}
		private KastRelay Relay { get; set; }
		private int TickDelay { get; set; }

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
			Thread servitorThread = new Thread(new ThreadStart(MainServitor.Start));
			servitorThread.Start();

			Console.WriteLine ("The server is now listening");
			for (;/*ever*/; )
			{
				Thread.Sleep(TickDelay);

				if (MainServitor.Changed)
					MainServitor.Collect ().ForEach (x => Relay.AddComponent (x.Split (' ')));

				Relay.Pulse ();
			}
		}
	}
}

