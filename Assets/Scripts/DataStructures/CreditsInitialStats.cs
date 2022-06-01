using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Character")]
public class CreditsInitialStats : ScriptableObject
{
    public string characterName;
    public int combat, exploration, negotiation, constitution;
    public Sprite portrait;

    public string Biography;
}
