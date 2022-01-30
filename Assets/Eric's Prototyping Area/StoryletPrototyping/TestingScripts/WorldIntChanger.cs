using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace StoryletTesting {

    public class WorldIntChanger : MonoBehaviour
    {
        public TMP_Text text;
        public string worldStat; // probably should leave blank.
        public Slider currentSlider; // drag it in.
        public TMP_InputField inputField; // also drag it in.
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

            // Sets a listener for an event.
            inputField.onEndEdit.AddListener(UpdateWorldValueFromInput);
        }

		private void Update()
		{
		    
		}

        public void UpdateWorldValueFromInput(string input)
		{
            
		}

		/// <summary>
		/// What we use to update the world value.
		/// </summary>
		/// <param name="inputChange">IDK, it needs this or something.</param>
		public void UpdateWorldValue(float inputChange)
        {
            // change the world value.
            theWorld.AddWorldValue(worldStat, currentSlider.value);

            // Set the text to the world value.
            value = theWorld.GetWorldValue(worldStat);
        }

        public void IncrementValue()
		{
		}
    }

}