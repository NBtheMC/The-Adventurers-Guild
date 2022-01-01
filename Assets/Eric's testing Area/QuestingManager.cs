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
	public GameObject QuestReturn; // The UI script we're eventually be using to give quest returns.
	
    private void Awake()
	{
        //timeSystem.TickAdded += AdvanceAllQuests;
		bankedQuests = new List<QuestSheet>();
		activeQuests = new List<QuestSheet>();
	}

	/// <summary>
	/// Used to advance all the quests.
	/// </summary>
	public void AdvanceAllQuests(object source, GameTime gameTime)
	{
		foreach(QuestSheet quest in activeQuests)
		{
			quest.advancebyTick();
			if (quest.QuestComplete == true)
			{
				// Something about sending QuestReturn the correct amount of gold. quest.accumutatedGold;
			}
		}
	}

	/// <summary>
	/// Tells QuestManager to start a quest, given the QuestSheet.
	/// </summary>
	/// <param name="questToBeMoved">The Quest to start</param>
	/// <returns>True if successful, False otherwise.</returns>
	public bool StartQuest(QuestSheet questToBeMoved)
	{
		if (questToBeMoved != null) { return false; }
		bankedQuests.Remove(questToBeMoved);
		activeQuests.Add(questToBeMoved);
		return true;
	}
}