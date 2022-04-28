using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BondDisplayManager : MonoBehaviour
{
    public CharacterSheet bonder; //the character who has all the bonds
    public IDictionary<Adventurer, BondDisplay> allBonds;

    // Start is called before the first frame update
    void Start()
    {
        //get whatever character this is linked to
        bonder = transform.parent.GetComponent<CharacterInfoUI>().charSheet;
        Debug.Log("Bonder name: " + bonder.name);
        //set up all Bonds and link them with display objects
        Debug.Log("Friendships = " + bonder.adventurer.friendships);
        int currentChild = 0;
        foreach(var friendship in bonder.adventurer.friendships){
            Debug.Log("Bond with: " + friendship.Key + " = " + friendship.Value);
            //get gameobject of bond display
            BondDisplay currentBondDisplay = this.transform.GetChild(currentChild).gameObject.GetComponent<BondDisplay>();
            //link it to adventurer in friendship and add to dictionary
            allBonds.Add(friendship.Key, currentBondDisplay);
            //set bar friendship (not necessary rn but may change)
            currentBondDisplay.InitialSet(friendship.Key, friendship.Value);
            currentChild++;
        }
    }

    public void UpdateBond(Adventurer bondee)
    {
        //Find Bond display that correlates and change it
        BondDisplay bondDisplay = allBonds[bondee];
        bondDisplay.SetBond(bonder.adventurer.friendships[bondee]);
    }
}