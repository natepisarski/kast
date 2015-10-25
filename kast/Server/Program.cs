using System;
using System.Net.Sockets;

namespace Kast
{
	public class Program
	{
		/// <summary>
		/// Create a new Server, listening on the default port
		/// to listen on is 4206
		/// </summary>
		public Program ()
		{
			Console.WriteLine ();
			Console.WriteLine ("Welcome to the Server. Let's test some stuff out!");

			Console.WriteLine ("Making a new KastBox for ls");
			var kb = new KastBox ("ls");

			Console.WriteLine ("k. That went well. Let's process it and see what it had to say.");
			kb.ProcessBuffer ();
			Console.WriteLine ("---");
			kb.Buffer.ForEach (x => {Console.WriteLine (x);});
			Console.WriteLine ("---");

			Console.WriteLine ("Still running, eh? Okay. Slick. Let's set up a feed between ls and ls -R to get every subdirectory");
			var kb2 = new KastBox ("ls");
			kb2.ProcessArguments.Add ("-R");
			var kf = new KastFeed (kb, kb2, KastFeedOption.Last);

			Console.WriteLine ("Good. Feed it, maybe?");
			kf.Feed ();

			Console.WriteLine ("---");
			kf.Destination.Buffer.ForEach (x => {Console.WriteLine (x);});
			Console.WriteLine ("---");

			Console.WriteLine ("Did that appear to work? Well golly gee. We're almost all tested.");
			Console.WriteLine ("I just wanna set a hook up now.");
			var kh = new KastHook (kb, "Server", KastHookOption.First);

			Console.WriteLine ("And see how it reacts to two strings");
			kh.React ("Server /home");
			Console.WriteLine ("K. Now the second one");
			kh.React ("notServer /home");

			Console.WriteLine ("Did all that go well? Sweet. Now just write the relay and we're about good to go :)");

			Console.WriteLine ("The server is not currently finished. It will now \n" +
				"just accept your connection and not do anything with it. ^C to kill.");
			TcpListener tcpL = new TcpListener (System.Net.IPAddress.Loopback, 4206);
			tcpL.Start ();

			for (; /*ever*/;)
				tcpL.AcceptTcpClientAsync ();
		}
	}
}

