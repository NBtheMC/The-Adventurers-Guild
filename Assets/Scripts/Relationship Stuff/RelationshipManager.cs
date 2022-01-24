using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles all processing of social models
public class RelationshipManager : MonoBehaviour
{
    // REQUIREMENTS--------------------------------
    // Predicates: Sets of variables that will bind to characters during evaluation. eg (Friend ?x ?y)
    // Rules: Made up of predicates to make adjustments. eg (Friend ?x ?y) !(Friend ?y ?z) => (Friend ?y ?x)

    public Transform adventurers; //parent of all adventurers in the scene. Named the same

    //called first by quest when quest is done. updates friendships based on win or loss
    //done on current party, change is determined by quest
    public void UpdatePartyRelationships(Transform party, int change){
        List<Adventurer> partyMembers = new List<Adventurer>(); //should be changed to whatever object holds the party
        foreach(Transform a in party){
            partyMembers.Add(a.GetComponent<Adventurer>());
        }
        for(int i  = 0; i < partyMembers.Count; i++){
            Adventurer a = partyMembers[i];
            for(int j  = i+1; j < partyMembers.Count; j++){
                Adventurer b = partyMembers[j];
                //update friendship between a and b
                a.ChangeFriendship(b, change);
                b.ChangeFriendship(a, change); //do if we want to handle relationships pretty much completely here
            }
        }
        RecalculateAllRelationships();
    }

    //called after relationships are updated
    //loops all rules through all people 's volitions, relationships, etc, in order to
    public void RecalculateAllRelationships(){
        //loop through all Adventurers in entire scene
        List<Adventurer> allAdventurers = new List<Adventurer>();
        foreach(Transform a in adventurers){
            allAdventurers.Add(a.GetComponent<Adventurer>());
        }
        for(int i  = 0; i < allAdventurers.Count - 2; i++){
            Adventurer a = allAdventurers[i];
            for(int j  = i+1; j < allAdventurers.Count - 1; j++){
                Adventurer b = allAdventurers[j];
                for(int k = j+1; k < allAdventurers.Count; k++){
                    Adventurer c = allAdventurers[k];
                    Debug.Log(a.gameObject.name + " + " + b.gameObject.name + " friends= " + a.IsFriendsWith(b));
                    Debug.Log(a.gameObject.name + " + " + c.gameObject.name + " friends= " + a.IsFriendsWith(c));      
                    Debug.Log(b.gameObject.name + " + " + c.gameObject.name + " friends= " + b.IsFriendsWith(c));              
                    //Take friendship levels into account when recalculating eventually
                    //eg int abFriendship = a.GetFriendship(b);

                    //Each friendship so easier to access
                    bool abFriends = a.IsFriendsWith(b);
                    bool acFriends = a.IsFriendsWith(c);
                    bool bcFriends = b.IsFriendsWith(c);

                    //DO ALL RULES HERE
                    //HAVE TO ACCOUNT FOR ALL COMBINATIONS
                    //Friend of friend is my friend
                    //(Friend ?x ?y) !(Friend ?y ?z) => Friendship(?x ?z) +=1
                    if(abFriends && bcFriends){
                        Debug.Log("Friend of a friend");
                        a.ChangeFriendship(c, 1);
                        c.SetFriendship(a, a.GetFriendship(c));
                    }
                    if(acFriends && bcFriends){
                        Debug.Log("Friend of a friend");
                        a.ChangeFriendship(b, 1);
                        b.SetFriendship(a, a.GetFriendship(b));
                    }
                    if(abFriends && acFriends){
                        Debug.Log("Friend of a friend");
                        c.ChangeFriendship(b, 1);
                        b.SetFriendship(c, c.GetFriendship(b));
                    }

                    //Enemy of enemy is my friend
                    //(Enemies ?x ?y) !(Enemies ?y ?z) => Friendship(?x ?z) +=1
                    if(!abFriends && !bcFriends){
                        Debug.Log("Enemy of an enemy is a friend");
                        a.ChangeFriendship(c, -1);
                        c.SetFriendship(a, a.GetFriendship(c));
                    }
                    if(!acFriends && !bcFriends){
                        Debug.Log("Friend of a friend");
                        a.ChangeFriendship(b, -1);
                        b.SetFriendship(a, a.GetFriendship(b));
                    }
                    if(!abFriends && !acFriends){
                        Debug.Log("Friend of a friend");
                        c.ChangeFriendship(b, -1);
                        b.SetFriendship(c, c.GetFriendship(b));
                    }
                }
            }
        }
    }

}
