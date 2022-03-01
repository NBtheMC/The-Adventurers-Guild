using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Character")]
public class CharacterInitialStats : ScriptableObject
{
    public string characterName;
    public bool hiredAtStart;
    public int combat, exploration, negotiation, constitution;
    public Sprite portrait;
}
