using System;

namespace Kast
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Welcome to Kast, a management service for \n" +
			"loosely coupled services. Kast is in development and cannot work in its current form. \n" +
			"This binary exists simply to test compilation of the project and to inform prospective users" +
			"that it will not work in its current state. \n" +
			"since I don't know what to do with you, now that you're here, I'll just start the server.");

			Program run = new Program ();
		}
	}
}
