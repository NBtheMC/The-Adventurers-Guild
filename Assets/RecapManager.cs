using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecapManager : MonoBehaviour
{
    private TimeSystem timeSystem;
    // Start is called before the first frame update
    void Start()
    {
        timeSystem = GameObject.Find("TimeSystem").GetComponent<TimeSystem>();
        timeSystem.TickAdded += CheckForRecap;
    }

    //listens to tick tracker. when its 24
    void CheckForRecap(object source, GameTime gameTime){
        if(gameTime.hour == timeSystem.activeHours) StartRecap();
    }

    //Go into recap mode
    public void StartRecap(){
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
        timeSystem.SetDay(timeSystem.getTime().day + 1);
        timeSystem.StartTimer();
    }
}
