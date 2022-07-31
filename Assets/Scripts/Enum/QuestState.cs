/// <summary>
/// Represents the current state of a quest sheet, both data wise and display wise
/// WAITING : Questsheet is waiting on either a party assignment, rejection, or timing out
/// ADVENTURING : Questsheet has been assigned a party and is advancing the quest
/// DONE : Quest is completed, either rejected or timed out during Waiting, or completed after Adventuring
/// </summary>
public enum QuestState
{
    WAITING, ADVENTURING, DONE
}
