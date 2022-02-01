using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace StoryletTesting {

    public class WorldIntChanger : MonoBehaviour
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
            value = theWorld.GetWorldInt(worldStat);

            // Sets the title to the the string of the stat.
            text.text = worldStat;

            // Ensures that all input are intergers.
            inputField.contentType = TMP_InputField.ContentType.IntegerNumber;

            // Ensures that the slider is sliding on whole numbers
            currentSlider.wholeNumbers = true;

            // Sets a listener for an event.
            inputField.onEndEdit.AddListener(UpdateWorldValueFromInput);
            currentSlider.onValueChanged.AddListener(UpdateWorldValue);

        }

		private void Update()
		{
			// changes the value to the string.
			valueText.text = value.ToString();

			// Change the bounds on the slider if it needs to be changed.
			if (value > currentSlider.maxValue) { currentSlider.maxValue = value; }
            else if(value < currentSlider.minValue) { currentSlider.minValue = value; }
            // Change the value on the slider itself.
			currentSlider.value = value;

		}

        public void UpdateWorldValueFromInput(string input)
		{
            UpdateWorldValue(int.Parse(input));
		}

		/// <summary>
		/// What we use to update the world value.
		/// </summary>
		/// <param name="inputChange">IDK, it needs this or something.</param>
		public void UpdateWorldValue(float inputChange)
        {
            // change the world value.
            theWorld.AddWorldInt(worldStat,(int)inputChange);

            // Set the text to the world value.
            value = theWorld.GetWorldInt(worldStat);
        }
    }

}