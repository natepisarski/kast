using System;
using System.Threading;
using Kast.Server.General;

namespace Kast
{
	/*Master TODO list (in no particular order)*/

	//TODO: Allow no assets
	//TODO: Add list capabilities to client (list running names)
	//TODO: Fix all the documentation
	//TODO: Handle all thrown exceptions
	//TODO: Implement command builder interface for client
	//TODO: Add logging to Server with variable log file
	//TODO: Add unlist for named items
	//TODO: Move Server stuff to the Kast.Server namespace
	//TODO: Add KastConfiguration to the server (logfile location, port)
	//TODO: Implement basic logger module
	//TODO: Add port configuration to the client
	//TODO: Add config file support for server and client (consider server block and client block)

	/* Very future TODOS */
	//TODO: Add scripting support (read arguments one-by-one as kast-client commands)
	//TODO: Add GUI support for client

	/// <summary>
	/// The entry point for the Kast program.
	/// </summary>
	class MainClass
	{
		/// <summary>
		/// Get help for a specific part of kast.
		/// </summary>
		/// <param name="part">Part.</param>
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

				helpMsg += "In places where [box syntax] is accepted, @name is accepted to reference a currently running box in the relay";
			} else return Help();

			return helpMsg;
		}

		/// <summary>
		/// General help
		/// </summary>
		public static string Help(){
			string helpMsg = "";
			helpMsg += "Welcome to kast help.\n";
			helpMsg += "For more general help, type \"kast help server\" or \"kast help client\"\n";

			return helpMsg;
		}

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


			if (!(args [0].Equals ("server") || args [0].Equals ("client"))) {
				Console.WriteLine ("Unrecognized command. Exiting kast. Use \"kast help\" for help.");
			} else {
				if(args[0].Equals("server")){
					Kast.Server.Program server = new Kast.Server.Program();
					server.Start ();
				} else {
					// Send everything but "client"
					Client.Program.main (Misc.Subsequence (args, 1, args.Length).ToArray());
				}
			}
        //*/
		}

		/// <summary>
		/// Until the project is mature, I will be using this to test the program
		/// </summary>
		public static void test(){
			Kast.Server.Program server = new Kast.Server.Program ();

			// The server is running now
			new Thread (new ThreadStart (server.Start)).Start ();

			Client.Program.SendData ("box echo +args hello,msg name hello+");
			Client.Program.SendData ("box notify-send +name writer+");
			Client.Program.SendData ("hook |box notify-send +args \"hi\"+| |hello| |name myName|");

			Console.WriteLine ("Successful test");
		}
	}
}
