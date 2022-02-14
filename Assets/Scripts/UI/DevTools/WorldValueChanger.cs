using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace StoryletTesting
{
    public class WorldValueChanger : MonoBehaviour
    {
        public TMP_Text text;
        public TMP_Text valueText;
        public Slider currentSlider; // drag it in.
        public TMP_InputField inputField; // also drag it in.
        public string worldStat; // probably should leave blank.
        float value;

        public WorldStateManager theWorld;

        // Start is called before the first frame update
        void Start()
        {
            // get the current world Value
            value = theWorld.GetWorldValue(worldStat);

            // Sets the title to the the string of the stat.
            text.text = worldStat;

            // Ensures that all input are floats
            inputField.contentType = TMP_InputField.ContentType.DecimalNumber;

            // Sets a listener for an event.
            inputField.onEndEdit.AddListener(UpdateWorldValueFromInput);
            currentSlider.onValueChanged.AddListener(UpdateWorldValue);

            theWorld.FloatChangeEvent += UpdateFromWorld;
        }

        private void UpdateFromWorld(object sender, string inputName)
        {
            if (inputName != worldStat) { return; }

            value = theWorld.GetWorldValue(worldStat);

            // changes the value to the string.
            valueText.text = value.ToString();

            // Change the bounds on the slider if it needs to be changed.
            if (value > currentSlider.maxValue) { currentSlider.maxValue = value; }
            else if (value < currentSlider.minValue) { currentSlider.minValue = value; }
            // Change the value on the slider itself.
            currentSlider.value = value;

        }

        public void UpdateWorldValueFromInput(string input)
        {
            UpdateWorldValue(float.Parse(input));
        }

        /// <summary>
        /// What we use to update the world value.
        /// </summary>
        /// <param name="inputChange">IDK, it needs this or something.</param>
        public void UpdateWorldValue(float inputChange)
        {
            // change the world value.
            theWorld.AddWorldValue(worldStat, inputChange);

            // Set the text to the world value.
            value = theWorld.GetWorldValue(worldStat);
        }
    }

}