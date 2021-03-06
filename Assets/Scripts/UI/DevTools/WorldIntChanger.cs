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
        public float value;

        public WorldStateManager theWorld;

        // Start is called before the first frame update
        void Start()
        {
            // get the current world Value
            value = theWorld.GetWorldInt(worldStat);

            // Sets the title to the the string of the stat.
            if (text != null) { text.text = worldStat; }

            // Checking input Fields.
            if (inputField != null)
            {
                //Sets the content type and adds a listener for any world changes.
                inputField.contentType = TMP_InputField.ContentType.IntegerNumber;
                inputField.onEndEdit.AddListener(UpdateWorldValueFromInput);
            }
            else { Debug.LogWarning($"Error, {worldStat} int changer has no inputField"); }


            // Ensures that the slider is sliding on whole numbers
            if (currentSlider != null)
            {
                currentSlider.wholeNumbers = true;
                currentSlider.onValueChanged.AddListener(UpdateWorldValue);
            }
            
            // Sets a listener for an event.
            
            theWorld.IntChangeEvent+=(UpdateFromWorld);
        }

		private void UpdateFromWorld(object sender, string decider)
		{
            if(decider != worldStat) { return; }

            value = theWorld.GetWorldInt(worldStat);

			// changes the value to the string.
			valueText.text = value.ToString();

			// Change the bounds on the slider if it needs to be changed.
            if (currentSlider != null)
			{
		        if (value > currentSlider.maxValue) { currentSlider.maxValue = value; }
                else if(value < currentSlider.minValue) { currentSlider.minValue = value; }

                // Change the value on the slider itself.
			    currentSlider.value = value;
			}

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