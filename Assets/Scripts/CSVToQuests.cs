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
    void Awake()
    {
        if (csvStorylets == null) { csvStorylets = Resources.Load<TextAsset>("Storylets"); }
        if (csvEvents == null) { csvEvents = Resources.Load<TextAsset>("Events"); }
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

        
        foreach(string[] storyletPacket in storyletStringPackages){ //8 is however many properties there are
            Storylet newStorylet = ScriptableObject.CreateInstance<Storylet>(); //this also needs a proper constructor
            
            //list all the damn things we need.
            string storyletName = storyletPacket[0].Split('\t')[1]; //row 1, col b storylet name.
            string storyletDetails = storyletPacket[0].Split('\t')[2]; // row 1, col c storylet details.
            string eventHeadName = storyletPacket[1].Split('\t')[1]; // row 2, col b event name.
            string toBeInstancedString = storyletPacket[1].Split('\t')[2]; // row 2, col c, to be instanced.
            string toBeDuplicated = storyletPacket[1].Split('\t')[3]; // row 2, col d, to be duplicated.
            string[] triggerIntStrings = storyletPacket[2].Split('\t'); // row 3, all the trigger ints.
            string[] triggerFloatStrings = storyletPacket[3].Split('\t'); // row 4, all the trigger floats.
            string[] triggerBoolStrings = storyletPacket[4].Split('\t'); // row 5, all the trigger bools.
            string[] changeIntStrings = storyletPacket[5].Split('\t'); // row 6, all changing ints.
            string[] changeFloatStrings = storyletPacket[6].Split('\t'); // row 7, all changing floats.
            string[] changeBoolStrings = storyletPacket[7].Split('\t'); // row 8, all changing bools.

            // Storylet names and description.
            newStorylet.name = storyletName;
            newStorylet.questName = storyletName;
            newStorylet.questDescription = storyletDetails;
            
            // Get the instancing box.
            bool temporaryHoldBool = false;
            if (!bool.TryParse(toBeInstancedString.ToLower(), out temporaryHoldBool))
            {Debug.Log($"bool unable to be parsed in {storyletName}'s instance box, colum c. Has {toBeInstancedString.ToLower()}. set to false.");}
            newStorylet.canBeInstanced = temporaryHoldBool;

            // Get the duplication box.
            temporaryHoldBool = false;
            if (!bool.TryParse(toBeDuplicated.ToLower(), out temporaryHoldBool))
            {Debug.Log($"bool unable to be parsed in {storyletName}'s duplication box, colum d. Has {toBeInstancedString.ToLower()}. set to false.");}
            newStorylet.canBeDuplicated = temporaryHoldBool;

            // Put the event node in.
            EventNode temporaryLookupNode = null;
            if (eventHeadName!= "" && !eventLookup.TryGetValue(eventHeadName,out temporaryLookupNode)){Debug.LogError($"{storyletName}'s eventNode could not be found, skipping."); continue; }
            newStorylet.eventHead = temporaryLookupNode;

            // The trigger ints.
            List<Storylet.TriggerInt> triggerInts = new List<Storylet.TriggerInt>();
            for(int j = 1; j < triggerIntStrings.Length-1; j+=3){
                Storylet.TriggerInt newTriggerInt = new Storylet.TriggerInt();
                //Set the name.
                if(triggerIntStrings[j] == ""){continue;}
                newTriggerInt.name = triggerIntStrings[j];

                //get type of sign
                Storylet.NumberTriggerType sign = new Storylet.NumberTriggerType();
                if (!tryFindSign(triggerIntStrings[j+1], ref sign)){ Debug.LogError($"Sign not recognized in {storyletName}'s Trigger Ints, column {j+1}, skipping"); continue; }
                newTriggerInt.triggerType = sign;

                // Get the value.
                int inputValue;
                if (!int.TryParse(triggerIntStrings[j+2], out inputValue)){Debug.Log($"Int unable to be parsed in {storyletName}'s Trigger Ints, colum {j+2}, skipping"); continue; }
                newTriggerInt.value = inputValue;
                
                //Add it to the list.
                triggerInts.Add(newTriggerInt);
            }
            newStorylet.triggerInts = triggerInts; // Add the list to the storylet.

            //TRIGGER VALUES
            List<Storylet.TriggerValue> triggerValues = new List<Storylet.TriggerValue>();
            for(int j = 1; j < triggerFloatStrings.Length-1; j+=3){
                Storylet.TriggerValue newTriggerFloat = new Storylet.TriggerValue();
                //Set the name.
                if(triggerFloatStrings[j] == ""){continue;}
                newTriggerFloat.name = triggerFloatStrings[j];

                //Set the sign.
                Storylet.NumberTriggerType sign = new Storylet.NumberTriggerType();
                if (!tryFindSign(triggerFloatStrings[j+1], ref sign)){ Debug.LogError($"Sign not recognized in {storyletName}'s Trigger Values, column {j+1}, skipping"); continue; }
                newTriggerFloat.triggerType = sign;

                // Get the value.
                float inputValue;
                if (!float.TryParse(triggerFloatStrings[j+2], out inputValue)){Debug.Log($"Float unable to be parsed in {storyletName}'s Trigger Values, colum {j+2}, skipping"); continue; }
                newTriggerFloat.value = inputValue;

                //Add it to the list.
                triggerValues.Add(newTriggerFloat);
            }
            newStorylet.triggerValues = triggerValues;

            //TRIGGER STATES
            List<Storylet.TriggerState> triggerBools = new List<Storylet.TriggerState>();
            for(int j = 1; j < triggerBoolStrings.Length-1; j+=3){
                Storylet.TriggerState newTriggerState = new Storylet.TriggerState();
                //Set the name.
                if(triggerBoolStrings[j] == ""){continue;}
                newTriggerState.name = triggerBoolStrings[j];

                // Get the value.
                bool inputValue;
                if (!bool.TryParse(triggerBoolStrings[j+1].ToLower(), out inputValue)){
                    Debug.Log($"bool unable to be parsed in {storyletName}'s Trigger States, colum {j+1}. Has {triggerBoolStrings[j + 1].ToLower()}. skipping");
                    continue; }
                newTriggerState.state = inputValue;

                // Add it to the list.
                triggerBools.Add(newTriggerState);
            }
            newStorylet.triggerStates = triggerBools;

            //INT CHANGES
            List<Storylet.IntChange> intChanges = new List<Storylet.IntChange>();
            for(int j = 1; j < changeIntStrings.Length-1; j+=3){
                // Create the new items.
                Storylet.IntChange newIntChange = new Storylet.IntChange();
                if(changeIntStrings[j] == ""){continue;}
                newIntChange.name = changeIntStrings[j];

                // The value to change.
                int inputValue;
                if (!int.TryParse(changeIntStrings[j+1], out inputValue)){
                    Debug.Log($"Int unable to be parsed in {storyletName}'s Int Changes, colum {j+1}, skipping");
                    continue; }
                newIntChange.value = inputValue;

                // The set value.
                switch (changeIntStrings[j+2]){
                    case "SET":
                        newIntChange.set = true;
                        break;
                    default:
                        newIntChange.set = false;
                        break;
                }

                intChanges.Add(newIntChange);
            }
            newStorylet.triggerIntChanges = intChanges;

            //VALUE CHANGES
            List<Storylet.ValueChange> valueChanges = new List<Storylet.ValueChange>();
            for(int j = 1; j < changeFloatStrings.Length-1; j+=3){
                // Create the new items.
                Storylet.ValueChange newValueChange = new Storylet.ValueChange();
                if(changeFloatStrings[j] == ""){continue;}
                newValueChange.name = changeFloatStrings[j];

                // The valeu to change.
                float inputValue;
                if (!float.TryParse(changeFloatStrings[j+1], out inputValue)){Debug.Log($"Float unable to be parsed in {storyletName}'s Float Changes, colum {j+1}, skipping"); continue; }
                newValueChange.value = inputValue;

                // the set value
                switch (changeFloatStrings[j+2]){
                    case "SET":
                        newValueChange.set = true;
                        break;
                    default:
                        newValueChange.set = false;
                        break;
                }
                //Do extra parsing
                valueChanges.Add(newValueChange);
            }
            newStorylet.triggerValueChanges = valueChanges;

            //STATE CHANGES
            List<Storylet.StateChange> stateChanges = new List<Storylet.StateChange>();
            for(int j = 1; j < changeBoolStrings.Length-1; j+=3){
                Storylet.StateChange newStateChange = new Storylet.StateChange();
                if(changeBoolStrings[j] == ""){continue;}
                newStateChange.name = changeBoolStrings[j];

                // The valeu to change.
                bool inputValue;
                if (!bool.TryParse(changeBoolStrings[j+1].ToLower(), out inputValue)){
                    Debug.Log($"Bool unable to be parsed in {storyletName}'s bool Changes. has {changeBoolStrings[j + 1].ToLower()}, colum {j+1}, skipping");
                    continue; }
                newStateChange.state = inputValue;
                //Do extra parsing
                stateChanges.Add(newStateChange);
            }
            newStorylet.triggerStateChanges = stateChanges;

            // Add the damn thing.
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
            
            eventNodeStringPackages.Add(eventNodepackage);
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
            EventNode temporaryLookupNode;
            if (successNodestring == ""){ newEvent.successNode = null; }
            else if (!eventLookup.TryGetValue(successNodestring,out temporaryLookupNode)){
                Debug.LogError($"{nameString}'s success Node could not be found, skipping.");
                continue;
            }
            else {newEvent.successNode = temporaryLookupNode;}
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

            // adds new connections to the failureNode.
            if (successNodestring == "") { newEvent.failureNode = null; }
            else if (!eventLookup.TryGetValue(failNodestring,out temporaryLookupNode)){
                Debug.LogError($"{nameString}'s failure Node could not be found, skipping.");
                continue;
            }
            else { newEvent.failureNode = temporaryLookupNode; }
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

            allEvents.Add(newEvent);
            Debug.Log($"Added Event {eventDescription}");
        }
    }

    private bool tryFindSign(string inputType, ref Storylet.NumberTriggerType changer){
        bool correctCase = true;
        switch (inputType){
            case "<":
                changer = Storylet.NumberTriggerType.LessThan;
                break;
            case "<=":
                changer = Storylet.NumberTriggerType.LessThanEqualTo;
                break;
            case "equals":
                changer = Storylet.NumberTriggerType.EqualTo;
                break;
            case ">":
                changer = Storylet.NumberTriggerType.GreaterThan;
                break;
            case ">=":
                changer = Storylet.NumberTriggerType.GreaterThanEqualTo;
                break;
            default:
                correctCase = false;
                break;
        }
        return correctCase;
    }
}