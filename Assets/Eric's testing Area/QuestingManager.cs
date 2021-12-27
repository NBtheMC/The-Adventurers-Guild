using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps track of active quests and advances their ticks.
/// </summary>
public class QuestingManager : MonoBehaviour
{
    public TimeSystem timeSystem;
	public List<QuestSheet> activeQuests;
	
    private void Awake()
	{
        timeSystem.TickAdded += AdvanceAllQuests;
	}

	public void AdvanceAllQuests(object source, GameTime gameTime)
	{
		foreach(QuestSheet quest in activeQuests)
		{
			quest.advancebyTick();
		}
	}
}