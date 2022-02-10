using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles all processing of social models
public class Adventurer : MonoBehaviour
{
    public CharacterSheet characterSheet; //an adventurer's associated charactersheet
    //Relationships with others combined with a strength
    //if another adventurer isn't in this list, their relationship is neutral
    public IDictionary<Adventurer, int> friendships; //number between -10 and 10
    public IDictionary<Adventurer, int> romances; //not used
    IDictionary<Adventurer, int> wereFriends; //not used
    IDictionary<Adventurer, int> wereEnemies; //not used
    //Main relationships (give bonuses later?)
    Adventurer bestFriend;
    Adventurer worstEnemy;
    //Selecting/dragging
    bool isSelected;

    // Start is called before the first frame update
    void Awake()
    {
        friendships = new Dictionary<Adventurer, int>();
        romances = new Dictionary<Adventurer, int>();
    }

    public void ChangeFriendship(Adventurer a, int friendshipChange){
        //adding new friendship
        Debug.Log("Friendships: " + friendships);
        if(friendships.Count == 0){
            friendships.Add(a, friendshipChange);
            return;
        }
        else if (friendships.ContainsKey(a) == false){
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
        if(friendships.ContainsKey(a)){
            if(friendships[a] > 0){
                return true;
            }
        }
        return false;
    }

    //takes in other adventurer and gets romance level with it
    //returns true if romanced
    public bool IsRomancedWith(Adventurer a){
        if(romances.ContainsKey(a)){
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
            friendships.Add(a, friendshipLevel);
            return;
        }
        friendships[a] = friendshipLevel;
    }

    public int GetRomance(Adventurer a){
        if(romances.ContainsKey(a) == false){
            return 0;
        }
        return romances[a];
    }
}