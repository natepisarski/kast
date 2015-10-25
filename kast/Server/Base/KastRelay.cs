using System;
using System.Collections.Generic;

using Kast.Base;
using Kast.Feed;
using Kast.General;

namespace Kast.Base
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
		/// Pulse will cause all of the boxes to perform
		/// their actions.
		/// </summary>
		public void Pulse(){

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
	}
}

