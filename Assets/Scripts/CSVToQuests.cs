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
        csvStorylets = Resources.Load<TextAsset>("Storylets");
        csvEvents = Resources.Load<TextAsset>("Events");
        MakeStorylets();
        MakeEvents();
    }

    //Pull data from storylets csv and makes 1 event per grouping
    //New event denoted by Storylet name row
    public void MakeStorylets(){
        string[] eventData = csvEvents.text.Split(new char[] {'\n'});
        for(int i = 0; i < eventData.Length; i += 9){ //9 is however many properties there are
            
            Storylet newStorylet = ScriptableObject.CreateInstance<Storylet>(); //this also needs a proper constructor
        }
           
    }

    //Pull data from events csv and makes 1 event per grouping
    //New event denoted by EventDescription row
    //description, stat, dc, time, reward
    //success string, successintchanges, successvaluechanges, successstatechanges
    //failure string, 
    public void MakeEvents(){
        string[] eventData = csvEvents.text.Split(new char[] {'\n'});
        for(int i = 0; i < eventData.Length; i += 15){ //15 is however many properties there are
            
            EventNode newEvent = ScriptableObject.CreateInstance<EventNode>(); //this needs a proper constructor
        }
    
    }
}
