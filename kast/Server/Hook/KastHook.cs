using System;

using System.Collections.Generic;

namespace Kast.General
{
	/// <summary>
	/// Hooks will capture the input of any command that matches
	/// their Target, and execute, using the rest of the string as an argument.
	/// </summary>
	public class KastHook : IKastComponent
	{
		private KastBox Box {get; set; }

		/// <summary>
		/// Gets or sets the target, the value that will cause this
		/// Hook's Box to act.
		/// </summary>
		/// <value>The target.</value>
		public string Target { get; set; }

		/// <summary>
		/// Define how this KastHook should react to input
		/// </summary>
		/// <value>The option.</value>
		public KastHookOption Option { get; set; }

		/// <summary>
		/// Name the Box so that it can be accessed from the Relay.
		/// </summary>
		/// <value>The name.</value>
		public string Name {get; set;}

		/// <summary>
		/// Makes a KastHook using an existing Box and a string target.
		/// </summary>
		/// <param name="box">Box.</param>
		/// <param name="target">Target.</param>
		/// <param name="option">The option to use</param> 
		public KastHook (KastBox box, string target, KastHookOption option)
		{
			Box = box; 
			Target = target;
			Option = option;
		}

		/// <summary>
		/// Makes a new KastHook using a process name to create a new box.
		/// </summary>
		/// <param name="processName">Process name.</param>
		/// <param name="target">The string to match</param>
		/// <param name="option">The option to use</param> 
		public KastHook(string processName, string target, KastHookOption option){
			Box = new KastBox(processName);
			Target = target;
			Option = option;
		}

		/// <summary>
		/// Create a new Named Hook
		/// </summary>
		/// <param name="box">Box.</param>
		/// <param name="target">Target.</param>
		/// <param name="option">Option.</param>
		/// <param name="name">Name.</param>
		public KastHook (KastBox box, string target, KastHookOption option, string name){
			Box = box;
			Target = target;
			Option = option;
			Name = name;
		}

		/// <summary>
		/// Create a new named hook from a string
		/// </summary>
		/// <param name="processName">Process name.</param>
		/// <param name="target">Target.</param>
		/// <param name="option">Option.</param>
		/// <param name="name">Name.</param>
		public KastHook(string processName, string target, KastHookOption option, string name){
			Box = new KastBox (processName, name);
			Target = target;
			Option = option;
			Name = name;
		}

		/// <summary>
		/// React the specified input, according to the KastHookOption
		/// </summary>
		/// <param name="input">Input.</param>
		public bool React(string input){
			var inputWords = new List<string>(input.Split(' '));

			// Clear the old argument list
			Box.ProcessArguments = new List<string> ();

			switch(Option){

			case KastHookOption.First:
				if (inputWords [0].Equals (Target))
					Box.ProcessArguments = Misc.Subsequence (inputWords, 1, inputWords.Count);
				break;

			case KastHookOption.InnerKeep:
			case KastHookOption.InnerRemove:
				if (Misc.Any (inputWords, (x) => x == Target)) {
					if (Option == KastHookOption.InnerKeep)
						Box.ProcessArguments = inputWords;
					else{
						// removes EVERY occurence.
						inputWords.RemoveAll (x => x == Target);
						Box.ProcessArguments = inputWords;
					}
				}
				break;

			case KastHookOption.Last:
				if (inputWords [inputWords.Count - 1].Equals (Target))
					Box.ProcessArguments = Misc.Subsequence (inputWords, 0, inputWords.Count - 2);
				break;
			}

			if (!Box.ProcessArguments.Equals (new List<string> ())) {
				Box.ProcessBuffer ();
				return true;
			} else
				return false;
		}

		/// <summary>
		/// Do not use. Implemented only for Polymorphism of all cast components.
		/// </summary>
		public void PulseReact(){
			Console.WriteLine("[Warning]: KastHook.PulseReact() should never be called.");
		}

		public string Latest(){
			return Box.Latest ();
		}
	}
}

