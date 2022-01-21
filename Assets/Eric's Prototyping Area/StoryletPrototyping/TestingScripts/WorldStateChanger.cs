using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorldStateChanger : MonoBehaviour
{

    public TMP_Text text;
    public string headerMessage;
    public string worldState;
    bool state;
    Button theButton;
    

    public WorldStateManager theWorld;

    // Start is called before the first frame update
    void Start()
    {
        // Set references to the slider.
        theButton = GetComponent<Button>();

        // Set up the world to behave with the slider.
        theWorld.addWorldState(worldState, false);

        // get the current world Value
        state = theWorld.getWorldState(worldState);

        // Set the text to the world value.
        text.text = $"{headerMessage}: {state}";
    }

    /// <summary>
    /// What we use to update the world value.
    /// </summary>
    /// <param name="inputChange">IDK, it needs this or something.</param>
    public void UpdateWorldState()
    {
        // change the world value.
        theWorld.addWorldState(worldState, !state);

        // Set the text to the world value.
        state = theWorld.getWorldState(worldState);
        text.text = $"{headerMessage}: {state}";
    }
}
