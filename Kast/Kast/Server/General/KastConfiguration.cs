using System;
using System.IO;

using System.Collections.Generic;

namespace Kast.Server.General
{
	public class KastConfiguration
	{
		Dictionary<string, string> Assets;

		/// <summary>
		/// Initialize a blank KastConfiguration instance
		/// </summary>
		public KastConfiguration ()
		{
			Assets = new Dictionary<string, string> ();
		}

		/// <summary>
		/// Create a new Configuration with an existing list of Assets and an Option
		/// </summary>
		/// <param name="assets">The configuration dictionary</param>
		public KastConfiguration(Dictionary<string, string> assets){
			Assets = assets;
		}

		/// <summary>
		/// Generate a list of assets from two lists of keys and values.
		/// </summary>
		/// <returns>The two arrays bound as a dictionary.</returns>
		/// <param name="keys">The keys for the dictionary</param>
		/// <param name="values">The values for the dictionary</param>
		public static Dictionary<string, string> BuildAssets(string[] keys, string[] values){
			var collection = new Dictionary<string, string> ();

			for (int index = 0; index < keys.Length && index < values.Length; index++)
				collection.Add (keys [index], values [index]);

			return collection;
		}

		/// <summary>
		/// Build the list of assets from an array of strings.
		/// This is similar to what BuildAssets(string) does.
		/// </summary>
		/// <returns>The array unbound as a dictionary</returns>
		/// <param name="builderList">A list of strings which contain assets</param>
		public static Dictionary<string, string> BuildAssets(string[] builderList){
			Tuple<string[], string[]> unboundList = Misc.Unbind (builderList);

			return BuildAssets (unboundList.Item1, unboundList.Item2);
		}

		/// <summary>
		/// Build the assets from space-delimited strings, in the syntax of 
		/// "key1 value1 key2 value2 ..."
		/// </summary>
		/// <returns>The assets as a dictionary.</returns>
		/// <param name="builderLine">The string that contains the assets</param>
		public static Dictionary<string, string> BuildAssets(string builderLine){
			if (builderLine.Equals ("null"))
				return new Dictionary<string, string> ();

			return BuildAssets (builderLine.Split (' '));
		}

		/// <summary>
		/// Load a file of sections into the kast configuration
		/// </summary>
		/// <param name="filename">The path to the file containing the configuration</param>
		public void Load(string filename){
			Assets = new Dictionary<string, string> ();

			foreach (string line in File.ReadAllLines(filename)) {

				// Emptyish line? Proceed
				if (line.Length < 2)
					continue;

				// Header comment
				if (line [0].Equals ('+') && line [1].Equals ('+'))
					continue;

				var keyValue = Sections.ParseSections (line, '|');
				Assets.Add (keyValue [0], keyValue [1]);
			}

		}

		/// <summary>
		/// Return a sane default configuration for the kast program. This is the default that the repository
		/// include in its file and that all documentation examples use.
		/// </summary>
		/// <returns>The default configuration to use</returns>
		public static Dictionary<string, string> DefaultConfiguration(){
			var assets = new Dictionary<string, string> ();

			//BEGIN AUTOMATICALLY GENERATED CODE
			// Message Blocks
			assets.Add ("message_server_start", "Server now listening on port");
			assets.Add ("message_added", "Component added to the relay");
			assets.Add ("message_builder_started", "Command builder started");

			// Diagnostics
			assets.Add ("message_boxes", "Boxes.");
			assets.Add ("message_hooks", "Hooks.");
			assets.Add ("message_feeds", "Feeds.");

			// Exceptions
			assets.Add ("message_misshapen_box", "Misshapen box detected");
			assets.Add ("message_misshapen_feed", "Misshapen feed detected");
			assets.Add ("message_misshapen_hook", "Misshapen hook detected");
			assets.Add ("mesage_improper_build", "Received improper request to build: ");
			assets.Add ("message_output_error", "Failed to execute properly. Ignoring output");
			assets.Add ("message_ambiguous_process_error", "Ambiguous process execution error. Is this command in your $PATH? Command:");
			assets.Add ("message_kasthook_option_unrecognized", "This KastHookOption was specified, but unrecognized: ");

			// Client commands
			assets.Add ("command_remove", "unlist");

			// Core components
			assets.Add ("command_box", "box");
			assets.Add ("command_feed", "feed");
			assets.Add ("command_hook", "hook");

			// Settings
			assets.Add ("settings_tick_delay", "1");
			assets.Add ("settings_port", "4206");

			// Server Defaults
			assets.Add ("client_address", "127.0.0.1");
			assets.Add ("client_port", "4206");

			assets.Add ("server_port", "4206");
			assets.Add ("server_address", "127.0.0.1");
			assets.Add ("server_log", "/tmp/kast_server.log");

			// Internal defaults
			assets.Add ("default_kasthook_option", "first");
			assets.Add ("default_kastfeed_option", "last");

			// Internals
			assets.Add ("name", "Unnamed component");
			assets.Add ("args", "No arguments");

			//END AUTOMATICALLY GENERATED CODE

			return assets;
		}

		/// <summary>
		/// Get the default value for the specified key. If it's not found
		/// in the list of default values. If it's not found, null takes its place.
		/// </summary>
		/// <returns>The default argument</returns>
		/// <param name="key">The key to look up</param>
		public static string RetrieveDefault(string key){
			try{
			return KastConfiguration.DefaultConfiguration () [key];
			} catch(KeyNotFoundException k){
				Console.WriteLine (key + " not found in default values");
				return null;
			}
		}

		/// <summary>
		/// Gets the asset, or supplies the default value
		/// </summary>
		/// <returns>The asset's value/returns>
		/// <param name="asset">The asset to look up</param>
		public string Get(string asset){
			try {
				return this.Assets[asset];
			} catch(KeyNotFoundException k){
				return KastConfiguration.RetrieveDefault (asset);
			}
		}

		/// <summary>
		/// Determines whether this instance has  message in its keys
		/// </summary>
		/// <returns><c>true</c> if this instance has message; otherwise, <c>false</c>.</returns>
		/// <param name="message">The message to search for</param>
		public bool Has(string message){
			return Assets.ContainsKey (message);
		}
	}
}

