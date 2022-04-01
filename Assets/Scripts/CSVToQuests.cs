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
            
            string storyletDescription = storyletData[i].Split('/t')[1];
            string head = storyletData[i+1].Split('/t')[1];
            string[] csvTriggerInts = storyletData[i+2].Split('/t');
            List<Storylet.TriggerInt> triggerInts = new List<Storylet.TriggerInt>();
            for(int j = 1; j < csvTriggerInts.Length; j+=2){
                Storylet.TriggerInt newTriggerInt = new Storylet.TriggerInt();
                
                //Do extra parsing
                triggerInts.Add(newTriggerInt);
            }
            string[] csvTriggerStates = storyletData[i+3].Split('/t');
            List<Storylet.TriggerState> triggerStates = new List<Storylet.TriggerState>();
            for(int j = 1; j < csvTriggerStates.Length; j+=2){
                Storylet.TriggerState newTriggerState = new Storylet.TriggerState();
                
                //Do extra parsing
                triggerStates.Add(newTriggerState);
            }
            string[] csvTriggerValues = storyletData[i+4].Split('/t');
            List<Storylet.TriggerValue> triggerValues = new List<Storylet.TriggerValue>();
            for(int j = 1; j < csvTriggerValues.Length; j+=2){
                Storylet.TriggerValue newTriggerValue = new Storylet.TriggerValue();
                
                //Do extra parsing
                triggerValues.Add(newTriggerValue);
            }
            string canBeInstanced = storyletData[i+5].Split('/t')[1];
            string successString = storyletData[i+6].Split('/t')[1];
            string[] csvIntChanges = storyletData[i+7].Split('/t');
            List<Storylet.IntChange> intChanges = new List<Storylet.IntChange>();
            for(int j = 1; j < csvIntChanges.Length; j+=2){
                Storylet.TriggerValue newTriggerValue = new Storylet.TriggerValue();
                
                //Do extra parsing
                triggerValues.Add(newTriggerValue);
            }
            string[] csvStateChanges = storyletData[i+8].Split('/t');
            List<Storylet.StateChange> stateChanges = new List<Storylet.StateChange>();
            for(int j = 1; j < csvStateChanges.Length; j+=2){
                Storylet.StateChange newStateChange = new Storylet.StateChange();
                
                //Do extra parsing
                stateChanges.Add(newStateChange);
            }
            string[] csvValueChanges = storyletData[i+9].Split('/t');
            List<Storylet.ValueChange> valueChanges = new List<Storylet.ValueChange>();
            for(int j = 1; j < csvValueChanges.Length; j+=2){
                Storylet.ValueChange newValueChange = new Storylet.ValueChange();
                
                //Do extra parsing
                valueChanges.Add(newValueChange);
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
        for(int i = 0; i < eventData.Length; i += 11){ //15 is however many properties there are
            EventNode newEvent = ScriptableObject.CreateInstance<EventNode>(); //this needs a proper constructor
            //basic properties
            string eventDescription = eventData[i].Split('/t')[1];
            string stat = eventData[i+1].Split('/t')[1];
            string dc = eventData[i+1].Split('/t')[2];
            string time = eventData[i+2].Split('/t')[1];
            string reward = eventData[i+2].Split('/t')[2];
            //success stuff
            string successNode = eventData[i+3].Split('/t')[1];
            string successString = eventData[i+3].Split('/t')[2];
            string[] csvSuccessIntChanges = eventData[i+4].Split('/t'); //can be multiple
            List<Storylet.IntChange> successIntChanges = new List<Storylet.IntChange>();
            for(int j = 1; j < csvSuccessIntChanges.Length; j++){
                NumberTriggerType sign;
                //get type of sign
                switch (csvSuccessIntChanges[j+1]){
                    case "<":
                        sign = Storylet.NumberTriggerType.LessThan;
                        break;
                    case "<=":
                        sign = Storylet.NumberTriggerType.LessThanEqualTo;
                        break;
                    case "=":
                        sign = Storylet.NumberTriggerType.EqualTo;
                        break;
                    case ">":
                        sign = Storylet.NumberTriggerType.GreaterThan;
                        break;
                    case ">=":
                        sign = Storylet.NumberTriggerType.Greater;
                        break;
                }
                //Do extra parsing
                successIntChanges.Add(new Storylet.IntChange(csvSuccessIntChanges[j], sign, csvSuccessIntChanges[j+1]));
            }
            string[] csvSuccessValueChanges = eventData[i+8].Split('/t'); //can be multiple
            List<Storylet.ValueChange> successValueChanges = new List<Storylet.ValueChange>();
            for(int j = 1; j < csvSuccessValueChanges.Length; j++){
                NumberTriggerType sign;
                //get type of sign
                switch (csvSuccessValueChanges[j+1]){
                    case "<":
                        sign = Storylet.NumberTriggerType.LessThan;
                        break;
                    case "<=":
                        sign = Storylet.NumberTriggerType.LessThanEqualTo;
                        break;
                    case "=":
                        sign = Storylet.NumberTriggerType.EqualTo;
                        break;
                    case ">":
                        sign = Storylet.NumberTriggerType.GreaterThan;
                        break;
                    case ">=":
                        sign = Storylet.NumberTriggerType.Greater;
                        break;
                }
                string successValueChange = csvSuccessValueChanges[j];
                //Do extra parsing
                //successValueChanges.Add();
            }
            string[] csvSuccessStateChanges = eventData[i+9].Split('/t'); //can be multiple
            List<Storylet.StateChange> successStateChanges = new List<Storylet.StateChange>();
            for(int j = 1; j < csvSuccessStateChanges.Length; j++){
                string successStateChange = csvSuccessStateChanges[j];
                //Do extra parsing
                //successStateChanges.Add();
            }
            //fail stuff
            string failNode = eventData[i+10].Split('/t')[1];
            string failString = eventData[i+11].Split('/t')[1];
            string[] csvFailIntChanges = eventData[i+12].Split('/t'); //can be multiple
            List<Storylet.IntChange> failIntChanges = new List<Storylet.IntChange>();
            for(int j = 1; j < csvFailIntChanges.Length; j++){
                string failIntChange = csvFailIntChanges[j];
                //Do extra parsing
                //failIntChanges.Add();
            }
            string[] csvFailValueChanges = eventData[i+13].Split('/t'); //can be multiple
            List<Storylet.ValueChange> failValueChanges = new List<Storylet.ValueChange>();
            for(int j = 1; j < csvFailValueChanges.Length; j++){
                string failValueChange = csvFailValueChanges[j];
                //Do extra parsing
                //failValueChanges.Add();
            }
            string[] csvFailStateChanges = eventData[i+14].Split('/t'); //can be multiple
            List<Storylet.StateChange> failStateChanges = new List<Storylet.StateChange>();
            for(int j = 1; j < csvFailStateChanges.Length; j++){
                string failStateChange = csvFailStateChanges[j];
                //Do extra parsing
                //failStateChanges.Add();
            }
            
        
        }
    
    }
}
