using System;
using Kast.General;

using System.Collections.Generic;

namespace Kast.Feed
{
	/// <summary>
	/// KastFeeds control two KastBoxes, feeding the output from one into the other.
	/// Using KastFeedOption, you can control how the arguments are fed into the 
	/// destination box.
	/// </summary>
	public class KastFeed : IKastComponent
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
		/// Name the Box so that it can be accessed from the Relay.
		/// </summary>
		/// <value>The name.</value>
		public string Name {get; set;}

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
		/// Create a new named KastFeed.
		/// </summary>
		/// <param name="source">Source.</param>
		/// <param name="destination">Destination.</param>
		/// <param name="option">Option.</param>
		/// <param name="name">Name.</param>
		public KastFeed(KastBox source, KastBox destination, KastFeedOption option, string name){
			Source = source;
			Destination = destination;
			Option = option;
			this.Name = name;
		}

		/// <summary>
		/// Create a new KastFeed from a KastConfiguration
		/// </summary>
		/// <param name="source">Source.</param>
		/// <param name="destination">Destination.</param>
		/// <param name="config">Config.</param>
		public KastFeed(KastBox source, KastBox destination, KastConfiguration config){
			Source = source;
			Destination = destination;
			try {
				Option = BuildKastFeedOption(config.Assets["option"]);
				Name = config.Assets["name"];
			}catch(Exception e){
				Defaults ();
			}
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

		public void PulseReact(){
			Feed ();
		}

		public string Latest(){
			return Destination.Latest ();
		}

		public void Defaults(){
			Name = "";
			Option = KastFeedOption.Last;
		}
		/// <summary>
		/// Builds a KastFeedOption from a string. Expects it to be formatted in
		/// such a way: "option {All|Last}"
		/// </summary>
		/// <param name="buildString">Build string.</param>
		public static KastFeedOption BuildKastFeedOption(string buildString){
			return buildString.ToLower ().Contains ("all") ? KastFeedOption.All : KastFeedOption.Last;
		}
	}
}

