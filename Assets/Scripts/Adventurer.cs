using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles all processing of social models
public class Adventurer : MonoBehaviour
{
    //Relationships with others combined with a strength
    //if another adventurer isn't in this list, their relationship is neutral
    Dictionary<Adventurer, int> friendships;
    Dictionary<Adventurer, int> romances;
    Dictionary<Adventurer, int> wereFriends;
    Dictionary<Adventurer, int> wereEnemies;
    //Main relationships (give bonuses later?)
    Adventurer bestFriend;
    Adventurer worstEnemy;
    //Selecting/dragging
    bool isSelected;


    // Start is called before the first frame update
    void Start()
    {
        bonds = new Dictionary<Adventurer, int>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeFriendship(Adventurer a, int friendshipChange){
        friendships[a] = friendships[a] += friendshipChange;
    }

    public bool IsFriendsWith(Adventurer a){
        if(friendships.ContainsKey(a)){
            if(friendships[a] > 0){
                return true;
            }
        }
        return false;
    }

    public bool IsEnemiesWith(Adventurer a){
        if(friendships.ContainsKey(a)){
            if(friendships[a] < 0){
                return true;
            }
        }
        return false;
    }

    public void RecalculateRelationships(){
        for(int i  = 0; i < friendships.Count; i++){
            Adventurer a = friendships[i];
            //friends of friends are more friends
        }
        for(int i  = 0; i < romances.Count; i++){
            Adventurer a = romances[i];
            //friends of romances are more friends
        }
    }
}