using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace StoryletTesting
{

    public class ActiveQuestScripts : MonoBehaviour
    {
        public QuestingManager theQuestingManager;
		public TMP_Text text;
		public string starterText;

		private void Start()
		{
			text.text = starterText;
		}

		private void Update()
		{
			string biglist = "";

			foreach (QuestSheet bankedQuest in theQuestingManager.bankedQuests)
			{
				biglist = "\n" + $"{bankedQuest.questName}";
			}

			text.text = starterText + biglist;
		}
	}
}
