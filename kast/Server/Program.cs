using System;
using System.Net.Sockets;
using System.Collections.Generic;

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
			KastRelay kr = new KastRelay ();

			Console.WriteLine ("Add one of every kind of thing to the Relay with a string GO!");

			kr.AddComponent ("hook |box ls +arguments -R,-a name named+| myHook |name named|");
			kr.AddComponent ("feed |box ls +arguments -R,-a name named2+| |box less +arguments -a,;+| |option last|");


			Console.WriteLine ("Eventually, this will be a server. For now, let's put stuff in the relay manually.");

			List<string> currentInput = new List<String> ();
			for (; /*ever*/;) {

				Console.WriteLine ("Next string or flush:");
				var input = Console.ReadLine ();
				if (input.Equals ("flush")) {
					kr.AddComponent (currentInput.ToArray ());
					currentInput = new List<string> ();
				} else
					currentInput.Add (input);
			}

		}
	}
}

