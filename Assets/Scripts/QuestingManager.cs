using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps track of active quests and advances their ticks.
/// </summary>
public class QuestingManager : MonoBehaviour
{
    public QuestingManager Instance;

    public TimeSystem timeSystem; // a reference to the time system to update quest with.
    public List<QuestSheet> activeQuests; // All quests currently embarked.
    public List<QuestSheet> bankedQuests; // All quests waiting to be embarked.
    public List<List<QuestSheet>> finishedQuests; //All quests that have been finished(failed or succeeded)

    public event EventHandler<QuestSheet> QuestStarted;
    public event EventHandler<QuestSheet> QuestFinished;
    public event EventHandler<QuestSheet> QuestAdded;

    public RelationshipManager relationshipManager;

    private void Awake()
    {
        //timeSystem.TickAdded += AdvanceAllQuests;
        bankedQuests = new List<QuestSheet>();
        activeQuests = new List<QuestSheet>();
        finishedQuests = new List<List<QuestSheet>>();
        finishedQuests.Add(new List<QuestSheet>());

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        timeSystem.TickAdded += AdvanceAllQuests;
        timeSystem.NewDay += AddFinishedQuestPage;
    }

    /// <summary>
    /// Adds new page to Finished Quest log
    /// </summary>
    private void AddFinishedQuestPage(object source, GameTime gameTime)
    {
        finishedQuests.Add(new List<QuestSheet>());
    }

    /// <summary>
    /// Used to advance all the quests.
    /// </summary>
    public void AdvanceAllQuests(object source, GameTime gameTime)
    {
        List<QuestSheet> markForDeletion = new List<QuestSheet>();

        foreach (QuestSheet quest in bankedQuests)
        {
            if (quest.advancebyTick() == 2)
            {
                quest.currentState = QuestState.REJECTED;
                // Add quest to a list for deletion (Move to archive really)
                markForDeletion.Add(quest);
            }
        }

        //Debug.Log(activeQuests.Count);
        foreach (QuestSheet quest in activeQuests)
        {
            if (quest.advancebyTick() == 1)
            {
                quest.AddGuildGold();
                quest.currentState = QuestState.COMPLETED;
                // Add quest to a list for deletion (Move to archive really)
                markForDeletion.Add(quest);
            }
        }

        foreach (QuestSheet quest in markForDeletion)
        {
            activeQuests.Remove(quest);
            int currentDay = timeSystem.getTime().day;
            finishedQuests[currentDay].Add(quest);
            QuestFinished(this, quest);
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
        questToBeMoved.currentState = QuestState.ADVENTURING;
		return true;
	}

    public void AddQuest(QuestSheet quest)
    {
        bankedQuests.Add(quest);
        QuestAdded(this, quest);
    }
}