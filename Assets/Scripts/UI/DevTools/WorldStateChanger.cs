using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace StoryletTesting
{
    public class WorldStateChanger : MonoBehaviour
    {
        public TMP_Text text;
        public Button button; // A button
        public TMP_Text buttonText;
        public string worldStat; // probably should leave blank.
        bool value;

        public WorldStateManager theWorld;

        // Start is called before the first frame update
        void Start()
        {
            // get the current world Value
            value = theWorld.GetWorldState(worldStat);

            // Sets the title to the the string of the stat.
            text.text = worldStat;

            // Add an listener for world events.
            theWorld.StateChangeEvent += UpdateFromWorld;
        }

		private void UpdateFromWorld(object sender, string inputName)
		{
            if (inputName != worldStat) { return; }
            value = theWorld.GetWorldState(worldStat);
            buttonText.text = "is: " + value;
		}

        public void ReverseButton()
		{
            theWorld.AddWorldState(worldStat, !value);
            value = theWorld.GetWorldState(worldStat);
		}
	}

}