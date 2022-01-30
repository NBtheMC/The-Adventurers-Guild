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
    }

    /// <summary>
    /// Used to advance all the quests.
    /// </summary>
    public void AdvanceAllQuests(object source, GameTime gameTime)
	{
		List<QuestSheet> markForDeletion = new List<QuestSheet>();

		foreach(QuestSheet quest in activeQuests)
		{
			quest.advancebyTick();
			if (quest.QuestComplete == true)
			{
				markForDeletion.Add(quest);

				// Something about sending QuestReturn the correct amount of gold. quest.accumutatedGold;
				QuestReturn.GetComponent<QuestReturnUI>().GenerateQuestReturnBox(quest);
			}
		}
		
		foreach(QuestSheet quest in markForDeletion)
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