using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    private QuestGenerator questGenerator;
    private int newQuestInterval = 3; //The time interval (in hours) in which new quests are generated

    private void Awake()
    {
        questGenerator = transform.Find("QuestGenerator").GetComponent<QuestGenerator>();
    }
    private void Start()
    {
        GameObject.Find("TimeSystem").GetComponent<TimeSystem>().TickAdded += CheckTime;
    }

    private void CheckTime(object source, GameTime gameTime)
    {
        if (gameTime.hour % newQuestInterval == 0)
        {
            Debug.Log("New Quest has been generated");
            QuestGenerator.QuestParameters questParameters;
            questParameters.length = 3;
            questParameters.questDifficulty = QuestGenerator.QuestDifficulty.HARD;
            questParameters.stats = new string[] { "diplomacy", "combat", "exploration", "stamina" };

            questGenerator.GenerateQuest(questParameters, "test");
        }
    }

}
