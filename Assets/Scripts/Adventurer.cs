using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles all processing of social models
public class Adventurer : MonoBehaviour
{
    //Relationships with others combined with a strength
    //if another adventurer isn't in this list, their relationship is neutral
    Dictionary<Adventurer, int> friendships; //number between -10 and 10
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
    // void Update()
    // {

    // }

    public void ChangeFriendship(Adventurer a, int friendshipChange){
        //adding new friendship
        if(friendships.ContainsKey(a) == false){
            friendships.Add(a, friendshipChange);
            return;
        }
        //changing existing friendship
        friendships[a] = friendships[a] += friendshipChange;
    }

    //dont worry about for now 
    public void ChangeRomances(Adventurer a, int romanceChange){
        romances[a] = romances[a] += romanceChange;
    }

    //takes in other adventurer and gets friendship level with it
    //returns true if friends and false if enemies
    public bool IsFriendsWith(Adventurer a){
        if(friendships[a]){
            if(friendships[a] > 0){
                return true;
            }
        }
        return false;
    }

    //takes in other adventurer and gets romance level with it
    //returns true if romanced
    public bool IsRomancedWith(Adventurer a){
        if(romances[a]){
            if(romances[a] > 0){ //change this number to something higher
                return true;
            }
        }
        return false;
    }

    public int GetFriendship(Adventurer a){
        if(friendships.ContainsKey(a) == false){
            return 0;
        }
        return friendships[a];
    }

    //probably wont have to use. just in case though
    public void SetFriendship(Adventurer a, int friendshipLevel){
        if(friendships.ContainsKey(a) == false){
            return;
        }
        friendships[a] = friendshipLevel;
    }

    public int GetRomance(Adventurer a){
        return 0;
    }


}