using System;

using System.Collections.Generic;
using System.Threading;
using System.Net.Sockets;
using System.Net;

using Kast.Server.Base;
using Kast.Server;
using Kast.Server.General;

using HumDrum.Operations;

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

		Logger Log {get; set;}

		KastConfiguration MasterConfig {get; set;}

		int TickDelay { get; set; }

		/// <summary>
		/// Create a new Server, listening to the
		/// default port of 4206.
		/// <param name="master">The master configuration to use</param>
		/// </summary>
		public Program (KastConfiguration master)
		{
			MainServitor = new Servitor();
			Relay = new KastRelay(master, new Logger(master.Get("log")));
			TickDelay = 1000;
		}

		/// <summary>
		/// Initialize the Program given an IP Adress and a port.
		/// </summary>
		/// <param name="address">The address to listen to</param>
		/// <param name="port">The port to listen to</param>
		public Program(KastConfiguration master, Logger logger){
			MainServitor = new Servitor (IPAddress.Parse(master.Get("server_address")),
				int.Parse(master.Get("server_port")));
			Log = logger;
			MasterConfig = master;
			Relay = new KastRelay (master, logger);

			// Tick delay is read in seconds
			TickDelay = (int) double.Parse(master.Get("settings_tick_delay"))*(1000);
		}

		/// <summary>
		/// Starts the server.
		/// </summary>
		public void Start()
		{
			var servitorThread = new Thread(new ThreadStart(MainServitor.Start));
			servitorThread.Start();

					Log.Log (MasterConfig.Get("message_server_start"));
			for (;/*ever*/; )
			{
				Thread.Sleep(TickDelay);

				// Add all the new input from the client to the relay
				if (MainServitor.Changed) {
					var commands = MainServitor.Collect ();
					foreach (string command in commands) {
						var commandWords = command.Split (' ');

						if (commandWords [0].Equals (MasterConfig.Get("command_remove")))
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

