using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGenerator : MonoBehaviour
{
    public QuestingManager questingManager;
    public string[] stats = { "diplomacy", "combat", "exploration", "stamina"};

    //Bounds for DC check based on difficulty, inclusive
    public const int EASY_DC_LOW = 5;
    public const int EASY_DC_HIGH = 15;
    public const int MED_DC_LOW = 16;
    public const int MED_DC_HIGH = 30;
    public const int HARD_DC_LOW = 31;
    public const int HARD_DC_HIGH = 50;

    public enum QuestDifficulty {
        EASY, NORMAL, HARD
    }

    public struct QuestParameters {
        public int length; //How many events the party needs to go through before finishing the quest
        public QuestDifficulty questDifficulty; //Determines how high the DC checks are
    }

    void GenerateQuest(QuestParameters questParameters) {
        string stat;
        int dc = 0;
        int time;
        
        int rand = Random.Range(0, 3);
        stat = stats[rand];

        switch(questParameters.questDifficulty)
        {
            case (QuestDifficulty.EASY): { dc = Random.Range(EASY_DC_LOW, EASY_DC_HIGH); break; }
            case (QuestDifficulty.NORMAL): { dc = Random.Range(MED_DC_LOW, MED_DC_HIGH); break; }
            case (QuestDifficulty.HARD): { dc = Random.Range(HARD_DC_LOW, HARD_DC_HIGH); break; }
        }

        time = 30;

        EventNode head = new EventNode(stat, dc, time);
    }

}
