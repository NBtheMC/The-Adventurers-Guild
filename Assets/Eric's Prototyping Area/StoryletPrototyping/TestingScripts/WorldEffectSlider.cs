using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorldEffectSlider : MonoBehaviour
{
    public TMP_Text text;
    public string headerMessage;
    public string worldStat;
    float value;
    Slider currentSlider;

    public WorldStateManager theWorld;

    // Start is called before the first frame update
    void Start()
    {
        // Set references to the slider.
        currentSlider = this.GetComponent<Slider>();

        // Set up the world to behave with the slider.
        theWorld.addWorldValue(worldStat, currentSlider.value);

        // get the current world Value
        value = theWorld.worldValues[worldStat].value;

        // Set the text to the world value.
        text.text = $"{headerMessage}: {value}";
        currentSlider.onValueChanged.AddListener(UpdateWorldValue);
    }

    /// <summary>
    /// What we use to update the world value.
    /// </summary>
    /// <param name="inputChange">IDK, it needs this or something.</param>
    void UpdateWorldValue(float inputChange)
	{
        // change the world value.
        theWorld.addWorldValue(worldStat, currentSlider.value);

        // Set the text to the world value.
        value = theWorld.worldValues[worldStat].value;
        text.text = $"{headerMessage}: {value}";
    }
}
