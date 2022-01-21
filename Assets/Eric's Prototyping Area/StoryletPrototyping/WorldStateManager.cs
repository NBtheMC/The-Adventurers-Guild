using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldStateManager : MonoBehaviour
{
    private Dictionary<string, WorldValue> worldValues;
    private Dictionary<string, WorldState> worldStates;

    public StoryletPool storyletPool;

    // Start is called before the first frame update
    void Awake()
    {
        worldValues = new Dictionary<string, WorldValue>();
        worldStates = new Dictionary<string, WorldState>();
    }

    /// <summary>
    /// Adds a new World Value to the World State Manager.
    /// If the value already exists, it replaces the existed value with the new value.
    /// Therefore, this can also be used to set values.
    /// </summary>
    /// <param name="name">Name of the value</param>
    /// <param name="value">The value.</param>
    public void addWorldValue(string name, float value)
    {
        if (!worldValues.ContainsKey(name))
        {
            worldValues.Add(name, new WorldValue(name, value));
        }
        else { worldValues[name].value = value; }
    }

    /// <summary>
    /// Adds a new World State to the World State Manager
    /// If the state already exists, it replaces the existing state with the new state.
    /// Therefore, this can also be used to set states.
    /// </summary>
    /// <param name="name">Name of the State</param>
    /// <param name="value">The State.</param>
    public void addWorldState(string name, bool state)
	{
        if (!worldStates.ContainsKey(name))
        {
            worldStates.Add(name, new WorldState(name, state));
        }
        else
        {
            worldStates[name].state = state;
        }
    }

    /// <summary>
    /// Changes the world value by a certain amount.
    /// </summary>
    /// <param name="name">Name of the value to change.</param>
    /// <param name="value">The change to be added to current value.</param>
    public void changeWorldValue(string name, float value)
	{
        if (!worldValues.ContainsKey(name)) { addWorldValue(name, value); }
        else { worldValues[name].value += value; }
	}

    // You don't need a changeWorldState. Just set it using addWorldState.

    public float getWorldValue(string key)
	{
        WorldValue specifiedWorldValue;
        if (worldValues.TryGetValue(key, out specifiedWorldValue))
		{
            return specifiedWorldValue.value;
		}
		else { addWorldValue(key, 0); return 0; }
	}

    public bool getWorldState(string key)
	{
        WorldState specifiedWorldState;
        if (worldStates.TryGetValue(key, out specifiedWorldState))
		{
            return specifiedWorldState.state;
		}
        else { addWorldState(key, false); return false; }
	}


    /// <summary>
    /// Checks the Storylet Pool for a good storylet to hit.
    /// </summary>
    public void TriggerStorylets()
	{
        storyletPool.CheckPool(this);
        Debug.Log("Checking for Storylets");
	}
}

// Standard Structures for keeping our worldStates.
public class WorldValue
{
    public string name; public float value;

    public WorldValue(string inputName, float inputValue) { inputName = name; inputValue = value; }   
}
public class WorldState
{
    public string name; public bool state;

    public WorldState(string inputName, bool inputState) { inputName = name; inputState = state;}
}