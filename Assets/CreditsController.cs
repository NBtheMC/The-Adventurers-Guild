using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsController : MonoBehaviour
{
    public List<GameObject> creditsText;
    public List<GameObject> creditsPortrait;

    public Transform textDropzone;
    public Transform portraitDropzone;

    public float speed; //speed of objects as they move across the screen
    public float cooldown; //time between each credit

    private int currentCredit;
    private int totalCredits;

    // Start is called before the first frame update
    void Start()
    {
        currentCredit = 1;
        totalCredits = 12; //change later?
    }

    // Update is called once per frame
    void Update()
    {
        cooldown -= Time.deltaTime;
        if (cooldown <= 0 && (currentCredit <= totalCredits)){
            //deploy objects
            Instantiate(creditsText[currentCredit - 1], textDropzone);
            Instantiate(creditsPortrait[currentCredit - 1], portraitDropzone);
        }
        else if(currentCredit > totalCredits || Input.GetKeyDown("escape")){
            EndCredits();
        }
    }

    //fadeout and exit credits
    void EndCredits(){

    }
}
