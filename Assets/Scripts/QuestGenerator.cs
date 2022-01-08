using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGenerator : MonoBehaviour
{
    public QuestingManager questingManager;

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

    // Quest is generated based off of these parameters
    public struct QuestParameters {
        public int length; //How many events the party needs to go through before finishing the quest
        public QuestDifficulty questDifficulty; //Determines how difficult the DC checks are
        public string[] stats; //What stats should be used for dc checks. Chosen at random
    }

    /// <summary>
    /// Generates a quest and adds it to the bankedQuests list in questingManager
    /// </summary>
    /// <param name="questParameters">A struct containing parameters that will be used to generate the quest</param>
    /// <param name="questName">Name of the quest</param>
    public void GenerateQuest(QuestParameters questParameters, string questName) {
        //Create head node and set its eventType to head
        EventNode head = GenerateNode(questParameters);
        head.eventType = EventNode.EventTypes.head;
        // Debug.Log("Head Node: " + "Level: " + questParameters.length + ", " + head.stat + ", " + head.DC + ", " + head.time);
        
        //Attach additional nodes to the head node
        AttachNodes(questParameters, head, questParameters.length - 1);

        //Create new quest sheet and add it to the questingManager
        QuestSheet questSheet = new QuestSheet(head, questName);
        questingManager.bankedQuests.Add(questSheet);

        return;
    }

    /// <summary>
    /// Recursively attaches success and fail nodes to the given node until length = 0
    /// </summary>
    /// <param name="questParameters">Same questParameters struct passed to GenerateQuest()</param>
    /// <param name="node">Success and fail nodes will be attached to this node</param>
    /// <param name="length">Used to determine when to end recursive call</param>
    private void AttachNodes(QuestParameters questParameters, EventNode node, int length)
    {
        if (length == 0)
            return;

        //Generate success and fail nodes given the quest Parameters
        EventNode successNode = GenerateNode(questParameters);
        EventNode failNode = GenerateNode(questParameters);
        successNode.eventType = EventNode.EventTypes.successful;
        failNode.eventType = EventNode.EventTypes.fail;

        //Add connections
        node.addConnection(successNode);
        node.addConnection(failNode);

        //Debug.Log("Success Node: " + "Level: " + length + ", " + successNode.stat + ", " + successNode.DC + ", " + successNode.time);
        //Debug.Log("Fail Node: " + "Level: " + length + ", " + failNode.stat + ", " + failNode.DC + ", " + failNode.time);

        //Recursively call AttachNodes on the newly generated nodes
        AttachNodes(questParameters, successNode, length - 1);
        AttachNodes(questParameters, failNode, length - 1);
    }

    private EventNode GenerateNode(QuestParameters questParameters)
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
        questingManager = GameObject.Find("QuestingManager").GetComponent<QuestingManager>();
        // Gets the quest manager for use in other functions. And to throw finished quests into.
        //questingManager = GameObject.Find("QuestManager").GetComponent<QuestingManager>();
        /*

        QuestParameters questParameters;
        questParameters.length = 3;
        questParameters.questDifficulty = QuestDifficulty.HARD;
        questParameters.stats = new string[] { "diplomacy", "combat", "exploration", "stamina" };

        GenerateQuest(questParameters, "test");
        */
    }
}
