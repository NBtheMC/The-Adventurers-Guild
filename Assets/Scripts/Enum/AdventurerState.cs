/// <summary>
/// Represents the current state of an adventurer, both data wise and display wise
/// UNHIRED : Has not been hired and is not displayed in the adventurer pool
/// FREE : Has been hired, is not quest, and is not displayed in adventurer pool
/// ASSIGNED : Has been assigned to a quest that hasnt been sent out yet. Portrait is grayed out in adventurer pool
/// QUESTING : Has bene assigned to an active quest. Portrait is blacked out in adventurer pool
/// </summary>
public enum AdventurerState
{
    UNHIRED, FREE, ASSIGNED, QUESTING, DEAD
}
