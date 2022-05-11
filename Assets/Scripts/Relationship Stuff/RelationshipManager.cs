using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles all processing of social models
public class RelationshipManager : MonoBehaviour
{
    // REQUIREMENTS--------------------------------
    // Predicates: Sets of variables that will bind to characters during evaluation. eg (Friend ?x ?y)
    // Rules: Made up of predicates to make adjustments. eg (Friend ?x ?y) !(Friend ?y ?z) => (Friend ?y ?x)

    public CharacterSheetManager characterSheetManager; //parent of all adventurers in the scene. Named the same
    public GameObject relationshipPopup; //the prefab that will be modified with a string each time
    public const float POPUPTIMER = 5f;
    private float currentTimer;

    //all the relevant info that occured with relationships in a given update
    // public struct RelationshipsInfo{
    //     List<(Adventurer, Adventurer, int)> relationshipChanges; //describes how much each relationship changed by
    //     List<string> relationshipStories; //anything notable that happens
    // }

    public void Start(){
        currentTimer = POPUPTIMER;
    }

    public void Update(){
        currentTimer -= Time.deltaTime;
        if(currentTimer <= 0){
            //make popup
            
            currentTimer = POPUPTIMER;
        }
    }

    //called after relationships are updated
    //loops all rules through all people 's volitions, relationships, etc, in order to
    public void RecalculateAllRelationships(){
        //loop through all Adventurers in entire scene
        List<CharacterSheet> allAdventurers = new List<CharacterSheet>();

        allAdventurers.AddRange(characterSheetManager.QuestingAdventurers);
        allAdventurers.AddRange(characterSheetManager.FreeAdventurers);

        for(int i  = 0; i < allAdventurers.Count - 2; i++){
            Adventurer a = allAdventurers[i].adventurer;
            for(int j  = i+1; j < allAdventurers.Count - 1; j++){
                Adventurer b = allAdventurers[j].adventurer;
                for(int k = j+1; k < allAdventurers.Count; k++){
                    Adventurer c = allAdventurers[k].adventurer;
                    //Debug.Log(a.gameObject.name + " + " + b.gameObject.name + " friends= " + a.IsFriendsWith(b));
                    //Debug.Log(a.gameObject.name + " + " + c.gameObject.name + " friends= " + a.IsFriendsWith(c));      
                    //Debug.Log(b.gameObject.name + " + " + c.gameObject.name + " friends= " + b.IsFriendsWith(c));              
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
                        c.ChangeFriendship(a, 1);
                    }
                    if(acFriends && bcFriends){
                        Debug.Log("Friend of a friend");
                        a.ChangeFriendship(b, 1);
                        b.ChangeFriendship(a, 1);
                    }
                    if(abFriends && acFriends){
                        Debug.Log("Friend of a friend");
                        c.ChangeFriendship(b, 1);
                        b.ChangeFriendship(c, 1);
                    }

                    //Enemy of enemy is my friend
                    //(Enemies ?x ?y) !(Enemies ?y ?z) => Friendship(?x ?z) +=1
                    if(!abFriends && !bcFriends){
                        Debug.Log("Enemy of an enemy is a friend");
                        a.ChangeFriendship(c, -1);
                        c.ChangeFriendship(a, -1);
                    }
                    if(!acFriends && !bcFriends){
                        Debug.Log("Friend of a friend");
                        a.ChangeFriendship(b, -1);
                        b.ChangeFriendship(a, -1);
                    }
                    if(!abFriends && !acFriends){
                        Debug.Log("Friend of a friend");
                        c.ChangeFriendship(b, -1);
                        b.ChangeFriendship(c, -1);
                    }
                }
            }
        }
    }

    //takes 2 random adventurers that are in party and puts out blurb about them. excel sheet? idk
    public string RandomRelationshipUpdate(){
        //randomly pick adventurer A
        //randomly pick adventurer B
        //Get relationship between A and B
        //Look in spreadsheet for random line
        return "Adventurers A and B are getting along quite nicely";
    }

}