using System;

using System.Collections.Generic;
using Kast.Server.General;

namespace Kast.Server.Hook
{
	/// <summary>
	/// Hooks will capture the input of any command that matches
	/// their Target, and execute, using the rest of the string as an argument.
	/// </summary>
	public class KastHook : IKastComponent
	{
		public IKastComponent Box {get; set; }

		/// <summary>
		/// Gets or sets the target, the value that will cause this
		/// Hook's Box to act.
		/// </summary>
		/// <value>The value which will cause this hook to fire</value>
		public string Target { get; set; }

		/// <summary>
		/// Define how this KastHook should react to input
		/// </summary>
		/// <value>Either First, Last, InnerRemove, or InnerKeep</value>
		public KastHookOption Option { get; set; }

		/// <summary>
		/// Name the Box so that it can be accessed from the Relay.
		/// </summary>
		/// <value>Any string that names this instance.</value>
		public string Name {get; set;}

		/// <summary>
		/// The log to write to
		/// </summary>
		public Logger Log;

		/// <summary>
		/// The master configuration
		/// </summary>
		public KastConfiguration MasterConfig;

		/// <summary>
		/// Makes a KastHook using an existing Box and a string target.
		/// </summary>
		/// <param name="box">Box.</param>
		/// <param name="target">Target.</param>
		/// <param name="option">The option to use</param> 
		public KastHook (KastBox box, string target, KastHookOption option, KastConfiguration masterConfig, Logger log)
		{
			Box = box; 
			Target = target;
			Option = option;

			Name = "";

			MasterConfig = masterConfig;
			Log = log;
		}

		/// <summary>
		/// Build a KastHook using a KastConfiguration
		/// </summary>
		/// <param name="box">The box to capture.</param> 
		/// <param name="target">The string that will make this hook fire</param>
		/// <param name="config">Configuration assets, such as name and option.</param>
		public KastHook(IKastComponent box, string target, KastConfiguration config, KastConfiguration masterConfig, Logger log){
			Box = box;
			Target = target;
			MasterConfig = masterConfig;
			Log = log;
			try {
				Option = KastHook.BuildKastHookOption(config.Get("option"));
				Name = config.Get("name");
			}catch(Exception e){
				Defaults ();
			}
		}

		/// <summary>
		/// React the specified input, according to the KastHookOption
		/// </summary>
		/// <param name="input">The input to react on. Usually other Component's output.</param>
		public bool React(string input){
			if (Box is KastFuture)
				return false;

			var lBox = Box as KastBox;

			var inputWords = new List<string>(input.Split(' '));

			// Clear the old argument list
			lBox.ProcessArguments = new List<string> ();

			switch(Option){

			case KastHookOption.First:
				if (inputWords [0].Equals (Target))
					lBox.ProcessArguments = Misc.Subsequence (inputWords, 1, inputWords.Count);
				break;

			case KastHookOption.InnerKeep:
			case KastHookOption.InnerRemove:
				if (Misc.Any (inputWords, x => x == Target)) {
					if (Option == KastHookOption.InnerKeep)
						lBox.ProcessArguments = inputWords;
					else{
						// removes EVERY occurence.
						inputWords.RemoveAll (x => x == Target);
						lBox.ProcessArguments = inputWords;
					}
				}
				break;

			case KastHookOption.Last:
				if (inputWords [inputWords.Count - 1].Equals (Target))
					lBox.ProcessArguments = Misc.Subsequence (inputWords, 0, inputWords.Count - 2);
				break;
			}

			if (lBox.ProcessArguments.Count >= 1) {
				lBox.ProcessBuffer ();
				return true;
			}
				return false;
		}

		/// <summary>
		/// Do not use. Implemented only for Polymorphism of all cast components.
		/// </summary>
		public void PulseReact(){
			Log.Warn ("KastHook.PulseReact() should never be called");
		}

		public string Latest(){
			return Box.Latest ();
		}

		public void Defaults(){
			Name = "";
			Option = KastHookOption.First;
		}

		public string GetName(){
			return Name;
		}

		/// <summary>
		/// Builds a KastHookOption from a string. Expects it to be formatted in
		/// such a way: "option {First|Last|InnerRemove|InnerKeep}"
		/// </summary>
		/// <param name="buildString">The string which contains an option</param>
		public static KastHookOption BuildKastHookOption(string buildString){
			buildString = buildString.ToLower ();

			if (buildString.Contains ("first"))
				return KastHookOption.First;

			if (buildString.Contains ("last"))
				return KastHookOption.Last;
				
			if (buildString.Contains ("innerremove"))
				return KastHookOption.InnerRemove;

			if (buildString.Contains ("innerkeep"))
				return KastHookOption.InnerKeep;

			throw new Exception ("BuildKastHookOption did not recognize: " + buildString);
		}
	}
}

