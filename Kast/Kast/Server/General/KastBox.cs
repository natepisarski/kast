using System;

using System.Diagnostics;
using System.Collections.Generic;

using HumDrum.Operations;
using HumDrum.Collections;

namespace Kast.Server.General
{
	/// <summary>
	/// Kast Boxes wrap a process and arguments. The box can then
	/// "process their buffer", which will run the command with the given
	/// arguments and put the result in the Buffer.
	/// </summary>
	public class KastBox : KastComponent
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
		/// <value>The output of this KastBox</value>
		public List<string> Buffer { 
			get;
			set;
		}

		/// <summary>
		/// Creates a new KastBox, with the name as the process's name
		/// </summary>
		/// <param name="name">The name of the process that this Box will manage</param>
		public KastBox (string name, KastConfiguration masterConfig)
		{
			ProcessName = name;
			MasterConfig = masterConfig;
			ProcessArguments = new List<string> ();
			Defaults ();

		}

		/// <summary>
		/// Build the Box using a process name and configuration
		/// </summary>
		/// <param name="procName">The name of the process</param>
		/// <param name="kc">The KastConfiguration for this Box. Accepts: args, name</param>
		public KastBox(string procName, KastConfiguration kc, KastConfiguration masterConfig, Logger log){
			ProcessName = procName;
			MasterConfig = masterConfig;
			Buffer = new List<string> ();
			ProcessArguments = new List<string> ();
			Log = log;
			try{
				if(kc.Has("args"))
					ProcessArguments = Sections.EscapeSplit(kc.Get("args"), ',');

				Name = kc.Get("name");
			}catch(Exception e){
				Defaults ();
			}
		}

		/// <summary>
		/// Get the arguments for this box as a String
		/// </summary>
		/// <returns>The arguments for this box</returns>
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

			try {
				Process runningProcess = Process.Start (info);
				runningProcess.WaitForExit();
				string processOutput = runningProcess.StandardOutput.ReadToEnd();
				Buffer.Add(processOutput);
			} catch(InvalidOperationException e) {
				Log.Log(ProcessName + MasterConfig.Get("message_output_error"));
			} catch(Exception e){
				Log.Log(MasterConfig.Get("message_ambiguous_process_error") + this.ProcessName);
				Marked = true;
			}
		}

		/// <summary>
		/// Process the buffer
		/// </summary>
		public override void PulseReact(){
			ProcessBuffer ();
		}

		/// <summary>
		/// Return the latest output from this process
		/// </summary>
		public override string Latest(){
			return Buffer [Buffer.Count - 1];
		}

		/// <summary>
		/// Set the default information for this KastBox
		/// </summary>
		public override void Defaults(){
			Name = "";
			Buffer = new List<string> ();
			ProcessArguments = ProcessArguments ?? new List<string> ();
		}
	}
}