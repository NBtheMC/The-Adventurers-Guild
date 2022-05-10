using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecapManager : MonoBehaviour
{
    private TimeSystem timeSystem;
    private GameObject recapObject;

    public Text earnedText;
    public Text lostText;
    public Text totalText;
    private int goldEarned;
    private int goldLost;
    private int goldTotal;

    // Start is called before the first frame update
    void Start()
    {
        timeSystem = GameObject.Find("TimeSystem").GetComponent<TimeSystem>();
        timeSystem.TickAdded += CheckForRecap;
        // earnedText = transform.Find("EarnedNumber").GetComponent<Text>();
        // lostText = transform.Find("LostNumber").GetComponent<Text>();
        // totalText = transform.Find("TotalNumber").GetComponent<Text>();
    }

    //listens to tick tracker. when its 24
    void CheckForRecap(object source, GameTime gameTime){
        if(gameTime.hour == timeSystem.activeHours) StartRecap();
    }

    //Go into recap mode
    public void StartRecap(){
        UpdateRecapScreen();
        foreach(Transform child in transform){
            child.gameObject.SetActive(true);
        }
        timeSystem.StopTimer();
    }

    //Get out of recap mode
    public void EndRecap(){
        foreach(Transform child in transform){
            child.gameObject.SetActive(false);
        }
        goldEarned = 0;
        goldLost = 0;
        //set day and clock correct ANIMATE LATER?
        timeSystem.SetDay(timeSystem.getTime().day + 1);
        timeSystem.StartTimer();
    }

    public void AddDayGold(int goldAdded){
        goldEarned += goldAdded;
    }

    public void UpdateRecapScreen(){
        //update with gold earned, lost, and total ANIMATE LATER
        earnedText.text = goldEarned.ToString();
        lostText.text = goldLost.ToString();
        //totalText = worldstate gold total
        //other recap GONNA BE HONEST I DONT KNOW WHAT THIS IS
    }
}
