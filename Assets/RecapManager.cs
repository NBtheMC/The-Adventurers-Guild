using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StoryletTesting;

public class RecapManager : MonoBehaviour
{
    private TimeSystem timeSystem;
    private GameObject recapObject;
    private MusicManager musicManager;

    public DebriefReport debrief;
    public DebriefTracker debriefTracker;

    public Text earnedText;
    public Text lostText;
    public Text totalText;
    private int goldEarned;
    private int goldLost;
    private WorldIntChanger totalGold;

    // Start is called before the first frame update
    void Start()
    {
        timeSystem = GameObject.Find("TimeSystem").GetComponent<TimeSystem>();
        timeSystem.EndOfDay += StartRecap;
        totalGold = GameObject.Find("Gold").GetComponent<WorldIntChanger>();
        musicManager = GameObject.Find("MusicManager").GetComponent<MusicManager>();
        // earnedText = transform.Find("EarnedNumber").GetComponent<Text>();
        // lostText = transform.Find("LostNumber").GetComponent<Text>();
        // totalText = transform.Find("TotalNumber").GetComponent<Text>();
    }

    //Go into recap mode
    public void StartRecap(object sender, GameTime gameTime){
        Debug.Log("Starting Recap");
        UpdateRecapScreen();
        /*foreach(Transform child in transform){
            child.gameObject.SetActive(true);
        }*/
        debrief.TriggerEndOfDay();

        musicManager.StopMusic();
    }

    //Get out of recap mode
    public void EndRecap(){
        Debug.Log("Ending Recap");
        foreach(Transform child in transform){
            child.gameObject.SetActive(false);
        }
        goldEarned = 0;
        goldLost = 0;
        //set day and clock correct ANIMATE LATER?
        timeSystem.StartNewDay();
        musicManager.PlayMusic();
    }

    public void AddDayGold(int goldAdded){
        goldEarned += goldAdded;
    }

    public void UpdateRecapScreen(){
        //update with gold earned, lost, and total ANIMATE LATER
        earnedText.text = goldEarned.ToString();
        lostText.text = goldLost.ToString();
        totalText.text = totalGold.value.ToString();

        //string debriefLog = "Gold Earned: " + goldEarned.ToString() + "\nGold Lost: " + goldLost.ToString() + "\nTotal Gold: " + totalGold.value.ToString();
        //debriefTracker.submitLog(debriefLog);
        //other recap GONNA BE HONEST I DONT KNOW WHAT THIS IS
    }
}
