using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BondDisplay : MonoBehaviour
{
    public CharacterSheet character;
    //public int currentBond;
    public Image greenBar;
    public Image redBar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // //called after BondDisplayManager figures stuff out
    // void InitialSet(){
    //     //assign portrait
    //     //GameObject.Find("Portrait").GetComponent<Image>().sprite = character.portrait;
    //     //set visuals both to 0
    //     SetBond(0);
    // }

    public void SetBond(int bond){
        float percentage = bond/10f; //should be from 0-1
        //length is equal to initial size times the percentage
        if(bond < 0){
            //set green bar to 0
            greenBar.fillAmount = 0;
            redBar.fillAmount = -percentage;
        }
        else{
            //set red bar to 0
            redBar.fillAmount = 0;
            greenBar.fillAmount = percentage;
        }
    }
}