using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles all processing of social models
public class Adventurer : MonoBehaviour
{
    //Relationships with others combined with a strength
    //if another adventurer isn't in this list, their relationship is neutral
    Dictionary<Adventurer, int> friendships;
    Dictionary<Adventurer, int> enemyships;
    //Main relationships (give bonuses later?)
    Adventurer bestFriend;
    Adventurer worstEnemy;
    //Selecting/dragging
    bool isSelected;


    // Start is called before the first frame update
    void Start()
    {
        friendships = new Dictionary<Adventurer, int>();
        enemyships = new Dictionary<Adventurer, int>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool IsFriendsWith(){
        return false;
    }

    public bool IsEnemiesWith(){
        return false;
    }
}