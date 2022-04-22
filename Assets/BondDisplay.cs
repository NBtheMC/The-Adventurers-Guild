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

    // Start is called before the first frame update
    void Start()
    {
        //assign portrait
        GameObject.Find("Portrait").GetComponent<Image>().sprite = character.portrait;
        //set visuals both to 0
        greenBar.rect.Set(greenBar.rect.x, greenBar.rect.y, 0, greenBar.rect.height);
        redBar.rect.Set(redBar.rect.x, redBar.rect.y, 0, redBar.rect.height);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}