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
        }

		private void Update()
		{
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