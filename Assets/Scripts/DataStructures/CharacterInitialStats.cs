using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Character")]
public class CharacterInitialStats : ScriptableObject
{
    public string characterName;

    public int combat, exploration, diplomacy, stamina;
}
