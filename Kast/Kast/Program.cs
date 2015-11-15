using System;
using System.Threading;
using System.Net;

using Kast.Server.General;

namespace Kast
{
	/*Master TODO list (in no particular order)*/

	//TODO: Add list capabilities to client (list running names)
	//TODO: Handle all thrown exceptions
	//TODO: Fix the ton of port commands in the configuration file
	//TODO: Replace all Logger parameters with Logger constructor
	//TODO: Implement command builder interface for client
	//TODO: Make the configuration file more forgiving (missing key? Use default value)
	//TODO: Add "ObjectExists" exception for name clashes
	//TODO: Change Builder around to use inheritence of a Builder
	//TODO: Make an automatic generator for the default assets

	/* Very future TODOS */
	//TODO: Add scripting support (read arguments one-by-one as kast-client commands)
	//TODO: Add GUI support for client
	//TODO: Generate HTML documentation

	/// <summary>
	/// The entry point for the Kast program.
	/// </summary>
	class MainClass
	{
		/// <summary>
		/// Get help for a specific part of kast.
		/// </summary>
		/// <param name="part">The part of the program to get help for. Can be server or client.</param>
		public static string Help(string part){
			string helpMsg = "";

			if(part.Equals("server")){
				return ("The command \"kast server\" will start the server");
			}
			if (part.Equals ("client")) {
				helpMsg += "[] denotes a required argument. {} denotes optional ones. OR denotes choices\n";
				helpMsg += "Client commands templates are:\n";

				helpMsg += "kast client box [command] +{args comma,separated,args} {name boxName}\n";
				helpMsg += "kast client feed |[box syntax]| |[box syntax]| |{name feedName} {option all OR last}\n";
				helpMsg += "kast client hook |[box syntax]| target |{name hookName} {option first OR last OR innerRemove OR innerKeep}\n";
				helpMsg += "kast client unlist name\n";
				helpMsg += "In places where [box syntax] is accepted, @name is accepted to reference a currently running box in the relay";

			} else return Help();

			return helpMsg;
		}

		/// <summary>
		/// General help. This happens if no input is given on the command line.
		/// </summary>
		public static string Help(){
			string helpMsg = "";

			helpMsg += "Welcome to kast help.\n";
			helpMsg += "For more general help, type \"kast help server\" or \"kast help client\"\n";

			return helpMsg;
		}

		/// <summary>
		/// Either creates a server in this thread, or sends data to a running server.
		/// </summary>
		/// <param name="args">The command-line arguments.</param>
		public static void Main (string[] args)
		{
			//*
			Console.WriteLine ();

			// Help the user if they need it
			if (args.Length == 0 || args [0].Equals ("help")){
				if (args.Length > 1)
					Console.WriteLine (Help (args [1]));
				else
					Console.WriteLine (Help ());
				return;
			}

			// If it's not asking for help, server, or client, just exit.
			if (!(args [0].Equals ("server") || args [0].Equals ("client"))) {
				Console.WriteLine ("Unrecognized command. Exiting kast. Use \"kast help\" for help.");
				Environment.Exit (0);
			}

			var masterConfig = new KastConfiguration ();

			// Is there a command line flag specifying the configuration?
			if (args.Length > 1 && args [1].Equals ("file"))
				masterConfig.Load (args [2]);
			else
				masterConfig = new KastConfiguration(KastConfiguration.DefaultConfiguration ());

			var logger = new Logger (masterConfig.Get("server_log"));

			if(args[0].Equals("server")){
				var server = new Server.Program (masterConfig, logger);
				server.Start ();
			} else {
				// Send everything but "client" and "file" if it exists
				var client = new Client.Program (masterConfig, logger);
				client.main(Misc.Subsequence (args, (args[1].Equals("file")?3:1), args.Length).ToArray());
			}
			//*/
		}
		

	/// <summary>
	/// Until the project is mature, I will be using this to test the program
	/// </summary>
	public static void Test(){
			//*
			KastConfiguration master = new KastConfiguration ();
			master = new KastConfiguration(KastConfiguration.DefaultConfiguration ());
		
			Logger log = new Logger (master.Get("server_log"));

			Kast.Server.Program server = new Kast.Server.Program (master, log);
			Kast.Client.Program client = new Kast.Client.Program (master, log);

			// The server is running now
			new Thread (new ThreadStart (server.Start)).Start ();

			client.SendData ("box gshfdkgj sdgfjk");

			Console.WriteLine ("Successful test");
			//*/
		}
	}
}