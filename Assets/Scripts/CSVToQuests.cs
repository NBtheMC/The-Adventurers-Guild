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
            Storylet newStorylet = ScriptableObject.CreateInstance<Storylet>(); //this also needs a proper constructor
            
            string storyletDescription = storyletData[i].Split(',')[1];
            string head = storyletData[i+1].Split(',')[1];
            string[] csvTriggerInts = storyletData[i+2].Split(',');
            List<Storylet.TriggerInt> triggerInts = new List<Storylet.TriggerInt>();
            for(int j = 1; j < csvTriggerInts.Length; j++){
                string triggerInt = csvTriggerInts[j];
                //Do extra parsing
                triggerInts.Add();
            }
            string[] csvTriggerStates = storyletData[i+3].Split(',');
            List<Storylet.TriggerState> triggerStates = new List<Storylet.TriggerState>();
            for(int j = 1; j < csvTriggerStates.Length; j++){
                string triggerState = csvTriggerStates[j];
                //Do extra parsing
                triggerInts.Add();
            }
            string[] csvTriggerValues = storyletData[i+4].Split(',');
            List<Storylet.TriggerValue> triggerValues = new List<Storylet.TriggerValue>();
            for(int j = 1; j < csvTriggerValues.Length; j++){
                string triggerValue = csvTriggerValues[j];
                //Do extra parsing
                triggerValues.Add();
            }
            string canBeInstanced = storyletData[i+5].Split(',')[1];
            string successString = storyletData[i+6].Split(',')[1];
            string[] csvIntChanges = storyletData[i+7].Split(',');
            List<Storylet.IntChange> intChanges = new List<Storylet.IntChange>();
            for(int j = 1; j < csvIntChanges.Length; j++){
                string intChange = csvIntChanges[j];
                //Do extra parsing
                intChanges.Add();
            }
            string[] csvStateChanges = storyletData[i+8].Split(',');
            List<Storylet.StateChange> stateChanges = new List<Storylet.StateChange>();
            for(int j = 1; j < csvStateChanges.Length; j++){
                string stateChange = csvStateChanges[j];
                //Do extra parsing
                stateChanges.Add();
            }
            string[] csvValueChanges = storyletData[i+9].Split(',');
            List<Storylet.ValueChange> intChanges = new List<Storylet.ValueChange>();
            for(int j = 1; j < csvValueChanges.Length; j++){
                string valueChange = csvValueChanges[j];
                //Do extra parsing
                valueChanges.Add();
            }
        
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
            List<Storylet.IntChange> successIntChanges = new List<Storylet.IntChange>();
            for(int j = 1; j < csvSuccessIntChanges.Length; j++){
                //add intchange to successIntChanges
                string successIntChange = csvSuccessIntChanges[j];
                //Do extra parsing
                successIntChanges.Add();
            }
            string[] csvSuccessValueChanges = eventData[i+8].Split(','); //can be multiple
            List<Storylet.ValueChange> successValueChanges = new List<Storylet.ValueChange>();
            for(int j = 1; j < csvSuccessValueChanges.Length; j++){
                string successValueChange = csvSuccessValueChanges[j];
                //Do extra parsing
                successValueChanges.Add();
            }
            string[] csvSuccessStateChanges = eventData[i+9].Split(','); //can be multiple
            List<Storylet.StateChange> successStateChanges = new List<Storylet.StateChange>();
            for(int j = 1; j < csvSuccessStateChanges.Length; j++){
                string successStateChange = csvSuccessStateChanges[j];
                //Do extra parsing
                successStateChanges.Add();
            }
            //fail stuff
            string failNode = eventData[i+10].Split(',')[1];
            string failString = eventData[i+11].Split(',')[1];
            string[] csvFailIntChanges = eventData[i+12].Split(','); //can be multiple
            List<Storylet.IntChange> failIntChanges = new List<Storylet.IntChange>();
            for(int j = 1; j < csvFailIntChanges.Length; j++){
                string failIntChange = csvFailIntChanges[j];
                //Do extra parsing
                failIntChanges.Add();
            }
            string[] csvFailValueChanges = eventData[i+13].Split(','); //can be multiple
            List<Storylet.ValueChange> failValueChanges = new List<Storylet.ValueChange>();
            for(int j = 1; j < csvFailValueChanges.Length; j++){
                string failValueChange = csvFailValueChanges[j];
                //Do extra parsing
                failValueChanges.Add();
            }
            string[] csvFailStateChanges = eventData[i+14].Split(','); //can be multiple
            List<Storylet.StateChange> failStateChanges = new List<Storylet.StateChange>();
            for(int j = 1; j < csvFailStateChanges.Length; j++){
                string failStateChange = csvFailStateChanges[j];
                //Do extra parsing
                failStateChanges.Add();
            }
            
        
        }
    
    }
}
