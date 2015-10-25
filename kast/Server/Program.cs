using System;
using System.Net.Sockets;

using Kast.Base;
using Kast.Feed;
using Kast.General;

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

			Console.WriteLine ("Did all that go well? Sweet.");

			Console.WriteLine ();
			Console.WriteLine ();
			Console.WriteLine ();

			Console.WriteLine ("... okay. So, you've almost got yourself a practical server. Wanna make a relay?");
			var kr = new KastRelay ();
			Console.WriteLine ("Yikes. Okay. We have it... Let's add a box, hook, and feed to it.");

			kr.AddComponent (kb);
			kr.AddComponent (kh);
			kr.AddComponent (kf);

			Console.WriteLine ("Wanna... You know. Make it pulse?");
			kr.Pulse ();

			Console.WriteLine ("Are we still here? Woah. Okay. So... one last step. Dear god.");
			Console.WriteLine ("Let's build a box from a string");
			kr.AddComponent(new String[]{"box", "ls", "name myname arguments -R,-a"});

			Console.WriteLine ("Can our new box play nicely with the other boxes?");
			kr.Pulse ();

			Console.WriteLine ("Let's sift through the ashes and see what happened to our poor box");
			Console.WriteLine ("---------");
			((KastBox)kr.GetComponentByName ("myname")).Buffer.ForEach (x => Console.WriteLine (x));
			Console.WriteLine ("----------");

			Console.WriteLine ("If you got here you're a lucky man. Okay! Now let's get that server up and running.");

			Console.WriteLine ("The server is not currently finished. It will now \n" +
				"just accept your connection and not do anything with it. ^C to kill.");
			TcpListener tcpL = new TcpListener (System.Net.IPAddress.Loopback, 4206);
			tcpL.Start ();

			for (; /*ever*/;)
				;//tcpL.AcceptTcpClientAsync ();
		}
	}
}

