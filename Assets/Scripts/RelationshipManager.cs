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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
                    Debug.Log("Adventurer 1: " + a.gameObject.name);
                    Debug.Log("Adventurer 2: " + b.gameObject.name);
                    Debug.Log("Adventurer 3: " + c.gameObject.name);
                    //Take friendship levels into account when recalculating eventually
                    //eg int abFriendship = a.GetFriendship(b);

                    //DO ALL RULES HERE
                    //Friend of friend is my friend
                    //(Friend ?x ?y) !(Friend ?y ?z) => Friendship(?x ?z) +=1
                    if(a.IsFriendsWith(b) && b.IsFriendsWith(c)){
                        Debug.Log("Friend of a friend");
                        a.ChangeFriendship(c, 1);
                        c.SetFriendship(a, a.GetFriendship(c));
                    }
                    //Enemy of enemy is my friend
                    //(Enemies ?x ?y) !(Enemies ?y ?z) => Friendship(?x ?z) +=1
                    if(!a.IsFriendsWith(b) && !b.IsFriendsWith(c)){
                        Debug.Log("Enemy of a Enemy");
                        a.ChangeFriendship(c, 1);
                        c.SetFriendship(a, a.GetFriendship(c));
                    }
                }
            }
        }
    }

}
