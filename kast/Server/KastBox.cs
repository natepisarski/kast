﻿using System;

using System.Diagnostics;
using System.Collections.Generic;

namespace Kast
{
	/// <summary>
	/// Kast Boxes wrap a process and arguments. The box can then
	/// "process their buffer", which will run the command with the given
	/// arguments and put the result in the Buffer.
	/// </summary>
	public class KastBox
	{
		/// <summary>
		/// Gets or sets the name of the process.
		/// </summary>
		/// <value>The name of the process.</value>
		public string ProcessName{ get; set; }

		/// <summary>
		/// Gets or sets the process arguments.
		/// </summary>
		/// <value>The process arguments.</value>
		public List<string> ProcessArguments { get; set; }

		/// <summary>
		/// Gets this Box's buffer
		/// </summary>
		/// <value>The buffer.</value>
		public List<string> Buffer { 
			get;
			set;
		}

		/// <summary>
		/// Creates a new KastBox, with the name as the process's name
		/// </summary>
		/// <param name="name">Name</param> The name of the process that this Box will manage
		public KastBox (string name)
		{
			ProcessName = name;
			ProcessArguments = new List<string> ();
			this.Buffer = new List<string> ();
		}
			
		/// <summary>
		/// Get the arguments for this box as a String
		/// </summary>
		/// <returns>The arguments.</returns>
		public string GetArguments(){
			string collection = "";

			foreach (string word in ProcessArguments)
				collection += (word + " ");

			return collection;
		}

		/// <summary>
		/// Process the buffer, putting the result into the Buffer field.
		/// </summary>
		public void ProcessBuffer(){
			ProcessStartInfo info = new ProcessStartInfo (ProcessName, GetArguments ());

			info.RedirectStandardOutput = true;
			info.UseShellExecute = false;

			Process runningProcess = Process.Start (info);

			try {
				runningProcess.WaitForExit();
				string processOutput = runningProcess.StandardOutput.ReadToEnd();
				Buffer.Add(processOutput);
			} catch(InvalidOperationException e) {
				//Console.WriteLine(ProcessName + " did not output properly. Ignoring output.");
				//Console.WriteLine (e.StackTrace);
			}
		}
	}
}

