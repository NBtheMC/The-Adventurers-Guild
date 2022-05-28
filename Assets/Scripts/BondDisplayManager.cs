using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BondDisplayManager : MonoBehaviour
{
    public CharacterSheet bonder; //the character who has all the bonds
    public IDictionary<Adventurer, BondDisplay> allBonds;
    private int numBonds;

    // Start is called before the first frame update
    void Start()
    {
        allBonds = new Dictionary<Adventurer, BondDisplay>();
        numBonds = 0;
        //get whatever character this is linked to
        bonder = transform.parent.GetComponent<CharacterInfoUI>().charSheet;
        Debug.Log("Bonder name: " + bonder.name);
        //set up all Bonds and link them with display objects
        //loop through current adventurers
        IReadOnlyCollection<CharacterSheet> activeAdventurers = GameObject.Find("CharacterSheetManager").GetComponent<CharacterSheetManager>().HiredAdventurers; 
        //Debug.Log("Active Adventurers: " + activeAdventurers);
        //foreach(KeyValuePair<Adventurer,int> friendship in bonder.adventurer.friendships){
        foreach(CharacterSheet c in activeAdventurers){
            if (c != bonder)
            {
                Debug.Log("CharacterSheet: " + c.adventurer);
                //Debug.Log("Character adding to sheet: " + friendship.Key + " = " + friendship.Value);
                //set gameobject of bond display active
                GameObject currentBondObject = this.transform.GetChild(numBonds).gameObject;
                currentBondObject.SetActive(true);
                BondDisplay currentBondDisplay = currentBondObject.GetComponent<BondDisplay>();
                Debug.Log("BondDisplay: " + currentBondDisplay);
                //link it to adventurer in friendship and add to dictionary
                allBonds.Add(c.adventurer, currentBondDisplay);
                //set bar friendship (not necessary rn but may change)
                Debug.Log("Bondee name: " + c.name);
                currentBondDisplay.InitialSet(c.adventurer, bonder.adventurer.GetFriendship(c.adventurer));
                numBonds++;
            }  
        }
    }

    //adding new adventurer to bondlist
    public void AddBond(Adventurer bondee)
    {

    }

    public void UpdateBond(Adventurer bondee)
    {
        //Find Bond display that correlates and change it
        BondDisplay bondDisplay = allBonds[bondee];
        bondDisplay.SetBond(bonder.adventurer.friendships[bondee]);
    }
}