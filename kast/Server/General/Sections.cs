using System;
using System.Collections.Generic;

namespace Kast.General
{
	/// <summary>
	/// Sections is the class that handles grouping for the command line 
	/// arguments. If you feed it arguments in Sections (delimited by two ||),
	/// it will give you a list of strings where those sections are parsed entirely
	/// as one string.
	/// </summary>
	public static class Sections
	{
		/// <summary>
		/// Parses a section.
		/// Example: { these are words. |this is a string|. |this has a \| in it |
		/// </summary>
		/// <returns>The sections.</returns>
		/// <param name="text">Text.</param>
		/// <param name="delim">Delim. </param>
		public static string[] ParseSections(string text, char delim){

			// What is being collected right now
			string selection = "";

			// Test to see if we need to find another | to flush selection
			bool collecting = false;

			// The final collection
			var collection = new List<string> ();

			foreach(char c in text.ToCharArray()){
				if (collecting && c.Equals (delim)) {
					collection.Add (selection);
					selection = "";
					collecting = false;
					continue;
				} else if (!collecting && c.Equals (delim)) {
					collecting = true;
					continue;
				} else if (c.Equals (' ') && !collecting) {
					collection.Add (selection);
					selection = "";
				} else
					selection += c;
			}
			collection.RemoveAll (x => x.Equals (""));

			return collection.ToArray ();
		}

		/// <summary>
		/// Turn an array of strings back into the original string
		/// </summary>
		/// <param name="toRepair">To repair.</param>
		public static string RepairString(string[] toRepair){
			string collection = "";

			foreach(string item in toRepair){
				collection += item;
				collection += " ";
			}

			return collection;
		}
	}
}

