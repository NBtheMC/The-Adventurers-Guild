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
        int currentChild = 0;

        //set up all Bonds
        foreach(var friendship in bonder.adventurer.friendships){
            //get gameobject of bond display
            BondDisplay currentBondObject = this.transform.GetChild(currentChild).gameObject.GetComponent<BondDisplay>();
            //link it to adventurer in friendship and add to dictionary
            allBonds.Add(friendship.Key, currentBondObject);
            //set bar friendship (not necessary rn but may change)
            currentBondObject.SetBond(friendship.Value);
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