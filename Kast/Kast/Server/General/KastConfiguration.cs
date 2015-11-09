using System;
using System.IO;

using System.Collections.Generic;

namespace Kast.Server.General
{
	public class KastConfiguration
	{
		public Dictionary<string, string> Assets;

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

			// Message Blocks
			assets.Add ("message_server_start", "Server now listening on port");
			assets.Add ("message_added", "Component added to the relay");

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

			// Client commands
			assets.Add ("command_remove", "unlist");

			// Core components
			assets.Add ("command_box", "box");
			assets.Add ("command_feed", "feed");
			assets.Add ("command_hook", "hook");

			// Settings
			assets.Add ("settings_tick_delay", "1");
			assets.Add ("settings_port", "4206");

			// Defaults
			assets.Add ("client_address", "127.0.0.1");
			assets.Add ("client_port", "4206");

			assets.Add ("server_port", "4206");
			assets.Add ("server_address", "127.0.0.1");
			assets.Add ("server_log", "/tmp/kast_server.log");

			return assets;
		}
	}
}

