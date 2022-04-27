using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class CSVToQuests : MonoBehaviour
{
    public TextAsset csvStorylets;
    public TextAsset csvEvents;

    public List<Storylet> allStorylets;
    public List<EventNode> allEvents;

    private Dictionary<string,EventNode> eventLookup;

    public void MakeEverything(){
        MakeEvents();
        MakeStorylets();
    }

    // Start is called before the first frame update
    void Start()
    {
        csvStorylets = Resources.Load<TextAsset>("Storylets");
        csvEvents = Resources.Load<TextAsset>("Events");
        MakeEverything();
    }

    //Pull data from storylets csv and makes 1 event per grouping
    //New event denoted by Storylet name row
    public void MakeStorylets(){
        if (eventLookup == null) {Debug.LogError("MakeStorylets() was callled before MakeEvents. Call MakeEvents first");}
        string[] storyletData = csvStorylets.text.Split(new char[] {'\n'});
        List<string[]> storyletStringPackages = new List<string[]>();

        // Seperate all items into packages.
        for(int i = 0; i < storyletData.Length - 1; i += 8){
            string[] storyletPackage = new string[8];
            System.Array.Copy(storyletData, i,storyletPackage,0,8);
            storyletStringPackages.Add(storyletPackage);
        }

        
        for(int i = 0; i < storyletData.Length - 8; i += 8){ //8 is however many properties there are
            Storylet newStorylet = ScriptableObject.CreateInstance<Storylet>(); //this also needs a proper constructor

            //NAME, DESCRIPTION, and HEAD
            string storyletName = storyletData[i].Split('\t')[1];
            string assetPath = AssetDatabase.GetAssetPath(newStorylet.GetInstanceID());
            AssetDatabase.RenameAsset(assetPath, storyletName);
            AssetDatabase.SaveAssets();
            Debug.Log("Path = " + assetPath);
            string storyletDescription = storyletData[i].Split('\t')[2];
            string head = storyletData[i+1].Split('\t')[1]; //get actual head with this later in AttachAll()
            newStorylet.eventHead = allEvents.Find(e => Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(e.GetInstanceID())).Contains(head));
            //TRIGGER INTS
            string[] csvTriggerInts = storyletData[i+2].Split('\t');
            List<Storylet.TriggerInt> triggerInts = new List<Storylet.TriggerInt>();
            for(int j = 1; j < csvTriggerInts.Length; j+=3){
                Storylet.TriggerInt newTriggerInt = new Storylet.TriggerInt();
                //Do extra parsing
                newTriggerInt.name = csvTriggerInts[j];
                newTriggerInt.value = int.Parse(csvTriggerInts[j+1]);
                //get type of sign
                Storylet.NumberTriggerType sign = new Storylet.NumberTriggerType();
                switch (csvTriggerInts[j+2]){
                    case "<":
                        sign = Storylet.NumberTriggerType.LessThan;
                        break;
                    case "<=":
                        sign = Storylet.NumberTriggerType.LessThanEqualTo;
                        break;
                    case "equals":
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
            newStorylet.triggerInts = triggerInts;

            //TRIGGER VALUES
            string[] csvTriggerValues = storyletData[i+3].Split('\t');
            List<Storylet.TriggerValue> triggerValues = new List<Storylet.TriggerValue>();
            for(int j = 1; j < csvTriggerValues.Length; j+=3){
                Storylet.TriggerValue newTriggerValue = new Storylet.TriggerValue();
                newTriggerValue.name = csvTriggerValues[j];
                Storylet.NumberTriggerType sign = new Storylet.NumberTriggerType();
                switch (csvTriggerValues[j+1]){
                    case "<":
                        sign = Storylet.NumberTriggerType.LessThan;
                        break;
                    case "<=":
                        sign = Storylet.NumberTriggerType.LessThanEqualTo;
                        break;
                    case "equals":
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
                newTriggerValue.value = float.Parse(csvTriggerValues[j+2]);
                
                triggerValues.Add(newTriggerValue);
            }
            newStorylet.triggerValues = triggerValues;

            //TRIGGER STATES
            string[] csvTriggerStates = storyletData[i+4].Split('\t');
            List<Storylet.TriggerState> triggerStates = new List<Storylet.TriggerState>();
            for(int j = 1; j < csvTriggerStates.Length; j+=2){
                Storylet.TriggerState newTriggerState = new Storylet.TriggerState();
                newTriggerState.name = csvTriggerStates[j];
                newTriggerState.state = bool.Parse(csvTriggerStates[j+1]);
                triggerStates.Add(newTriggerState);
            }
            newStorylet.triggerStates = triggerStates;

            //INT CHANGES
            string[] csvIntChanges = storyletData[i+5].Split('\t');
            List<Storylet.IntChange> intChanges = new List<Storylet.IntChange>();
            for(int j = 1; j < csvIntChanges.Length; j+=3){
                Storylet.IntChange newIntChange = new Storylet.IntChange();
                newIntChange.name = csvIntChanges[j];
                newIntChange.value = int.Parse(csvIntChanges[j+1]);
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
            newStorylet.triggerIntChanges = intChanges;

            //VALUE CHANGES
            string[] csvValueChanges = storyletData[i+6].Split('\t');
            List<Storylet.ValueChange> valueChanges = new List<Storylet.ValueChange>();
            for(int j = 1; j < csvValueChanges.Length; j+=3){
                Storylet.ValueChange newValueChange = new Storylet.ValueChange();
                newValueChange.name = csvValueChanges[j];
                newValueChange.value = float.Parse(csvValueChanges[j+1]);
                switch (csvValueChanges[j+2]){
                    case "SET":
                        newValueChange.set = true;
                        break;
                    case "NOSET":
                        newValueChange.set = false;
                        break;
                }
                newValueChange.set = bool.Parse(csvValueChanges[j+2]);
                //Do extra parsing
                valueChanges.Add(newValueChange);
            }
            newStorylet.triggerValueChanges = valueChanges;

            //STATE CHANGES
            string[] csvStateChanges = storyletData[i+7].Split('\t');
            List<Storylet.StateChange> stateChanges = new List<Storylet.StateChange>();
            for(int j = 1; j < csvStateChanges.Length; j+=2){
                Storylet.StateChange newStateChange = new Storylet.StateChange();
                newStateChange.name = csvStateChanges[j];
                newStateChange.state = bool.Parse(csvStateChanges[j+1]);
                //Do extra parsing
                stateChanges.Add(newStateChange);
            }
            newStorylet.triggerStateChanges = stateChanges;
            allStorylets.Add(newStorylet);
            Debug.Log("Storylet added: " + newStorylet.questName);
        }
    }

    //Pull data from events csv and makes 1 event per grouping
    //New event denoted by EventDescription row
    //description, stat, dc, time, reward
    //success string, successintchanges, successvaluechanges, successstatechanges
    //failure string, 
    public void MakeEvents(){
        string[] eventData = csvEvents.text.Split(new char[] {'\n'});
        List<string[]> eventNodeStringPackages = new List<string[]>();
        eventLookup = new Dictionary<string, EventNode>();
        // Gets every single event package and seperates them into nice little packets.
        for(int i = 0; i < eventData.Length - 1; i += 11){
            string[] eventNodepackage = new string[11];
            System.Array.Copy(eventData, i,eventNodepackage,0,11); //If it errors out here, it's most likely the inputs aren't on 11 scale anymore.
        }

        //Premake an event Node connection for assignment later.
        foreach(string[] eventNodePackage in eventNodeStringPackages){
            EventNode newEvent = ScriptableObject.CreateInstance<EventNode>();
            string eventDescription = eventNodePackage[0].Split('\t')[1]; // row 1, col b - EventName
            eventLookup.Add(eventDescription, newEvent);
        }


        foreach(string[] eventNodePackage in eventNodeStringPackages) { //11 is however many properties there are
            
            // getting all the properties 
            string nameString = eventNodePackage[0].Split('\t')[1]; // row 1, col b - EventName
            string eventDescription = eventNodePackage[0].Split('\t')[2]; // row 1, col c - Event Description
            string stat = eventNodePackage[1].Split('\t')[1]; // row 2, col b - Stat
            string DCstring = eventNodePackage[1].Split('\t')[2]; //row 2, col c - DC to check
            string timeString = eventNodePackage[2].Split('\t')[1]; //row 3, col b - Time to take
            string rewardString = eventNodePackage[2].Split('\t')[2]; // row 3, col c - the reward space
            string successNodestring = eventNodePackage[3].Split('\t')[1]; // row 4, col b - success node string
            string successDetailstring = eventNodePackage[3].Split('\t')[2]; // row 4, col c - success detail strin
            string[] successIntChangestring = eventNodePackage[4].Split('\t'); // row 5 basically the entire success int line.
            string[] successFloatChangestring = eventNodePackage[5].Split('\t'); // row 6, all succcess value lines.
            string[] successBoolChangestring = eventNodePackage[6].Split('\t'); // row 7, all success boolean lines.
            string failNodestring = eventNodePackage[7].Split('\t')[1]; // row 8, col b - fail node
            string failDetailstring = eventNodePackage[7].Split('\t')[2]; // row 8, col c - fail detail string.
            string[] failIntChangeString = eventNodePackage[8].Split('\t'); // row 9, all fail int lines.
            string[] failFloatChangeString = eventNodePackage[9].Split('\t'); // row 10, all fail float lines.
            string[] failBoolChangestring = eventNodePackage[10].Split('\t'); // row 11, all bool float lines.

            // Making the node.
            EventNode newEvent = eventLookup[nameString]; //this needs a proper constructor
            // The following is all dedicated to putting the damn thing into the EventNode
            newEvent.name = nameString;
            newEvent.description = eventDescription;
            switch(stat){
                case "Combat":
                    newEvent.stat =  CharacterSheet.StatDescriptors.Combat;
                    break;
                case "Exploration":
                    newEvent.stat =  CharacterSheet.StatDescriptors.Exploration;
                    break;
                case "Negotiation":
                    newEvent.stat =  CharacterSheet.StatDescriptors.Negotiation;
                    break;
                case "Constitution":
                    newEvent.stat =  CharacterSheet.StatDescriptors.Constitution;
                    break;
                default:
                    Debug.LogError($"Stat not correct in node {nameString}");
                    break;
            }
            newEvent.DC = int.Parse(DCstring);
            newEvent.time = int.Parse(timeString);
            if(rewardString != ""){newEvent.Reward = int.Parse(rewardString);}

            // adds new connection to the successNode.
            newEvent.successNode = eventLookup[successNodestring];
            newEvent.successString = successDetailstring;

            // Success Int Parsing.
            List<Storylet.IntChange> successIntChanges = new List<Storylet.IntChange>();
            for(int j = 1; j < successIntChangestring.Length; j+=2){
                // get the two string variables: name, and change amount.
                string nameOfVariable = successIntChangestring[j];
                if(nameOfVariable == "") { continue; }
                string valueString = successIntChangestring[j+1];
                if(valueString == "") { Debug.LogError($"Int Variable for node {nameString} success, column {j+1} is not formated correctly."); continue; }

                // Create the new items from storylets.
                Storylet.IntChange newSuccessIntChange = new Storylet.IntChange(); //instantiate the storylet.
                newSuccessIntChange.name = nameOfVariable; //put the name in.
                newSuccessIntChange.value = int.Parse(valueString); //put the value of the int in.
                successIntChanges.Add(newSuccessIntChange); // put the trigger in
            }
            newEvent.successIntChange = successIntChanges; // put all the triggers into the node.

            // Success Float Parsing.
            List<Storylet.ValueChange> successValueChanges = new List<Storylet.ValueChange>();
            for(int j = 1; j < successFloatChangestring.Length; j+=2){
                // get the values out.
                string nameOfVariable = successFloatChangestring[j];
                if(nameOfVariable ==  "") {continue;}
                float inputFloat;
                if(!float.TryParse(successFloatChangestring[j+1], out inputFloat)){ Debug.LogError($"Float Variable for node {nameString} success, column {j+1} is not formated correctly."); continue; }

                // Create the new items from storylets.
                Storylet.ValueChange newSuccessFloatChange = new Storylet.ValueChange();
                newSuccessFloatChange.name = nameOfVariable; // put the name in.
                newSuccessFloatChange.value = inputFloat; // put the value in.
                successValueChanges.Add(newSuccessFloatChange); // add the new change to the list.
            }
            newEvent.successValueChange = successValueChanges; // add the list to the node.\

            // Succcess Bool Parsing
            List<Storylet.StateChange> successStateChanges = new List<Storylet.StateChange>();
            for(int j = 1; j < successBoolChangestring.Length; j+=2){
                // get the values out.
                string nameOfVariable = successBoolChangestring[j];
                if(nameOfVariable ==  "") {continue;}
                bool inputBool;
                if(!bool.TryParse(successBoolChangestring[j+1], out inputBool)){ Debug.LogError($"Bool Variable for node {nameString} fail, column {j+1} is not formated correctly."); continue; }

                // Create the new items from storylets.
                Storylet.StateChange newSuccessBoolChange = new Storylet.StateChange();
                newSuccessBoolChange.name = nameOfVariable; // put the name in.
                newSuccessBoolChange.state = inputBool; // put the value in.
                successStateChanges.Add(newSuccessBoolChange); // add the new change to the list.
            }
            newEvent.successStateChange = successStateChanges;

            newEvent.failureNode = eventLookup[failNodestring];
            newEvent.failureString = failDetailstring;

            // Fail Int Parsing
            List<Storylet.IntChange> failIntChanges = new List<Storylet.IntChange>();
            for(int j = 1; j < failIntChangeString.Length; j+=2){
                // get the two string variables: name, and change amount.
                string nameOfVariable = failIntChangeString[j];
                if(nameOfVariable == "") { continue; }
                int inputInt;
                if(!int.TryParse(failFloatChangeString[j+1], out inputInt)) { Debug.LogError($"Int Variable for node {nameString} fail, column {j+1} is not formated correctly."); continue; }

                // Create the new items from storylets.
                Storylet.IntChange newFailIntChange = new Storylet.IntChange(); //instantiate the storylet.
                newFailIntChange.name = nameOfVariable; //put the name in.
                newFailIntChange.value = inputInt; //put the value of the int in.
                failIntChanges.Add(newFailIntChange); // put the trigger in
            }
            newEvent.failIntChange = failIntChanges;

            // Fail Value Parsing.
            List<Storylet.ValueChange> failValueChanges = new List<Storylet.ValueChange>();
            for(int j = 1; j < failFloatChangeString.Length; j+=3){
                // get the values out.
                string nameOfVariable = successFloatChangestring[j];
                if(nameOfVariable ==  "") {continue;}
                float inputFloat;
                if(!float.TryParse(successFloatChangestring[j+1], out inputFloat)){ Debug.LogError($"Float Variable for node {nameString} fail, column {j+1} is not formated correctly."); continue; }

                // Create the new items from storylets.
                Storylet.ValueChange newFloatChange = new Storylet.ValueChange();
                newFloatChange.name = nameOfVariable; // put the name in.
                newFloatChange.value = inputFloat; // put the value in.
                failValueChanges.Add(newFloatChange); // add the new change to the list.
            }
            newEvent.failValueChange = failValueChanges; // add the list to the node.\

            // Fail Bool Parsing
            List<Storylet.StateChange> failStateChanges = new List<Storylet.StateChange>();
            for(int j = 1; j < failBoolChangestring.Length; j+=2){
                // get the values out.
                string nameOfVariable = failBoolChangestring[j];
                if(nameOfVariable ==  "") {continue;}
                bool inputBool;
                if(!bool.TryParse(failBoolChangestring[j+1], out inputBool)){ Debug.LogError($"Bool Variable for node {nameString} fail, column {j+1} is not formated correctly."); continue; }

                // Create the new items from storylets.
                Storylet.StateChange newBoolChange = new Storylet.StateChange();
                newBoolChange.name = nameOfVariable; // put the name in.
                newBoolChange.state = inputBool; // put the value in.
                failStateChanges.Add(newBoolChange); // add the new change to the list.
            }
            newEvent.failStateChange = failStateChanges;
        }
    }
}