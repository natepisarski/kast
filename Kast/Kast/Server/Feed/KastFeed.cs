using System;
using Kast.Server.General;

using System.Collections.Generic;

namespace Kast.Server.Feed
{
	/// <summary>
	/// KastFeeds control two KastBoxes, feeding the output from one into the other.
	/// Using KastFeedOption, you can control how the arguments are fed into the 
	/// destination box.
	/// </summary>
	public class KastFeed : KastComponent
	{
		/// <summary>
		/// The source box. These arguments will be fed into
		/// Destination
		/// </summary>
		/// <value>The box that will be fed into destination</value>
		public KastComponent Source { get; set; }

		/// <summary>
		/// The destination. Source's output is supplied here.
		/// </summary>
		/// <value>The box that will be fed by the source</value>
		public KastComponent Destination{ get; set; } 

		/// <summary>
		/// Determines the behavior of this KastFeed.
		/// </summary>
		/// <value>An Option. Can be All or Last.</value>
		public KastFeedOption Option { get; set; }


		/// <summary>
		/// Creates a new feed from a source component, a destination component, and an
		/// option.
		/// </summary>
		/// <param name="source">The source box</param>
		/// <param name="destination">The destination box</param>
		/// <param name="option">The option to use. All or Last.</param>
		public KastFeed (KastComponent source, KastBox destination, KastFeedOption option)
		{
			Source = source;
			Destination = destination;
			Option = option;

			Name = "";
		}

		/// <summary>
		/// Create a new KastFeed from a source, destination, and configuration
		/// </summary>
		/// <param name="source">The source box</param>
		/// <param name="destination">The destination box</param>
		/// <param name="config">The configuratin</param>
		public KastFeed(KastComponent source, KastComponent destination, KastConfiguration config){
			Source = source;
			Destination = destination;

			try {
				Option = BuildKastFeedOption(config.Get("option"));
				Name = config.Get("name");
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
			KastBox lSource;
			KastBox lDestination;

			// Do nothing if the comonents are futures
			if (Source is KastFuture || Destination is KastFuture)
				return;

			lSource = Source as KastBox;
			lDestination = Destination as KastBox;

				lSource.ProcessBuffer ();

				// Set the command line arguments appropriately
				switch (Option) {
				case KastFeedOption.All:
					lDestination.ProcessArguments = lSource.Buffer;
					break;

				case KastFeedOption.Last:
					lDestination.ProcessArguments = new List<string> ();
					lDestination.ProcessArguments.Add (lSource.Buffer.FindLast (x => true));
					break;
				}

				lDestination.ProcessBuffer ();

		}

		/// <summary>
		/// Make the Source the Destination, and vice-versa
		/// </summary>
		public void Flip(){
			if (Source is KastFuture || Destination is KastFuture)
				return;

			var temp = Source as KastBox;

			Source = Destination;
			Destination = temp;
		}

		/// <summary>
		/// Gets the destination output.
		/// </summary>
		public List<string> GetDestinationOutput(){
			if(!(Destination is KastFuture))
				return (Destination as KastBox).Buffer;

			return null;
		}
			
		/// <summary>
		/// Feed the instances into one another
		/// </summary>
		public override void PulseReact(){
			Feed ();
		}

		/// <summary>
		/// Get the latest information from Destination's buffer
		/// </summary>
		public override string Latest(){
			return Destination.Latest ();
		}

		/// <summary>
		/// Set this instance to the default settings
		/// </summary>
		public override void Defaults(){
			Name = "";
			Option = KastFeedOption.Last;
		}

		/// <summary>
		/// Builds a KastFeedOption from a string. Expects it to be formatted in
		/// such a way: "option {All|Last}"
		/// </summary>
		/// <param name="buildString">A string containing an option</param>
		public static KastFeedOption BuildKastFeedOption(string buildString){
			return buildString.ToLower ().Contains ("all") ? KastFeedOption.All : KastFeedOption.Last;
		}
	}
}

