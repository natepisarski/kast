using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

using Kast.Server.General;

namespace Kast.Client
{
	public class Program
	{
		KastConfiguration MasterConfig;
		Logger Log;

		/// <summary>
		/// Initializes a new instance of the Program, with the masterConfig
		/// configuration
		/// </summary>
		/// <param name="masterConfig">The configuration to use for the client</param>
		/// <param name="log">The logger to write to</param>
		public Program (KastConfiguration masterConfig, Logger log)
		{
			MasterConfig = masterConfig;
			Log = log;
		}

		/// <summary>
		/// Sends data to a specified IPAdress and port. 
		/// </summary>
		/// <param name="data">The data to send as a string</param>
		/// <param name="adress">The address to send the data to</param>
		/// <param name="port">The port to send the data to</param>
		public static void SendData(string data, IPAddress adress, int port) {
			// Data buffer for incoming data.
			byte[] bytes = new byte[1024];

			// Connect to a remote device.
			try {

				// Create a TCP/IP  socket.
				Socket sender = new Socket(AddressFamily.InterNetwork, 
					SocketType.Stream, ProtocolType.Tcp );

				// Connect the socket to the remote endpoint. Catch any errors.
				try {
					sender.Connect(adress, port);

					// Encode the data string into a byte array.
					byte[] msg = Encoding.ASCII.GetBytes(data);

					// Send the data through the socket.
					int bytesSent = sender.Send(msg);

					// Release the socket.
					sender.Shutdown(SocketShutdown.Both);
					sender.Close();

				} catch (Exception e){
					Console.WriteLine(e.ToString());
				} 
			} catch(Exception e){
				Console.WriteLine (e.ToString ());
			}
	}

		/// <summary>
		/// Sends data to the IP and Port defined in the configuration file
		/// </summary>
		/// <param name="message">The message to send</param>
		public void SendData(string message){
			SendData (message, 
				IPAddress.Parse(MasterConfig.Assets["client_address"]), 
				int.Parse(MasterConfig.Assets["client_port"]));
		}

		/// <summary>
		/// Send these arguments to the server
		/// </summary>
		/// <param name="args">The arguments to send to the server</param>
		public void main(string[] args){
			SendData (Sections.RepairString (args));
		}
}
}
