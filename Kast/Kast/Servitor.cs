using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;

using System.Net.Sockets;

namespace Kast
{
	/// <summary>
	/// Servitor is the true server of the program. It 
	/// asynchronously accepts clients on port 4206 and sends
	/// their data to a receiving channel as a string.
	/// </summary>
	public class Servitor 
	{
		/// <summary>
		/// A list of all the input that this server has ever gotten.
		/// </summary>
		private List<string> AllInput {get; set;}

		/// <summary>
		/// The port to listen to (defaults to 4206)
		/// </summary>
		private int Port { get; set; }

		/// <summary>
		/// The address to listen for
		/// </summary>
		private IPAddress Address { get; set; }

		/// <summary>
		/// A boolean set up to determine whether input has
		/// been given since the last time it has been collected.
		/// </summary>
		public bool Changed { get; set; }

		/// <summary>
		/// The last time output was collected
		/// </summary>
		private int LastIndex { get; set; }

		/// <summary>
		/// Set up the servitor with the default settings
		/// </summary>
		public Servitor()
		{
			Address = IPAddress.Loopback;
			AllInput = new List<string>();
			LastIndex = 0;
			Changed = false;
			Port = 4206;
		}

		/// <summary>
		/// Create a new servitor given an IPAdress and a port.
		/// </summary>
		/// <param name="address">Address.</param>
		/// <param name="port">Port.</param>
		public Servitor(IPAddress address, int port){
			Address = address;
			AllInput = new List<string> ();
			LastIndex = 0;
			Port = port;
		}
		/// <summary>
		/// Gets all the input that the servitor has received since the last
		/// time it got any.
		/// </summary>
		/// <returns>The list of the new input</returns>
		public List<string> Collect()
		{
			lock (AllInput)
			{
				// It has not changed, because we just looked.
				Changed = false;

				// The last index is now. We just accessed it.
				LastIndex = AllInput.Count - 1;

				return General.Misc.Subsequence(AllInput, LastIndex, AllInput.Count);
			}
		}

		/// <summary>
		/// Start the Servitor, listening on its thread.
		/// </summary>
		public void Start()
		{
			TcpListener listener = new TcpListener(Address, Port);
			listener.Start();

			// Continuously accept clients
			for(;/*ever*/;){
				TcpClient client = listener.AcceptTcpClient();

				NetworkStream nwStream = client.GetStream();
				byte[] buffer = new byte[client.ReceiveBufferSize];

				int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);

				string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);

				lock (AllInput)
				{
					AllInput.Add(dataReceived);
				}

				Changed = true;
			}
		}
	}
}
