using System;
using Kast;

using System.Collections.Generic;

namespace Kast
{
	/// <summary>
	/// KastFeeds control two KastBoxes, feeding the output from one into the other.
	/// Using KastFeedOption, you can control how the arguments are fed into the 
	/// destination box.
	/// </summary>
	public class KastFeed
	{
		/// <summary>
		/// The source box. These arguments will be fed into
		/// Destination
		/// </summary>
		/// <value>The source.</value>
		public KastBox Source { get; set; }

		/// <summary>
		/// The destination. Source's output is supplied here.
		/// </summary>
		/// <value>The destination.</value>
		public KastBox Destination{ get; set; } 

		/// <summary>
		/// Determines the behavior of this KastFeed.
		/// </summary>
		/// <value>The option.</value>
		public KastFeedOption Option { get; set; }

		/// <summary>
		/// Creates a new feed. 
		/// </summary>
		/// <param name="source">Source.</param>
		/// <param name="destination">Destination.</param>
		/// <param name="option">The type of Feed this will assign</param>
		public KastFeed (KastBox source, KastBox destination, KastFeedOption option)
		{
			Source = source;
			Destination = destination;
			Option = option;
		}

		/// <summary>
		/// Feed this instance. The behavior
		/// of this method will vary depending on the value of 
		/// the KastFeedOption
		/// </summary>
		public void Feed(){
			Source.ProcessBuffer ();

			// Set the command line arguments appropriately
			switch(Option){
			case KastFeedOption.All:
				Destination.ProcessArguments = Source.Buffer;
				break;

			case KastFeedOption.Last:
				Destination.ProcessArguments = new List<string> ();
				Destination.ProcessArguments.Add (Source.Buffer.FindLast (x => true));
				break;
			}

			Destination.ProcessBuffer ();
		}

		/// <summary>
		/// Make the Source the Destination, and vice-versa
		/// </summary>
		public void Flip(){
			KastBox temp = Source;

			Source = Destination;
			Destination = temp;
		}

		/// <summary>
		/// Gets the destination output.
		/// </summary>
		public List<string> GetDestinationOutput(){
			return Destination.Buffer;
		}
	}
}

