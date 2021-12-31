using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGenerator : MonoBehaviour
{
    public QuestingManager questingManager;
    // public string[] stats = { "diplomacy", "combat", "exploration", "stamina"};

    //Bounds for DC check based on difficulty, inclusive
    private const int EASY_DC_LOW = 5;
    private const int EASY_DC_HIGH = 15;
    private const int MED_DC_LOW = 16;
    private const int MED_DC_HIGH = 30;
    private const int HARD_DC_LOW = 31;
    private const int HARD_DC_HIGH = 50;

    //Bounds for time based on difficulty, inclusive
    private const int EASY_TIME_LOW = 35;
    private const int EASY_TIME_HIGH = 50;
    private const int MED_TIME_LOW = 25;
    private const int MED_TIME_HIGH = 40;
    private const int HARD_TIME_LOW = 15;
    private const int HARD_TIME_HIGH = 30;


    public enum QuestDifficulty {
        EASY, NORMAL, HARD
    }

    public struct QuestParameters {
        public int length; //How many events the party needs to go through before finishing the quest
        public QuestDifficulty questDifficulty; //Determines how difficult the DC checks are
        public string[] stats; //What stats should be used for dc checks. Chosen at random
    }

    QuestSheet GenerateQuest(QuestParameters questParameters, string questName) {
        EventNode head = GenerateNode(questParameters);
        head.eventType = EventNode.EventTypes.head;

        Debug.Log("Head Node: " + "Level: " + questParameters.length + ", " + head.stat + ", " + head.DC + ", " + head.time);
        AttachNodes(questParameters, head, questParameters.length - 1);

        QuestSheet questSheet = new QuestSheet(head, questName);
        
        return questSheet;

        
    }

    //Recursively attaches success and fail nodes to the given node
    void AttachNodes(QuestParameters questParameters, EventNode node, int length)
    {
        if (length == 0)
            return;

        EventNode successNode = GenerateNode(questParameters);
        EventNode failNode = GenerateNode(questParameters);
        successNode.eventType = EventNode.EventTypes.head;
        failNode.eventType = EventNode.EventTypes.fail;

        node.addConnection(successNode);
        node.addConnection(failNode);

        Debug.Log("Success Node: " + "Level: " + length + ", " + successNode.stat + ", " + successNode.DC + ", " + successNode.time);
        Debug.Log("Fail Node: " + "Level: " + length + ", " + failNode.stat + ", " + failNode.DC + ", " + failNode.time);

        AttachNodes(questParameters, successNode, length - 1);
        AttachNodes(questParameters, failNode, length - 1);

    }

    EventNode GenerateNode(QuestParameters questParameters)
    {
        string stat;
        int dc = 0;
        int time = 0;

        int rand = Random.Range(0, questParameters.stats.Length - 1);

        stat = questParameters.stats[rand];

        switch (questParameters.questDifficulty)
        {
            case (QuestDifficulty.EASY): { dc = Random.Range(EASY_DC_LOW, EASY_DC_HIGH); break; }
            case (QuestDifficulty.NORMAL): { dc = Random.Range(MED_DC_LOW, MED_DC_HIGH); break; }
            case (QuestDifficulty.HARD): { dc = Random.Range(HARD_DC_LOW, HARD_DC_HIGH); break; }
        }

        switch (questParameters.questDifficulty)
        {
            case (QuestDifficulty.EASY): { time = Random.Range(EASY_TIME_LOW, EASY_TIME_HIGH); break; }
            case (QuestDifficulty.NORMAL): { time = Random.Range(MED_TIME_LOW, MED_TIME_HIGH); break; }
            case (QuestDifficulty.HARD): { time = Random.Range(HARD_TIME_LOW, HARD_TIME_HIGH); break; }
        }

        EventNode node = new EventNode(stat, dc, time);

        return node;
    }

    private void Start()
    {
        QuestParameters questParameters;
        questParameters.length = 3;
        questParameters.questDifficulty = QuestDifficulty.HARD;
        questParameters.stats = new string[] { "diplomacy", "combat", "exploration", "stamina" };

        QuestSheet questSheet = GenerateQuest(questParameters, "test");
    }
}
