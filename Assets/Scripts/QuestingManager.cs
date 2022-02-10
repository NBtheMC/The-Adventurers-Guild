using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Keeps track of active quests and advances their ticks.
/// </summary>
public class QuestingManager : MonoBehaviour
{
    public TimeSystem timeSystem; // a reference to the time system to update quest with.
    public List<QuestSheet> activeQuests; // All quests currently embarked.
    public List<QuestSheet> bankedQuests; // All quests waiting to be embarked.
    public List<QuestSheet> finishedQuests; //All quests that have been finished(failed or succeeded)

    public WorldStateManager stateManager; // Used to assign quests for world updates.

    public GameObject questListContent; //Gameobject that holds the list of banked quests

    public event EventHandler<QuestSheet> QuestStarted;
    public event EventHandler<QuestSheet> QuestFinished;
    public event EventHandler<QuestSheet> QuestAdded;

    private void Awake()
    {
        bankedQuests = new List<QuestSheet>();
        activeQuests = new List<QuestSheet>();
        finishedQuests = new List<QuestSheet>();
    }

    private void Start()
    {
        GameObject.Find("TimeSystem").GetComponent<TimeSystem>().TickAdded += AdvanceAllQuests;
    }

    private void AdvanceAllQuests(object source, GameTime gameTime)
    {
        List<QuestSheet> markForDeletion = new List<QuestSheet>();

        Debug.Log(activeQuests.Count);
        foreach (QuestSheet quest in activeQuests)
        {
            quest.advancebyTick();
            if (quest.QuestComplete == true)
            {
                markForDeletion.Add(quest);
                QuestFinished(this, quest);
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
        QuestStarted(this, questToBeMoved);
		return true;
	}

    /// <summary>
    /// Adds a quest to the banked quests list
    /// </summary>
    /// <param name="quest">The quest to add</param>
    public void AddQuest(QuestSheet quest)
    {
        bankedQuests.Add(quest);
        quest.worldStateManager = stateManager; 
        QuestAdded(this, quest);
    }
}