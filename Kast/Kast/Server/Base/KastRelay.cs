using System;
using System.Collections.Generic;

using Kast.Server.Feed;
using Kast.Server.General;
using Kast.Server.Hook;

namespace Kast.Server.Base
{
	/// <summary>
	/// The relay is the backbone of the Kast system. It controls
	/// the creation and management of all Boxes, Feeds, and Hooks.
	/// </summary>
	public class KastRelay
	{
		/// <summary>
		/// Boxes declared as independant workers
		/// </summary>
		/// <value>The active boxes.</value>
		private List<KastBox> ActiveBoxes {get; set;}

		/// <summary>
		/// Feeds
		/// </summary>
		/// <value>The feeds.</value>
		private List<KastFeed> Feeds { get; set; }

		/// <summary>
		/// Hooks
		/// </summary>
		/// <value>The hooks.</value>
		private List<KastHook> Hooks { get; set; }

		/// <summary>
		/// Every component in the relay
		/// </summary>
		public List<IKastComponent> Components {get; set;}

		/// <summary>
		/// Create an empty KastRelay
		/// </summary>
		public KastRelay()
		{
			ActiveBoxes = new List<KastBox>();
			Feeds = new List<KastFeed> ();
			Hooks = new List<KastHook> ();
			Components = new List<IKastComponent> ();
		}

		/// <summary>
		/// Add a new KastComponent to the list of active components
		/// </summary>
		/// <param name="component">Component.</param>
		public void AddComponent(IKastComponent component){
			if (component is KastBox)
				ActiveBoxes.Add (component as KastBox);
			else if (component is KastFeed)
				Feeds.Add (component as KastFeed);
			else if (component is KastHook)
				Hooks.Add (component as KastHook);

			Components.Add (component);

			Console.WriteLine ("Component added to relay");
		}

		/// <summary>
		/// Add a component to the relay with raw information
		/// </summary>
		/// <param name="args">Arguments.</param>
		public void AddComponent(string[] args){
			AddComponent (KastBuilder.Build (args));
		}

		/// <summary>
		/// Add the component from the string
		/// </summary>
		/// <param name="argument">Argument.</param>
		public void AddComponent(string argument){
			AddComponent (argument.Split (' '));
		}

		/// <summary>
		/// Removes the component from the list of Active Components.
		/// </summary>
		/// <param name="component">Component.</param>
		public void RemoveComponent(IKastComponent component){
			if (component is KastBox)
				ActiveBoxes.Remove (component as KastBox);
			else if (component is KastFeed)
				Feeds.Remove(component as KastFeed);
			else if (component is KastHook)
				Hooks.Remove (component as KastHook);

			Components.Remove (component);
		}
			
		/// <summary>
		/// Return the first KastComponent with a given name
		/// </summary>
		/// <returns>The component by name.</returns>
		/// <param name="name">Name.</param>
		public IKastComponent GetComponentByName(string name){
			foreach (IKastComponent component in Components) {
				if (component == null)
					continue;

				if (component.GetName ().Equals (name))
					return component;
			}
			return null;
		}

		/// <summary>
		/// Places two named components in a feed.
		/// </summary>
		/// <param name="name1">Name1.</param>
		/// <param name="name2">Name2.</param>
		/// <param name="config">The configuration to use for this Feed.</param>
		public void BindComponents(string name1, string name2, KastConfiguration config){
			IKastComponent item1 = GetComponentByName (name1);
			IKastComponent item2 = GetComponentByName (name2);

			// Weed out all possible sources of error.
			if (item1 == null || item2 == null || (!(item1 is KastBox)) || (!(item2 is KastBox)))
				return;

			Feeds.Add (new KastFeed (item1 as KastBox,
				item2 as KastBox,
				config));
		}

		/// <summary>
		/// Fill in futures if possible
		/// </summary>
		public void FillFutures(){
			// Replace feed futures
			foreach(KastFeed item in Feeds){
				if (item.Source is KastFuture) {
					var possibleFuture = GetComponentByName ((item.Source as KastFuture).Name);
					item.Source = possibleFuture ?? item.Source;
				}
				if (item.Destination is KastFuture) {
					var possibleFuture = GetComponentByName ((item.Destination as KastFuture).Name);
					item.Destination = possibleFuture ?? item.Source;
				}
			}

			// Replace hook futures
			foreach (KastHook item in Hooks) {
				if (item.Box is KastFuture) {
					var possibleFuture = GetComponentByName ((item.Box as KastFuture).Name);
					item.Box = possibleFuture ?? item.Box;
				}
			}
		}

		/// <summary>
		/// Pulse will cause all of the boxes to perform
		/// their actions.
		/// </summary>
		public void Pulse(){

			// Attempt to satisfy any futures
			FillFutures ();

			// Make the boxes and feeds react
			ActiveBoxes.ForEach (box => box.PulseReact ());
			Feeds.ForEach (feed => feed.PulseReact ());

			// Make the hooks react to the boxes and feeds
			foreach(KastHook hook in Hooks)
				foreach(IKastComponent component in Components.FindAll(x => x is KastBox || x is KastFeed))
					hook.React (component.Latest ());

			// Make the hooks react to hooks
			foreach (KastHook hook in Hooks)
				foreach (KastHook otherHook in Hooks)
					if (hook != otherHook)
						hook.React (otherHook.Latest ());
			
		}

		/// <summary>
		/// Print the status of the Relay
		/// </summary>
		public void PrintStatus(){
			Console.WriteLine (ActiveBoxes.Count + " boxes.");
			Console.WriteLine (Hooks.Count + " hooks");
			Console.WriteLine (Feeds.Count + " feeds");
		}
	}
}

