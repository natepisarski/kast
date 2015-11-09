using System;
using System.Collections.Generic;

using Kast.Server.General;
using Kast.Server.Feed;
using Kast.Server.Base;
using Kast.Server.Hook;

namespace Kast.Server.Base
{

	/// <summary>
	/// Kast Builder is a static Factory for building
	/// KastComponents from strings. These strings are expected
	/// to be in a certain format, that KastBuilder.Verify(string, Class)
	/// will ensure. Strings directly from the command line should be able to
	/// be put into the KastBuilder to have it build the component.
	/// </summary>
	public class KastBuilder
	{
		/// <summary>
		/// The master configuration
		/// </summary>
	    public KastConfiguration MasterConfig;

		/// <summary>
		/// The log to write to
		/// </summary>
	    public Logger Log;

		/// <summary>
		/// Initializes a KastBuilder with a configuration and a log
		/// </summary>
		/// <param name="masterConfig">The master configuration</param>
		/// <param name="log">Log.</param>
		public KastBuilder(KastConfiguration masterConfig, Logger log){
			MasterConfig = masterConfig;
			Log = log;
		}

		/// <summary>
		/// Simply return a future with the name after the @
		/// Expected format for "future" is @futureName
		/// </summary>
		/// <returns>A future with the given name</returns>
		/// <param name="future">The name to give this future</param>
		public static KastFuture BuildFuture(string future){
			var futureName = new string (Misc.Tail (future.ToCharArray ()).ToArray ());
			return new KastFuture (futureName);
		}


		/// <summary>
		/// Verify that the string is in the proper format to create a Box.
		/// The expectedformat for a Box is "box |named {name}| process args"
		/// </summary>
		/// <param name="toVerify">The string to verify</param>
		public bool VerifyBox(string[] toVerify){
			return toVerify[0].Equals (MasterConfig.Assets["command_box"]) && toVerify.Length >= 2;
		}

		/// <summary>
		///  Build the box itself
		/// </summary>
		/// <param name="source">Source lines given by the command line.</param>
		public IKastComponent BuildBox(string[] source) {
			source = Sections.ParseSections (Sections.RepairString (source), '+');

			if(!VerifyBox(source))
				throw new Exception(MasterConfig.Assets["message_misshapen_box"]);

			// program arg1 arg2...
			string[] programString = source [1].Split (' ');
			var config = new KastConfiguration (KastConfiguration.BuildAssets (source [2]));
			return new KastBox(programString[0], config, MasterConfig, Log); 

		}



		/// <summary>
		/// Checks that the current string is formatted in a way which can build
		/// a Hook. The expected format is "feed named x "source arguments" "destination arguments"
		/// </summary>
		/// <param name="toVerify">The string to check</param>
		public bool VerifyFeed(string[] toVerify){
			return toVerify [0].Equals (MasterConfig.Assets["command_feed"]) && toVerify.Length >= 3;
		}

		/// <summary>
		/// Create a new Feed given a list of arguments
		/// </summary>
		/// <param name="source">Source lines given by the command line.</param>
		public IKastComponent BuildFeed(string[] source){

			// Get the sections in the format [feed, source, destination, assets]
			source = Sections.ParseSections (Sections.RepairString(source), '|');

			if (!VerifyFeed (source))
				throw new Exception (MasterConfig.Assets["message_misshapen_feed"]);

			var configuration = new KastConfiguration (KastConfiguration.BuildAssets(source [3]));

			IKastComponent sourceComp;
			IKastComponent destinationComp;

			sourceComp =  (source [1] [0].Equals ('@')) ?
				BuildFuture (source [1]) : 
				BuildBox (source [1].Split (' '));

			destinationComp = (source [2] [0].Equals ('@')) ?
				BuildFuture (source [2]) :
				BuildBox (source [2].Split (' '));


			return new KastFeed (sourceComp, destinationComp, configuration);
		}



		/// <summary>
		/// Expects output in the form of "hook named name target "program args""
		/// </summary>
		/// <param name="source">Source lines given by the command line.</param>
		public bool VerifyHook(string[] source){
			return source [0].Equals (MasterConfig.Assets["command_hook"]) && source.Length >= 3;
		}

		/// <summary>
		/// Build the Hook itself
		/// </summary>
		/// <param name="source">Source lines given by the command line.</param>
		public IKastComponent BuildHook(string[] source){
			source = Sections.ParseSections (Sections.RepairString (source), '|');

			if (!VerifyHook (source))
				throw new Exception (MasterConfig.Assets["message_misshapen_hook"]);

			var configuration = new KastConfiguration (KastConfiguration.BuildAssets (source [3]));

			IKastComponent hook;

			// This Hook is a future!
			hook = (source [1] [0].Equals ('@')) ?
				BuildFuture (source [1]) :
				BuildBox (source [1].Split (' '));

			return new KastHook(
				hook,
				source [2], 
				configuration,
				MasterConfig,
				Log
			);
		}


		/// <summary>
		/// Builds a KastComponent, depending on 
		/// the first word supplied. Can be: box, hook, feed.
		/// </summary>
		/// <param name="source">Source lines given by the command line.</param>
		public IKastComponent Build(string[] source){
			Console.WriteLine ("Asked to build: " + Sections.RepairString (source));
			if (source [0].Equals (MasterConfig.Assets["command_box"]))
				return BuildBox (source);
			else if (source [0].Equals (MasterConfig.Assets["command_hook"]))
				return BuildHook (source);
			else if (source [0].Equals (MasterConfig.Assets["command_feed"]))
				return BuildFeed (source);

					throw new Exception (MasterConfig.Assets["message_improper_build"] + source[0]);
		}
	}
}

