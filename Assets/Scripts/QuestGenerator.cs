using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGenerator : MonoBehaviour
{
    public GameObject questPrefab;
    public QuestingManager questingManager;
    public GameObject QuestDisplay;
    public DropHandler dropHandler;

    [Range(0, 1)]
    public float DcCheckDifficulty;
    [Range(0, 1)]
    public float timeLimitDifficulty;

    //Lower and upper bounds for the dc checks and time limits
    private const int DC_CHECK_LOW_BOUND = 5;
    private const int DC_CHECK_HIGH_BOUND = 50;
    private const int TIME_LIMIT_LOW_BOUND = 15;
    private const int TIME_LIMIT_HIGH_BOUND = 50;

    //How much the dc check or time limit can deviate from its original value
    private const float RANDOM_PERCENT_DEVIATION = 0.25f;
    
    //Used to change dc check and time limie difficulty overtime
    [System.Serializable]
    public struct DifficultyTimeScale
    {
        public GameTime gameTime;
        [Range(0, 1)]
        public float dcDifficulty;
        [Range(0, 1)]
        public float timeDifficulty;
    }

    public List<DifficultyTimeScale> difficulties;

    // Quest is generated based off of these parameters
    public struct QuestParameters {
        public int length; //How many events the party needs to go through before finishing the quest
        public string[] stats; //What stats should be used for dc checks. Chosen at random
    }
    private void Start()
    {
        questingManager = GameObject.Find("QuestingManager").GetComponent<QuestingManager>();
        GameObject.Find("TimeSystem").GetComponent<TimeSystem>().TickAdded += CheckTime;
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
        QuestSheet questSheet = new QuestSheet(head, questName, 6);
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

    // Generates a node with the given questParamters. 
    private EventNode GenerateNode(QuestParameters questParameters)
    {
        int dc;
        int time;
        string stat = questParameters.stats[Random.Range(0, questParameters.stats.Length - 1)];

        dc = Mathf.RoundToInt(DC_CHECK_LOW_BOUND + ((DC_CHECK_HIGH_BOUND - DC_CHECK_LOW_BOUND) * DcCheckDifficulty));
        dc = Random.Range(dc -= Mathf.RoundToInt(dc * RANDOM_PERCENT_DEVIATION), dc += Mathf.RoundToInt(dc * RANDOM_PERCENT_DEVIATION));
        dc = Mathf.Clamp(dc, DC_CHECK_LOW_BOUND, DC_CHECK_HIGH_BOUND);

        time = Mathf.RoundToInt(TIME_LIMIT_HIGH_BOUND - ((TIME_LIMIT_HIGH_BOUND - TIME_LIMIT_LOW_BOUND) * timeLimitDifficulty));
        time = Random.Range(time -= Mathf.RoundToInt(time * RANDOM_PERCENT_DEVIATION), time += Mathf.RoundToInt(time * RANDOM_PERCENT_DEVIATION));
        time = Mathf.Clamp(time, TIME_LIMIT_LOW_BOUND, TIME_LIMIT_HIGH_BOUND);

        Debug.Log("Generated node with (dc,time) = (" + dc + "," + time + ")");

        EventNode node = new EventNode(stat, dc, time);

        return node;
    }

    //Will check difficulties list and see if the quest difficulties need to be updated
    private void CheckTime(object source, GameTime gameTime)
    {
        foreach(DifficultyTimeScale difficulty in difficulties)
        {
            if(difficulty.gameTime.day == gameTime.day && difficulty.gameTime.hour == gameTime.hour)
            {
                DcCheckDifficulty = difficulty.dcDifficulty;
                timeLimitDifficulty = difficulty.timeDifficulty;
            }
        }
    }
}
