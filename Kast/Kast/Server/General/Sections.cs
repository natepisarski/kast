﻿using System;
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

			// If the escape character \ appears, the next character is added to the selection
			bool escaped = false;

			// The final collection
			var collection = new List<string> ();

			foreach(char c in text.ToCharArray()){

				if (escaped) {
					escaped = false;
					selection += c;
					continue;
				}
					
				if (c.Equals ('\\')) {
					escaped = true;
					continue;
				}

				if (collecting && c.Equals (delim)) { // Stop collecting this selection
					collection.Add (selection);
					selection = "";
					collecting = false;
					continue;
				} else if (!collecting && c.Equals (delim)) { // Start collecting this selection
					collecting = true;
					continue;
				} else if (c.Equals (' ') && !collecting) { // Word's done? Add it
					collection.Add (selection);
					selection = "";
				} else
					selection += c;
			}
			collection.RemoveAll (x => x.Equals (""));

			return collection.ToArray ();
		}

		/// <summary>
		/// Split text, listening to the escape character.
		/// </summary>
		/// <returns>The split.</returns>
		/// <param name="text">Text.</param>
		/// <param name="splitOn">Split on.</param>
		public static List<string> EscapeSplit(string text, char splitOn){
			// The total collection
			List<string> collection = new List<string> ();

			// The current selection
			string selection = "";

			// Whether or not the escape character preceded this
			bool escaped = false;

			foreach (char c in text) {
				if (escaped) {
					escaped = false;
					selection += c;
					continue;
				}

				if (c.Equals ('\\')) {
					escaped = true;
					continue;
				}

				if (c.Equals (splitOn)) {
					collection.Add (selection);
					selection = "";
					continue;
				}

				selection += c;
			}

			return collection;
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
