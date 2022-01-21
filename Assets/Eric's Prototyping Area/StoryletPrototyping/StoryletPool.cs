using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The StoryletPool will hold all the storylets in the game, then send storylets to the world once their conditions are correct.
/// </summary>
public class StoryletPool : MonoBehaviour
{
	public WorldStateManager worldStateManager;

	public List<Storylet> storylets = new List<Storylet>();


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
				// Check if worldStateManager contains the triggerValue. If it doesn't, it adds it.
				if (!worldStateManager.worldValues.ContainsKey(triggerValue.name)) { worldStateManager.addWorldValue(name, 0);}

				// create a copy of the world's current value to check against.
				float worldValue = worldStateManager.worldValues[triggerValue.name].value;
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
				// Check if worldStateManager contrains the triggerState. If it doesn't, it adds it, assuming false.
				if (!worldStateManager.worldStates.ContainsKey(triggerState.name)) { worldStateManager.addWorldState(name, false); }

				// Check if the trigger state matches the world state.
				if (worldStateManager.worldStates[triggerState.name].state != triggerState.state) { validStorylet = false; break; }
			}
			
			// if this is not a valid storylet after checking through the trigger states, keep searching. otherwise, add to valid storylets.
			if (!validStorylet) { continue; }
			else { validStorylets.Add(storylet); }
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
		}
	}

}
