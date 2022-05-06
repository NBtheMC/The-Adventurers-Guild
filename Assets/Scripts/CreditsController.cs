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
    public const float MAXCOOLDOWN = 5; //time between each credit
    private float cooldown; 

    private int currentCredit;
    private int totalCredits;

    // Start is called before the first frame update
    void Start()
    {
        currentCredit = 1;
        totalCredits = 12; //change later?

        cooldown = MAXCOOLDOWN;
    }

    // Update is called once per frame
    void Update()
    {
        cooldown -= Time.deltaTime;
        //Debug.Log("Current cooldown: " + cooldown);
        if (cooldown <= 0 && (currentCredit <= totalCredits)){
            Debug.Log("Max cooldown: " + MAXCOOLDOWN);
            cooldown = MAXCOOLDOWN;
            //Debug.Log("Dropping theoretically");
            //deploy objects
            GameObject newPortrait = Instantiate(creditsPortrait[currentCredit - 1], portraitDropzone);
            newPortrait.GetComponent<CreditsPortrait>().SetSpeed(speed);
            GameObject newCredit = Instantiate(creditsText[currentCredit - 1], textDropzone);
            newCredit.GetComponent<CreditsText>().SetSpeed(speed);
        }
        if(currentCredit > totalCredits || Input.GetKeyDown("escape")){
            EndCredits();
        }
    }

    //fadeout and exit credits
    void EndCredits(){

    }
}
