using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVToQuests : MonoBehaviour
{
    public TextAsset csvStorylets;
    public TextAsset csvEvents;

    public void MakeEverything(){
        MakeStorylets();
        MakeEvents();
        AttachAll();
    }
    // Start is called before the first frame update
    void Start()
    {
        csvStorylets = Resources.Load<TextAsset>("Storylets");
        csvEvents = Resources.Load<TextAsset>("Events");
    }

    //Pull data from storylets csv and makes 1 event per grouping
    //New event denoted by Storylet name row
    public void MakeStorylets(){
        string[] storyletData = csvStorylets.text.Split(new char[] {'\n'});
        for(int i = 0; i < storyletData.Length - 8; i += 8){ //8 is however many properties there are
            Storylet newStorylet = ScriptableObject.CreateInstance<Storylet>(); //this also needs a proper constructor
            
            //NAME, DESCRIPTION, and HEAD
            string storyletName = storyletData[i].Split('\t')[1];
            string storyletDescription = storyletData[i].Split('\t')[2];
            string head = storyletData[i+1].Split('\t')[1]; //get actual head with this later in AttachAll()
            
            //TRIGGER INTS
            string[] csvTriggerInts = storyletData[i+2].Split('\t');
            List<Storylet.TriggerInt> triggerInts = new List<Storylet.TriggerInt>();
            for(int j = 1; j < csvTriggerInts.Length; j+=3){
                Storylet.TriggerInt newTriggerInt = new Storylet.TriggerInt();
                
                //Do extra parsing
                newTriggerInt.name = csvTriggerInts[j];
                newTriggerInt.value = int.Parse(csvTriggerInts[j+1]);
                Storylet.NumberTriggerType sign;
                newTriggerInt.triggerType = new Storylet.NumberTriggerType();
                //get type of sign
                Storylet.NumberTriggerType sign;
                switch (csvTriggerInts[j+2]){
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
                        sign = Storylet.NumberTriggerType.GreaterThanEqualTo;
                        break;
                }
                newTriggerInt.triggerType = sign;
                triggerInts.Add(newTriggerInt);
            }

            //TRIGGER VALUES
            string[] csvTriggerValues = storyletData[i+3].Split('\t');
            List<Storylet.TriggerValue> triggerValues = new List<Storylet.TriggerValue>();
            for(int j = 1; j < csvTriggerValues.Length; j+=3){
                newTriggerValue.name = csvTriggerValues[j];
                Storylet.NumberTriggerType sign = new Storylet.NumberTriggerType();
                switch (csvTriggerValues[j+1]){
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
                        sign = Storylet.NumberTriggerType.GreaterThanEqualTo;
                        break;
                }
                newTriggerValue.triggerType = sign;
                newTriggerValue.value = Double.Parse(csvTriggerValues[j+2]);
                
                triggerValues.Add(newTriggerValue);
            }

            //TRIGGER STATES
            string[] csvTriggerStates = storyletData[i+4].Split('\t');
            List<Storylet.TriggerState> triggerStates = new List<Storylet.TriggerState>();
            for(int j = 1; j < csvTriggerStates.Length; j+=2){
                Storylet.TriggerState newTriggerState = new Storylet.TriggerState();
                newTriggerState.name = csvTriggerStates[j];
                newTriggerState.state = Boolean.Parse(csvTriggerStates[j+1]);
                triggerStates.Add(newTriggerState);
            }

            //INT CHANGES
            string[] csvIntChanges = storyletData[i+5].Split('\t');
            List<Storylet.IntChange> intChanges = new List<Storylet.IntChange>();
            for(int j = 1; j < csvIntChanges.Length; j+=3){
                Storylet.IntChange newIntChange = new Storylet.IntChange();
                newIntChange.name = csvIntChanges[j];
                newIntChange.value = csvIntChanges[j+1];
                switch (csvIntChanges[j+2]){
                    case "SET":
                        newIntChange.set = true;
                        break;
                    case "NOSET":
                        newIntChange.set = false;
                        break;
                }
                //Do extra parsing
                intChanges.Add(newIntChange);
            }

            //STATE CHANGES
            string[] csvStateChanges = storyletData[i+6].Split('\t');
            List<Storylet.StateChange> stateChanges = new List<Storylet.StateChange>();
            for(int j = 1; j < csvStateChanges.Length; j+=2){
                Storylet.StateChange newStateChange = new Storylet.StateChange();
                newStateChange.name = csvStateChanges[j];
                newStateChange.state = csvStateChanges[j+1];
                //Do extra parsing
                stateChanges.Add(newStateChange);
            }

            //VALUE CHANGES
            string[] csvValueChanges = storyletData[i+7].Split('\t');
            List<Storylet.ValueChange> valueChanges = new List<Storylet.ValueChange>();
            for(int j = 1; j < csvValueChanges.Length; j+=3){
                Storylet.ValueChange newValueChange = new Storylet.ValueChange();
                newValueChange.name = csvValueChanges[j];
                newValueChange.value = csvValueChanges[j+1];
                newValueChange.set = csvValueChanges[j+2];
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
            string eventDescription = eventData[i].Split('\t')[1];
            string stat = eventData[i+1].Split('\t')[1];
            string dc = eventData[i+1].Split('\t')[2];
            string time = eventData[i+2].Split('\t')[1];
            string reward = eventData[i+2].Split('\t')[2];
            //success stuff
            string successNode = eventData[i+3].Split('\t')[1];
            string successString = eventData[i+3].Split('\t')[2];
            string[] csvSuccessIntChanges = eventData[i+4].Split('\t'); //can be multiple
            List<Storylet.IntChange> successIntChanges = new List<Storylet.IntChange>();
            for(int j = 1; j < csvSuccessIntChanges.Length; j+=3){
                Storylet.NumberTriggerType sign;
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
                        sign = Storylet.NumberTriggerType.GreaterThanEqualTo;
                        break;
                }
                //Do extra parsing
                Storylet.IntChange newSuccessIntChange = new Storylet.IntChange();

                successIntChanges.Add(newSuccessIntChange);
            }
            string[] csvSuccessValueChanges = eventData[i+8].Split('\t'); //can be multiple
            List<Storylet.ValueChange> successValueChanges = new List<Storylet.ValueChange>();
            for(int j = 1; j < csvSuccessValueChanges.Length; j+=3){
                Storylet.ValueChange newValueChange = new Storylet.ValueChange();
                Storylet.NumberTriggerType sign;
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
                        sign = Storylet.NumberTriggerType.GreaterThanEqualTo;
                        break;
                }
                Storylet.ValueChange newSuccessValueChange = new Storylet.ValueChange();

                successValueChanges.Add(newSuccessValueChange);
            }
            string[] csvSuccessStateChanges = eventData[i+9].Split('\t'); //can be multiple
            List<Storylet.StateChange> successStateChanges = new List<Storylet.StateChange>();
            for(int j = 1; j < csvSuccessStateChanges.Length; j+=2){
                Storylet.StateChange newStateChange = new Storylet.StateChange();
                string successStateChange = csvSuccessStateChanges[j];
                //Do extra parsing
                successStateChanges.Add(newStateChange);
            }
            //fail stuff
            string failNode = eventData[i+10].Split('\t')[1];
            string failString = eventData[i+11].Split('\t')[1];
            string[] csvFailIntChanges = eventData[i+12].Split('\t'); //can be multiple
            List<Storylet.IntChange> failIntChanges = new List<Storylet.IntChange>();
            for(int j = 1; j < csvFailIntChanges.Length; j+=3){
                Storylet.IntChange newIntChange = new Storylet.IntChange();
                string failIntChange = csvFailIntChanges[j];
                //Do extra parsing
                failIntChanges.Add(newIntChange);
            }
            string[] csvFailValueChanges = eventData[i+13].Split('\t'); //can be multiple
            List<Storylet.ValueChange> failValueChanges = new List<Storylet.ValueChange>();
            for(int j = 1; j < csvFailValueChanges.Length; j+=3){
                Storylet.ValueChange newValueChange = new Storylet.ValueChange();
                string failValueChange = csvFailValueChanges[j];
                //Do extra parsing
                failValueChanges.Add(newValueChange);
            }
            string[] csvFailStateChanges = eventData[i+14].Split('\t'); //can be multiple
            List<Storylet.StateChange> failStateChanges = new List<Storylet.StateChange>();
            for(int j = 1; j < csvFailStateChanges.Length; j+=2){
                Storylet.StateChange newStateChange = new Storylet.StateChange();
                string failStateChange = csvFailStateChanges[j];
                //Do extra parsing
                failStateChanges.Add(newStateChange);
            }
            
        
        }
    
    }

    //used to connect Event Nodes to each other
    private void AttachAll(){

    }
}
