using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVToQuests : MonoBehaviour
{
    public TextAsset csvStorylets;
    public TextAsset csvEvents;


    // Start is called before the first frame update
    void Start()
    {
        MakeEvents();
        MakeStorylets();
    }

    //Pull data from events csv and makes 1 event per grouping
    //New event denoted by EventDescription row
    //description, stat, dc, time, reward
    //success string, successintchanges, successvaluechanges, successstatechanges
    //failure string, 
    public void MakeEvents(){

        EventNode newEvent = ScriptableObject.CreateInstance<EventNode>();

    }

    //Pull data from storylets csv and makes 1 event per grouping
    //New event denoted by Storylet name row
    public void MakeStorylets(){
        Storylet newStorylet = ScriptableObject.CreateInstance<Storylet>();
    }

}
