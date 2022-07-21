using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StoryletTesting;

public class RecapManager : MonoBehaviour
{
    private TimeSystem timeSystem;
    private PauseMenu pauseMenu;
    private GameObject recapObject;
    private MusicManager musicManager;
    private ItemDisplayManager itemDisplayManager;

    public DebriefReport debrief;
    public DebriefTracker debriefTracker;

    public Text earnedText;
    public Text lostText;
    public Text totalText;
    private int goldEarned;
    private int goldLost;
    //private WorldIntChanger totalGold;
    private int TotalGold { get { return GameObject.Find("GuildManager").GetComponent<GuildManager>().Gold; } }

    // Start is called before the first frame update
    void Start()
    {
        timeSystem = GameObject.Find("TimeSystem").GetComponent<TimeSystem>();
        //timeSystem.EndOfDay += StartRecap;
        pauseMenu = GameObject.Find("PauseMenu").GetComponent<PauseMenu>();
        //totalGold = GameObject.Find("Gold").GetComponent<WorldIntChanger>();
        musicManager = GameObject.Find("MusicManager").GetComponent<MusicManager>();
        itemDisplayManager = GameObject.Find("CurrentItemDisplay").GetComponent<ItemDisplayManager>();
        // earnedText = transform.Find("EarnedNumber").GetComponent<Text>();
        // lostText = transform.Find("LostNumber").GetComponent<Text>();
        // totalText = transform.Find("TotalNumber").GetComponent<Text>();
    }

    //Go into recap mode
    public void StartRecap(){
        Debug.Log("Starting Recap");
        UpdateRecapScreen();
        /*foreach(Transform child in transform){
            child.gameObject.SetActive(true);
        }*/
        debrief.TriggerEndOfDay();

        musicManager.StopMusic();
        itemDisplayManager.ClearDisplay();
        pauseMenu.Pause();
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
        pauseMenu.Play();
    }

    public void AddDayGold(int goldAdded){
        if (goldAdded > 0)
            goldEarned += goldAdded;
        else
            goldLost += goldAdded;
    }

    public void UpdateRecapScreen(){
        //update with gold earned, lost, and total ANIMATE LATER
        earnedText.text = goldEarned.ToString();
        lostText.text = goldLost.ToString(); //CHANGE
        totalText.text = TotalGold.ToString();
    }
}
