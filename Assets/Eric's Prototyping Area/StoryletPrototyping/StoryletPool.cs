using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The StoryletPool will hold all the storylets in the game, then send storylets to the world once their conditions are correct.
/// </summary>
public class StoryletPool : MonoBehaviour
{

	public List<Storylet> storylets = new List<Storylet>();

	/// <summary>
	/// Checks all storylets for correct conditions, then triggers them.
	/// </summary>
	/// <param name="world">The World State Manager</param>
	public void CheckPool(WorldStateManager world)
	{
		List<Storylet> validStorylets = new List<Storylet>();
		
		// checks all our storylets to see if there are any valid storylets to trigger.
		foreach (Storylet storylet in storylets)
		{
			// Checks to see if it can be instanced, and if it can't, whether we've instanced it already.
			if (!storylet.canBeInstanced && storylet.numInstances > 0) { continue; }

			bool validStorylet = true;

			// Goes through the list of trigger values.
			foreach (Storylet.triggerValue triggerValue in storylet.triggerValues)
			{

				// create a copy of the world's current value to check against.
				float worldValue = world.getWorldValue(triggerValue.name);
				switch (triggerValue.triggerType)
				{
					case -1: // Fail check if world value is more.
						if (worldValue > triggerValue.value) { validStorylet = false;}
						break;
					case 0: // checks for exact equal value. fail check if world value is not exact.
						if (worldValue != triggerValue.value) { validStorylet = false;}
						break;
					case 1: // Fail check if world value is less.
						if (worldValue < triggerValue.value) { validStorylet = false;}
						break;
					default:
						break;
				}
				// If the value ended up false, stops checking other values. 
				if (!validStorylet) { break; }
			}

			// if this is not a valid storylet, keep searching
			if (!validStorylet) { continue; }
			
			foreach(Storylet.triggerState triggerState in storylet.triggerStates)
			{
				// Check if the trigger state matches the world state.
				if (world.getWorldState(triggerState.name) != triggerState.state) { validStorylet = false; break; }
			}
			
			// if this is not a valid storylet after checking through the trigger states, keep searching. otherwise, add to valid storylets.
			if (!validStorylet) { continue; }
			else { validStorylets.Add(storylet); Debug.Log($"Storylet {storylet.questName} works."); }
		}

		// goes through the list of valid storylets and triggers them.
		foreach (Storylet storylet in validStorylets)
		{
			
			// Goes through the storylets and applies worldchanges.
			foreach(Storylet.ValueChange valueChange in storylet.triggerValueChanges)
			{
				// Checks to set it directly, or to change it by a value.
				if (valueChange.set == true) { world.addWorldValue(valueChange.name, valueChange.value); }
				else { world.changeWorldValue(valueChange.name, valueChange.value); }
			}
			foreach(Storylet.StateChange change in storylet.triggerStateChanges)
			{
				world.addWorldState(change.name, change.state);
			}

			storylet.numInstances++;

			Debug.Log($"New Quest {storylet.name} created.");
			// InsertQuestAttachment to Quest here.
		}
	}

}
