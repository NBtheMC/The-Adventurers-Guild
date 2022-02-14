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
        string[] storyletData = csvStorylets.text.Split(new char[] {'\n'});
        for(int i = 0; i < storyletData.Length - 9; i += 9){ //9 is however many properties there are
            string storyletDescription = storyletData[i].Split(',')[1];
            string head = storyletData[i+1].Split(',')[1];
            string triggerInts = storyletData[i+2].Split(',')[1];
            string triggerStates = storyletData[i+3].Split(',')[1];
            string triggerValues = storyletData[i+4].Split(',')[1];
            string canBeInstanced = storyletData[i+5].Split(',')[1];
            string successString = storyletData[i+6].Split(',')[1];
            string[] triggerIntChanges = storyletData[i+7].Split(',');
            string[] triggerStateChanges = storyletData[i+8].Split(',');
            string[] triggerValueChangess = storyletData[i+9].Split(',');
            
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
            //basic properties
            string eventDescription = eventData[i].Split(',')[1];
            string stat = eventData[i+1].Split(',')[1];
            string dc = eventData[i+2].Split(',')[1];
            string time = eventData[i+3].Split(',')[1];
            string reward = eventData[i+4].Split(',')[1];
            //success stuff
            string successNode = eventData[i+5].Split(',')[1];
            string successString = eventData[i+6].Split(',')[1];
            string[] csvSuccessIntChanges = eventData[i+7].Split(','); //can be multiple
            string[] successIntChanges = new string[csvSuccessIntChanges.Length-1];
            for(int j = 1; j < csvSuccessIntChanges.Length; j++){

            }
            string[] csvSuccessValueChanges = eventData[i+8].Split(','); //can be multiple
            string[] successValueChanges = new string[csvSuccessValueChanges.Length-1];
            for(int j = 1; j < csvSuccessValueChanges.Length; j++){

            }
            string[] csvSuccessStateChanges = eventData[i+9].Split(','); //can be multiple
            string[] successStateChanges = new string[csvSuccessStateChanges.Length-1];
            for(int j = 1; j < csvSuccessStateChanges.Length; j++){

            }
            //fail stuff
            string failNode = eventData[i+10].Split(',')[1];
            string failString = eventData[i+11].Split(',')[1];
            string[] csvFailIntChanges = eventData[i+12].Split(','); //can be multiple
            string[] failIntChanges = new string[csvFailIntChanges.Length-1];
            for(int j = 1; j < csvFailIntChanges.Length; j++){

            }
            string[] csvFailValueChanges = eventData[i+13].Split(','); //can be multiple
            string[] failValueChanges = new string[csvFailValueChanges.Length-1];
            for(int j = 1; j < csvFailValueChanges.Length; j++){

            }
            string[] csvFailStateChanges = eventData[i+14].Split(','); //can be multiple
            string[] failStateChanges = new string[csvFailStateChanges.Length-1];
            for(int j = 1; j < csvFailStateChanges.Length; j++){

            }
            EventNode newEvent = ScriptableObject.CreateInstance<EventNode>(); //this needs a proper constructor
        
        }
    
    }
}
