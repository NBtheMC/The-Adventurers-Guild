using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BondDisplay : MonoBehaviour
{
    public CharacterSheet character;
    public int currentBond;
    public RectTransform greenBar;
    public RectTransform redBar;
    public float initialSize;

    // Start is called before the first frame update
    void Start()
    {
        //assign portrait
        GameObject.Find("Portrait").GetComponent<Image>().sprite = character.portrait;
        //set visuals both to 0
        initialSize = greenBar.rect.width;
        greenBar.rect.Set(greenBar.rect.x, greenBar.rect.y, 0, greenBar.rect.height);
        redBar.rect.Set(redBar.rect.x, redBar.rect.y, 0, redBar.rect.height);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetBond(int bond){
        float percentage = bond/10f; //should be from 0-1
        //length is equal to initial size times the percentage
        if(bond < 0){
            greenBar.rect.Set(greenBar.rect.x, greenBar.rect.y, 0, greenBar.rect.height);
            redBar.rect.Set(redBar.rect.x, redBar.rect.y, bond*percentage, redBar.rect.height);
        }
        else{
            redBar.rect.Set(redBar.rect.x, redBar.rect.y, 0, redBar.rect.height);
            greenBar.rect.Set(greenBar.rect.x, greenBar.rect.y, bond*percentage, greenBar.rect.height);
        }
    }
}