using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

using Kast.General;
namespace Kast.Client
{
	public class Program
	{
		public Program ()
		{

		}

		/// <summary>
		/// Sends data to a specified IPAdress and port. 
		/// </summary>
		/// <param name="data">Data.</param>
		/// <param name="adress">Adress.</param>
		/// <param name="port">Port.</param>
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
		/// Sends data to the default IP and port
		/// </summary>
		/// <param name="message">Message.</param>
		public static void SendData(string message){
			SendData (message, IPAddress.Loopback, 4206);
		}


		public static void main(string[] args){
			SendData (Sections.RepairString (args));
		}
}
}
