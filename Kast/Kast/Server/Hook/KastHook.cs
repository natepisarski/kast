using System;

using System.Collections.Generic;
using Kast.Server.General;

namespace Kast.Server.Hook
{
	/// <summary>
	/// Hooks will capture the input of any command that matches
	/// their Target, and execute, using the rest of the string as an argument.
	/// </summary>
	public class KastHook : KastComponent
	{
		public KastComponent Box {get; set; }

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
		/// Makes a KastHook using an existing Box and a string target.
		/// </summary>
		/// <param name="box">Box.</param>
		/// <param name="target">Target.</param>
		/// <param name="option">The option to use</param> 
		public KastHook (KastBox box, string target, KastHookOption option, KastConfiguration masterConfig)
		{
			Box = box; 
			Target = target;
			Option = option;

			Name = "";

			MasterConfig = masterConfig;
			Log = new Logger (masterConfig);
		}

		/// <summary>
		/// Build a KastHook using a KastConfiguration
		/// </summary>
		/// <param name="box">The box to capture.</param> 
		/// <param name="target">The string that will make this hook fire</param>
		/// <param name="config">Configuration assets, such as name and option.</param>
		public KastHook(KastComponent box, string target, KastConfiguration config, KastConfiguration masterConfig){
			Box = box;
			Target = target;
			MasterConfig = masterConfig;
			Log = new Logger (masterConfig);
			try {
				Option = this.BuildKastHookOption(config.Get("option"));
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
		public override void PulseReact(){
			Log.Warn ("KastHook.PulseReact() should never be called");
		}

		/// <summary>
		/// Get the box's latest output
		/// </summary>
		public override string Latest(){
			return Box.Latest ();
		}

		/// <summary>
		/// Set the default options for this hook
		/// </summary>
		public override void Defaults(){
			Name = "";
			Option = KastHookOption.First;
		}

		/// <summary>
		/// Builds a KastHookOption from a string. Expects it to be formatted in
		/// such a way: "option {First|Last|InnerRemove|InnerKeep}"
		/// </summary>
		/// <param name="buildString">The string which contains an option</param>
		public KastHookOption BuildKastHookOption(string buildString){
			try{
			buildString = buildString.ToLower ();

			if (buildString.Contains ("first"))
				return KastHookOption.First;

			if (buildString.Contains ("last"))
				return KastHookOption.Last;
				
			if (buildString.Contains ("innerremove"))
				return KastHookOption.InnerRemove;

			if (buildString.Contains ("innerkeep"))
				return KastHookOption.InnerKeep;

				throw new Exception (MasterConfig.Get("message_kasthook_option_unrecognized") + buildString);
			} catch(Exception e){
				Log.Warn (e.Message);
				return this.BuildKastHookOption (MasterConfig.Get ("default_kasthook_option"));
			}
		}
	}
}

