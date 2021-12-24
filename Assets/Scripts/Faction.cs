using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles all processing of social models
public class Faction : MonoBehaviour
{
    //Relationships with others combined with a strength
    //if another adventurer isn't in this list, their relationship is neutral
    Dictionary<Faction, int> allies;
    Dictionary<Faction, int> enemies;
    Dictionary<Adventurer, int> wereAllies;
    Dictionary<Adventurer, int> wereEnemies;
    //Main relationships (give bonuses later?)
    Faction bestAlly;
    Faction worstEnemy;
    //Selecting/dragging
    bool isSelected;


    // Start is called before the first frame update
    void Start()
    {
        allies = new Dictionary<Faction, int>();
        enemies = new Dictionary<Faction, int>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool IsAlliesWith(){
        return false;
    }

    public bool IsEnemiesWith(){
        return false;
    }
}