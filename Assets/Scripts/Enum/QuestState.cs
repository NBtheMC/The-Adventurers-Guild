/// <summary>
/// Represents the current state of a quest sheet, both data wise and display wise
/// WAITING : Questsheet is waiting on either a party assignment, rejection, or timing out
/// ADVENTURING : Questsheet has been assigned a party and is advancing the quest
/// COMPLETED : Quest was accepted and completed
/// REJECTED : Quest was rejected, either tiomed out or reject button pressed
/// </summary>
public enum QuestState
{
    WAITING, ADVENTURING, COMPLETED, REJECTED
}
