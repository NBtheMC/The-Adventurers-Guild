using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps track of active quests and advances their ticks.
/// </summary>
public class QuestingManager : MonoBehaviour
{
    public TimeSystem timeSystem; // a reference to the time system to update quest with.
    public List<QuestSheet> activeQuests; // All quests currently embarked.
    public List<QuestSheet> bankedQuests; // All quests waiting to be embarked.
    public List<QuestSheet> finishedQuests; //All quests that have been finished(failed or succeeded)
    public GameObject QuestReturn; // The UI script we're eventually be using to give quest returns.

	public GameObject questPrefab; // Quest UI prefab to display
	public GameObject QuestDisplay;
	public DropHandler dropHandler;

    private void Awake()
    {
        //timeSystem.TickAdded += AdvanceAllQuests;
        bankedQuests = new List<QuestSheet>();
        activeQuests = new List<QuestSheet>();
        finishedQuests = new List<QuestSheet>();
    }

    private void Start()
    {
        GameObject.Find("TimeSystem").GetComponent<TimeSystem>().TickAdded += AdvanceAllQuests;
        GameObject.Find("TimeSystem").GetComponent<TimeSystem>().TickAdded += CheckForQuests;
    }

    public void CheckForQuests(object source, GameTime gameTime)
    { 
        foreach (QuestSheet quest in bankedQuests)
        {
            //create questUI object and attach it to this questsheet
            Debug.Log("Making UI Element");
            GameObject newQuest = Instantiate(questPrefab); //add position to this later

            //add quest to QuestDisplay canvas
            newQuest.transform.parent = QuestDisplay.transform;
            newQuest.transform.localPosition = new Vector3(-100, 100, 0);

            //move to top of child object hierarchy for rendering order reasons
            newQuest.transform.SetAsFirstSibling();

            //pass quest info to quest UI
            QuestUI questUI = newQuest.GetComponent<QuestUI>();
            questUI.SetupQuestUI(quest);

            //add drop points from quest UI to DropHandler
            Transform party = newQuest.transform.Find("Canvas/Party");
            foreach (Transform child in party)
            {
                dropHandler.AddDropPoint(child.gameObject.GetComponent<ObjectDropPoint>());
            }

            Debug.Log("Done making UI Element");
        }
    }

    /// <summary>
    /// Used to advance all the quests.
    /// </summary>
    public void AdvanceAllQuests(object source, GameTime gameTime)
    {
        List<QuestSheet> markForDeletion = new List<QuestSheet>();

        Debug.Log(activeQuests.Count);
        foreach (QuestSheet quest in activeQuests)
        {
            quest.advancebyTick();
            if (quest.QuestComplete == true)
            {
                markForDeletion.Add(quest);

                // Something about sending QuestReturn the correct amount of gold. quest.accumutatedGold;
                QuestReturn.GetComponent<QuestReturnUI>().GenerateQuestReturnBox(quest);
            }
        }

        foreach (QuestSheet quest in markForDeletion)
        {
            activeQuests.Remove(quest);
            finishedQuests.Add(quest);
        }
	}

	/// <summary>
	/// Tells QuestManager to start a quest, given the QuestSheet.
	/// </summary>
	/// <param name="questToBeMoved">The Quest to start</param>
	/// <returns>True if successful, False otherwise.</returns>
	public bool StartQuest(QuestSheet questToBeMoved)
	{
		bankedQuests.Remove(questToBeMoved);
		activeQuests.Add(questToBeMoved);
		return true;
	}
}