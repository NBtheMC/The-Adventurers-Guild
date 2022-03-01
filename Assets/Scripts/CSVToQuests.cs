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
        // string[] storyletData = csvStorylets.text.Split(new char[] {'\n'});
        // for(int i = 0; i < storyletData.Length - 9; i += 9){ //9 is however many properties there are
        //     Storylet newStorylet = ScriptableObject.CreateInstance<Storylet>(); //this also needs a proper constructor
            
        //     string storyletDescription = storyletData[i].Split(',')[1];
        //     string head = storyletData[i+1].Split(',')[1];
        //     string[] csvTriggerInts = storyletData[i+2].Split(',');
        //     List<Storylet.TriggerInt> triggerInts = new List<Storylet.TriggerInt>();
        //     for(int j = 1; j < csvTriggerInts.Length; j+=3){
        //         Storylet.TriggerInt newTriggerInt = new Storylet.TriggerInt();
        //         newTriggerInt.name = csvTriggerInts[j];
        //         //newTriggerInt.value = int.Parse(csvTriggerInts[j+1]);
        //         string sign = csvTriggerInts[j+2];
        //         switch (sign){
        //             case "<":
        //                 newTriggerInt.triggerType = Storylet.NumberTriggerType.LessThan;
        //                 break;
        //             case "<=":
        //                 newTriggerInt.triggerType = Storylet.NumberTriggerType.LessThanEqualTo;
        //                 break;
        //             case "=":
        //                 newTriggerInt.triggerType = Storylet.NumberTriggerType.EqualTo;
        //                 break;
        //             case ">":
        //                 newTriggerInt.triggerType = Storylet.NumberTriggerType.GreaterThan;
        //                 break;
        //             case ">=":
        //                 newTriggerInt.triggerType = Storylet.NumberTriggerType.GreaterThanEqualTo;
        //                 break;
        //         }
        //         triggerInts.Add(newTriggerInt);
        //     }
        //     newStorylet.triggerInts = triggerInts;

        //     string[] csvTriggerStates = storyletData[i+3].Split(',');
        //     List<Storylet.TriggerState> triggerStates = new List<Storylet.TriggerState>();
        //     for(int j = 1; j < csvTriggerStates.Length; j+=2){
        //         Storylet.TriggerState newTriggerState = new Storylet.TriggerState();
        //         newTriggerState.name = csvTriggerStates[j];
        //         //newTriggerState.state = Bool.Parse(csvTriggerStates[j+1]);
        //         triggerStates.Add(newTriggerState);
        //     }
        //     newStorylet.triggerStates = triggerStates;

        //     string[] csvTriggerValues = storyletData[i+4].Split(',');
        //     List<Storylet.TriggerValue> triggerValues = new List<Storylet.TriggerValue>();
        //     for(int j = 1; j < csvTriggerValues.Length; j+=3){
        //         Storylet.TriggerValue newTriggerValue = new Storylet.TriggerValue();
        //         newTriggerValue.name = csvTriggerValues[j];
        //         //newTriggerValue.value = Double.Parse(csvTriggerValues[j+1]);
        //         string sign = csvTriggerInts[j+2];
        //         switch (sign){
        //             case "<":
        //                 newTriggerValue.triggerType = Storylet.NumberTriggerType.LessThan;
        //                 break;
        //             case "<=":
        //                 newTriggerValue.triggerType = Storylet.NumberTriggerType.LessThanEqualTo;
        //                 break;
        //             case "=":
        //                 newTriggerValue.triggerType = Storylet.NumberTriggerType.EqualTo;
        //                 break;
        //             case ">":
        //                 newTriggerValue.triggerType = Storylet.NumberTriggerType.GreaterThan;
        //                 break;
        //             case ">=":
        //                 newTriggerValue.triggerType = Storylet.NumberTriggerType.GreaterThanEqualTo;
        //                 break;
        //         }
        //         triggerValues.Add(newTriggerValue);
        //     }
        //     newStorylet.triggerValues = triggerValues;

        //     string canBeInstanced = storyletData[i+5].Split(',')[1];
        //     newStorylet.canBeInstanced = Bool.Parse(canBeInstanced);

        //     string[] csvIntChanges = storyletData[i+6].Split(',');
        //     List<Storylet.IntChange> intChanges = new List<Storylet.IntChange>();
        //     for(int j = 1; j < csvIntChanges.Length; j+=2){
        //         Storylet.IntChange newIntChange = new Storylet.IntChange();
                
        //         //Do extra parsing
        //         intChanges.Add(newIntChange);
        //     }
        //     newStorylet.triggerIntChanges = intChanges;

        //     string[] csvStateChanges = storyletData[i+7].Split(',');
        //     List<Storylet.StateChange> stateChanges = new List<Storylet.StateChange>();
        //     for(int j = 1; j < csvStateChanges.Length; j+=2){
        //         Storylet.StateChange newStateChange = new Storylet.StateChange();
                
        //         //Do extra parsing
        //         stateChanges.Add(newStateChange);
        //     }
        //     newStorylet.triggerStateChanges = stateChanges;

        //     string[] csvValueChanges = storyletData[i+8].Split(',');
        //     List<Storylet.ValueChange> valueChanges = new List<Storylet.ValueChange>();
        //     for(int j = 1; j < csvValueChanges.Length; j+=2){
        //         Storylet.ValueChange newValueChange = new Storylet.ValueChange();
        //         newValueChange.name = csvValueChanges[j];
        //         newValueChange.state = csvValueChanges[j];
        //         //Do extra parsing
        //         valueChanges.Add(newValueChange);
        //     }
        //     newStorylet.triggerValueChanges = valueChanges;
        
        // }

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
                //successIntChanges.Add();
            }
            string[] csvSuccessValueChanges = eventData[i+8].Split(','); //can be multiple
            List<Storylet.ValueChange> successValueChanges = new List<Storylet.ValueChange>();
            for(int j = 1; j < csvSuccessValueChanges.Length; j++){
                string successValueChange = csvSuccessValueChanges[j];
                //Do extra parsing
                //successValueChanges.Add();
            }
            string[] csvSuccessStateChanges = eventData[i+9].Split(','); //can be multiple
            List<Storylet.StateChange> successStateChanges = new List<Storylet.StateChange>();
            for(int j = 1; j < csvSuccessStateChanges.Length; j++){
                string successStateChange = csvSuccessStateChanges[j];
                //Do extra parsing
                //successStateChanges.Add();
            }
            //fail stuff
            string failNode = eventData[i+10].Split(',')[1];
            string failString = eventData[i+11].Split(',')[1];
            string[] csvFailIntChanges = eventData[i+12].Split(','); //can be multiple
            List<Storylet.IntChange> failIntChanges = new List<Storylet.IntChange>();
            for(int j = 1; j < csvFailIntChanges.Length; j++){
                string failIntChange = csvFailIntChanges[j];
                //Do extra parsing
                //failIntChanges.Add();
            }
            string[] csvFailValueChanges = eventData[i+13].Split(','); //can be multiple
            List<Storylet.ValueChange> failValueChanges = new List<Storylet.ValueChange>();
            for(int j = 1; j < csvFailValueChanges.Length; j++){
                string failValueChange = csvFailValueChanges[j];
                //Do extra parsing
                //failValueChanges.Add();
            }
            string[] csvFailStateChanges = eventData[i+14].Split(','); //can be multiple
            List<Storylet.StateChange> failStateChanges = new List<Storylet.StateChange>();
            for(int j = 1; j < csvFailStateChanges.Length; j++){
                string failStateChange = csvFailStateChanges[j];
                //Do extra parsing
                //failStateChanges.Add();
            }
            
        
        }
    
    }
}
