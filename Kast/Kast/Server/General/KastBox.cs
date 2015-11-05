﻿using System;

using System.Diagnostics;
using System.Collections.Generic;

namespace Kast.Server.General
{
	/// <summary>
	/// Kast Boxes wrap a process and arguments. The box can then
	/// "process their buffer", which will run the command with the given
	/// arguments and put the result in the Buffer.
	/// </summary>
	public class KastBox : IKastComponent
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
		/// Name the Box so that it can be accessed from the Relay.
		/// </summary>
		/// <value>The name.</value>
		public string Name {get; set;}

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
			Defaults ();
		}

		/// <summary>
		/// Build the Box using a process name and configuration
		/// </summary>
		/// <param name="procName">The name of the process</param>
		/// <param name="kc">The KastConfiguration for this Box. Accepts: args, name</param>
		public KastBox(string procName, KastConfiguration kc){
			ProcessName = procName;
			Buffer = new List<string> ();
			ProcessArguments = new List<string> ();
			try{
				if(kc.Assets.ContainsKey("args"))
					ProcessArguments = Sections.EscapeSplit(kc.Assets["args"], ',');

				Name = kc.Assets["name"];
			}catch(Exception e){
				Defaults ();
			}
		}

		/// <summary>
		/// Get the arguments for this box as a String
		/// </summary>
		/// <returns>The arguments.</returns>
		public string GetArguments(){
			return Sections.RepairString (ProcessArguments.ToArray());
		}

		/// <summary>
		/// Process the buffer, putting the result into the Buffer field.
		/// </summary>
		public void ProcessBuffer(){
			var info = new ProcessStartInfo (ProcessName, GetArguments ());

			info.RedirectStandardOutput = true;
			info.UseShellExecute = false;

			Process runningProcess = Process.Start (info);

			try {
				runningProcess.WaitForExit();
				string processOutput = runningProcess.StandardOutput.ReadToEnd();
				Buffer.Add(processOutput);
			} catch(InvalidOperationException e) {
				Console.WriteLine(ProcessName + " did not output properly. Ignoring output.");
				Console.WriteLine (e.StackTrace);
			}
		}

		public void PulseReact(){
			ProcessBuffer ();
		}

		public string Latest(){
			return Buffer [Buffer.Count - 1];
		}

		public void Defaults(){
			Name = "";
			Buffer = new List<string> ();
			ProcessArguments = ProcessArguments ?? new List<string> ();
		}

		public string GetName(){
			return Name;
		}
	}
}