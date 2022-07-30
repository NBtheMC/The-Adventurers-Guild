using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Character")]
public class CharacterInitialStats : ScriptableObject
{
    public string characterName;
    public bool hiredAtStart;
    public int combat, exploration, charisma;
    public Sprite portrait;

    public string Biography;
}
