using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class CSVToQuests : MonoBehaviour
{
    public TextAsset csvStorylets;
    public TextAsset csvEvents;

    public CharacterSheetManager characterSheets;

    public List<Storylet> allStorylets;
    public List<EventNode> allEvents;

    private Dictionary<string,EventNode> eventLookup;

    private WorldStateManager worldStateReference;

    public int ticksPerHour;

    public void MakeEverything(){
        MakeEvents();
        MakeStorylets();
    }

    // Start is called before the first frame update
    void Awake()
    {
        allStorylets = new List<Storylet>();
        allEvents = new List<EventNode>();

        if (csvStorylets == null) { csvStorylets = Resources.Load<TextAsset>("Storylets"); }
        if (csvEvents == null) { csvEvents = Resources.Load<TextAsset>("Events"); }
        MakeEverything();
    }

	private void Start()
    {
        // Geting our time ticks from the system.
        worldStateReference = this.GetComponent<WorldStateManager>();
        ticksPerHour = worldStateReference.timeSystem.TicksPerHour;
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
            Debug.Log($"Packaged {storyletPackage[0].Split('\t')[1]}");
        }

        
        foreach(string[] storyletPacket in storyletStringPackages){ //8 is however many properties there are
            Storylet newStorylet = new Storylet(); //this also needs a proper constructor
            
            //list all the damn things we need.
            string storyletName = storyletPacket[0].Split('\t')[1]; //row 1, col b storylet name.
            string isEndingstring = storyletPacket[0].Split('\t')[2]; // row 1, col c ending string.
            string factionName = storyletPacket[0].Split('\t')[3]; //row 1, col d faction name.
            string questgiverName = storyletPacket[0].Split('\t')[4]; //row 1, col e questgiver name.
            string storyletDetails = storyletPacket[0].Split('\t')[5]; // row 1, col f storylet details.
            string eventHeadName = storyletPacket[1].Split('\t')[1]; // row 2, col b event name.
            string toBeInstancedString = storyletPacket[1].Split('\t')[2]; // row 2, col c, to be instanced.
            string toBeDuplicated = storyletPacket[1].Split('\t')[3]; // row 2, col d, to be duplicated.
            string character = storyletPacket[1].Split('\t')[4]; // row 2, col e, character to be triggered.
            string debriefMessage = storyletPacket[1].Split('\t')[5]; // row 2, col f, debrief message
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

            newStorylet.adventurer = character;
            newStorylet.debriefMessage = debriefMessage;
            newStorylet.factionName = factionName;
            newStorylet.issuerName = questgiverName;

            // Get the end game box.
            bool temporaryHoldBool = false;
            if (!bool.TryParse(isEndingstring.ToLower(), out temporaryHoldBool))
            { Debug.Log($"bool unable to be parsed in {storyletName}'s end game box, colum c. Has {toBeInstancedString.ToLower()}. set to false."); }
            newStorylet.endGame = temporaryHoldBool;

            // Get the instancing box.
            temporaryHoldBool = false;
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
        Debug.Log("Currently Making events.");
        string[] eventData = csvEvents.text.Split(new char[] {'\n'});
        List<string[]> eventNodeStringPackages = new List<string[]>();
        eventLookup = new Dictionary<string, EventNode>();
        // Gets every single event package and seperates them into nice little packets.

        // Iterates through the entire array of string lists to chunk out our packets.
        int dataIter = 1; // start at one to avoid the clash with the first event.
        int packetStartPoint = 0;
        string[] currentPackage;
        //Loops through the entirety of the data.
        while (dataIter < eventData.Length){
            string firstItem = eventData[dataIter].Split('\t')[0]; // Get the first item.
            if (firstItem.Substring(0,5) == "Event") // If it's an event head, package the last bit and start anew.
			{
                currentPackage = new string[dataIter - packetStartPoint];
                System.Array.Copy(eventData,packetStartPoint, currentPackage, 0,dataIter-packetStartPoint);
                eventNodeStringPackages.Add(currentPackage);
                Debug.Log($"Made package - {currentPackage[0].Split('\t')[0]}: {currentPackage[0].Split('\t')[1]} of size {currentPackage.Length}.");
                packetStartPoint = dataIter;
			}
            dataIter++;
        }

        // Catch the last item.
        currentPackage = new string[dataIter - packetStartPoint];
        System.Array.Copy(eventData, packetStartPoint, currentPackage, 0, dataIter - packetStartPoint);
        eventNodeStringPackages.Add(currentPackage);
        Debug.Log($"Made package - {currentPackage[0].Split('\t')[0]}: {currentPackage[0].Split('\t')[1]} of size {currentPackage.Length}.");

        //Premake an event Node connection for assignment later.
        foreach (string[] eventNodePackage in eventNodeStringPackages){
            EventNode newEvent = new EventNode();
            string eventName = eventNodePackage[0].Split('\t')[1]; // row 1, col b - EventName
            eventLookup.Add(eventName, newEvent);
            Debug.Log($"Added {eventName} to lookup table.");
        }


        foreach(string[] eventNodePackage in eventNodeStringPackages) {
            
            // getting all the properties 
            string nameString = eventNodePackage[0].Split('\t')[1]; // row 1, col b - EventName
            string eventDescription = eventNodePackage[0].Split('\t')[2]; // row 1, col c - Event Description

            // Making the node.
            EventNode newEvent = eventLookup[nameString]; //this needs a proper constructor
            // The following is all dedicated to putting the damn thing into the EventNode
            newEvent.name = nameString;
            newEvent.description = eventDescription;

            newEvent.theWorld = worldStateReference; // Make sure it has a reference to the world state we're using.
            newEvent.eventCases = new List<EventNode.EventCase>(); // Initliaizes all the event cases.

            // Chunk out the Cases.
            List<string[]> casePackets = new List<string[]>();
            for (int i = 1; i < eventNodePackage.Length-4; i += 9)
            {
                string[] casePacket = new string[9];
                System.Array.Copy(eventNodePackage, i, casePacket, 0, 9);
                casePackets.Add(casePacket);
            }

            // Get the default case.
            string[] defaultCaseString = new string[4];
            System.Array.Copy(eventNodePackage, eventNodePackage.Length-4, defaultCaseString, 0,4);

            
            // Iterate through all the case Packets.
            foreach(string[] casePacket in casePackets)
			{
                //Identify all items in the packet.
                string nextNode = casePacket[0].Split('\t')[1]; // row 1, col b - Next Node.
                string timeString = casePacket[0].Split('\t')[2]; //row 1, col c - Time to take
                string rewardString = casePacket[0].Split('\t')[3]; // row 1, col d - the reward space
                string partyBond = casePacket[0].Split('\t')[4]; // row 1, col e - the reward space
                string nodeCompletionString = casePacket[0].Split('\t')[5]; // row 1, col f - node completion string
                string[] statTriggerStrings = casePacket[1].Split('\t'); // row 2, party stat triggers.
                string[] triggerIntStrings = casePacket[2].Split('\t'); // row 3, trigger Ints.
                string[] triggerFloatStrings = casePacket[3].Split('\t'); // row 4, trigger Floats.
                string[] triggerBoolStrings = casePacket[4].Split('\t'); // row 5, trigger Bools.
                string[] partyTriggerStrings = casePacket[5].Split('\t'); // row 6, party triggers.
                string[] intChangeStrings = casePacket[6].Split('\t'); // row 7, int changes.
                string[] floatChangeStrings = casePacket[7].Split('\t'); // row 8, float changes.
                string[] boolChangeStrings = casePacket[8].Split('\t'); // row 9, bool changes.

                // Create the Event Case
                EventNode.EventCase tempEventCase = new EventNode.EventCase();

                // Lookup the progressionNode
                EventNode temporaryLookupNode;
                if (nextNode == "") { tempEventCase.nextNode = null; }
                else if (!eventLookup.TryGetValue(nextNode, out temporaryLookupNode))
                {
                    Debug.LogError($"{nameString}'s success Node could not be found, skipping.");
                    continue;
                }

                // Input a bunch of descriptors and values for the node.
                if (rewardString != "") { tempEventCase.reward = int.Parse(rewardString); }
                if (string.IsNullOrEmpty(partyBond)) { tempEventCase.bondupdate = 0; }
                else { tempEventCase.bondupdate = int.Parse(partyBond); }
                tempEventCase.progressionDescription = nodeCompletionString;
                //Get Time
                float eventHours = float.Parse(timeString);
                tempEventCase.time = Mathf.CeilToInt(eventHours * ticksPerHour);

                List<EventNode.StatCheck> caseStatChecks = new List<EventNode.StatCheck>();
                //Parsing through the party Triggers.
                for (int i = 1; i < statTriggerStrings.Length-2; i+=3)
				{
                    Debug.Log($"Working through {nameString} item {(i + 2) / 3} party checks.");
                    bool finish = false;
                    // Get the Stat to check against.
                    EventNode.StatCheck statCheck = new EventNode.StatCheck();
                    switch (statTriggerStrings[i])
                    {
                        case "Combat":
                            statCheck.stat = CharacterSheet.StatDescriptors.Combat;
                            break;
                        case "Exploration":
                            statCheck.stat = CharacterSheet.StatDescriptors.Exploration;
                            break;
                        case "Charisma":
                            statCheck.stat = CharacterSheet.StatDescriptors.Charisma;
                            break;
                        // case "Constitution":
                        //     statCheck.stat = CharacterSheet.StatDescriptors.Constitution;
                        //     break;
                        case "":
                            finish = true;
                            break;
                        default:
                            Debug.LogError($"Stat not correct in node {nameString}, column {i}");
                            break;
                    }
                    if (finish) { break; }
                    
                    // Get the Trigger Type.
                    Storylet.NumberTriggerType triggerType = new Storylet.NumberTriggerType();
                    if (!tryFindSign(statTriggerStrings[i+1], ref triggerType)) {
                        Debug.LogError($"Sign not recognized in {nameString}'s case towards {nextNode}, Stat Check - column {i + 1}, skipping"); continue; }
                    statCheck.triggerType = triggerType;

                    // Get the number.
                    int inputValue;
                    if (!int.TryParse(statTriggerStrings[i + 2], out inputValue)) { Debug.Log($"Int unable to be parsed in {nameString}'s case towards {nextNode}, Stat Check - colum {i + 2}, skipping"); continue; }
                    statCheck.value = inputValue;

                    // Put the party check into the case Party Check
                    caseStatChecks.Add(statCheck);
                    Debug.Log($"Added a statCheck for {statCheck.stat}");
                }
                tempEventCase.statTriggers = caseStatChecks;

                // Now the int Triggers.
                List<Storylet.TriggerInt> caseTriggerInts = new List<Storylet.TriggerInt>();
                for (int i = 1; i < triggerIntStrings.Length-2; i+=3)
                {
                    string[] triggerPackage = new string[3];
                    System.Array.Copy(triggerIntStrings, i, triggerPackage, 0, 3);

                    Storylet.TriggerInt tempTriggerInt;
                    string errorMessage;
                    switch (tryParseTriggerInt(triggerPackage, out tempTriggerInt, out errorMessage))
                    {
                        case 0:
                            caseTriggerInts.Add(tempTriggerInt);
                            break;
                        case 1:
                            break;
                        case 2:
                            Debug.LogError($"{errorMessage} At {nameString}'s case towards {nextNode}. Trigger Ints {(i + 2) / 3}");
                            break;
                    }
                }
                tempEventCase.intTriggers = caseTriggerInts;

                // The Float Triggers.
                List<Storylet.TriggerValue> caseTriggerFloats = new List<Storylet.TriggerValue>();
                for (int i = 1; i < triggerFloatStrings.Length-2; i += 3)
                {
                    string[] triggerPackage = new string[3];
                    System.Array.Copy(triggerFloatStrings, i, triggerPackage, 0, 3);

                    Storylet.TriggerValue tempTrigger;
                    string errorMessage;
                    switch (tryParseTriggerFloat(triggerPackage, out tempTrigger, out errorMessage))
                    {
                        case 0:
                            caseTriggerFloats.Add(tempTrigger);
                            break;
                        case 1:
                            break;
                        case 2:
                            Debug.LogError($"{errorMessage} At {nameString}'s case towards {nextNode}. Trigger Floats {(i + 2) / 3}");
                            break;
                    }
                }
                tempEventCase.floatTriggers = caseTriggerFloats;

                // The Bool Triggers.
                List<Storylet.TriggerState> caseTriggerBools = new List<Storylet.TriggerState>();
                for (int i = 1; i < triggerBoolStrings.Length-2; i += 3)
                {
                    string[] triggerPackage = new string[3];
                    System.Array.Copy(triggerBoolStrings, i, triggerPackage, 0, 3);

                    Storylet.TriggerState tempTrigger;
                    string errorMessage;
                    switch (tryParseTriggerBool(triggerPackage, out tempTrigger, out errorMessage))
                    {
                        case 0:
                            caseTriggerBools.Add(tempTrigger);
                            break;
                        case 1:
                            break;
                        case 2:
                            Debug.LogError($"{errorMessage} At {nameString}'s case towards {nextNode}. Trigger Bools {(i + 2) / 3}");
                            break;
                    }
                }
                tempEventCase.boolTriggers = caseTriggerBools;

                // The Party Triggers
                tempEventCase.partyTriggers = new List<EventNode.PartyCheck>();
                for (int i = 1; i < partyTriggerStrings.Length-2; i+= 3)
				{
                    string[] triggerPackage = new string[3];
                    System.Array.Copy(partyTriggerStrings, i, triggerPackage, 0, 3);

                    if(triggerPackage[0] == "") { break; }

                    EventNode.PartyCheck tempTrigger = new EventNode.PartyCheck();
                    tempTrigger.character = triggerPackage[0];

                    // Get the value.
                    bool inputValue;
                    if (!bool.TryParse(triggerPackage[1].ToLower(), out inputValue)) { Debug.LogError("Bool unable to be parsed, skipping"); continue; }
                    tempTrigger.present = inputValue;

                    tempEventCase.partyTriggers.Add(tempTrigger);
                }

                // The Int Changes
                List<Storylet.IntChange> caseIntChanges = new List<Storylet.IntChange>();
                for (int i = 1; i < intChangeStrings.Length-2; i += 3)
                {
                    string[] triggerPackage = new string[3];
                    System.Array.Copy(intChangeStrings, i, triggerPackage, 0, 3);

                    Storylet.IntChange tempChange;
                    string errorMessage;
                    switch (tryParseIntChange(triggerPackage, out tempChange, out errorMessage))
                    {
                        case 0:
                            caseIntChanges.Add(tempChange);
                            break;
                        case 1:
                            break;
                        case 2:
                            Debug.LogError($"{errorMessage} At {nameString}'s case towards {nextNode}. Int Change {(i + 2) / 3}");
                            break;
                    }
                }
                tempEventCase.intChanges = caseIntChanges;

                // The Float Changes
                List<Storylet.ValueChange> caseFloatChanges = new List<Storylet.ValueChange>();
                for (int i = 1; i < floatChangeStrings.Length-2; i += 3)
                {
                    string[] triggerPackage = new string[3];
                    System.Array.Copy(floatChangeStrings, i, triggerPackage, 0, 3);

                    Storylet.ValueChange tempChange;
                    string errorMessage;
                    switch (tryParseFloatChange(triggerPackage, out tempChange, out errorMessage))
                    {
                        case 0:
                            caseFloatChanges.Add(tempChange);
                            break;
                        case 1:
                            break;
                        case 2:
                            Debug.LogError($"{errorMessage} At {nameString}'s case towards {nextNode}. Float Change {(i + 2) / 3}");
                            break;
                    }
                }
                tempEventCase.floatChanges = caseFloatChanges;

                // The Float Changes
                List<Storylet.StateChange> caseBoolChanges = new List<Storylet.StateChange>();
                for (int i = 1; i < boolChangeStrings.Length-2; i += 3)
                {
                    string[] triggerPackage = new string[3];
                    System.Array.Copy(boolChangeStrings, i, triggerPackage, 0, 3);

                    Storylet.StateChange tempChange;
                    string errorMessage;
                    switch (tryParseBoolChange(triggerPackage, out tempChange, out errorMessage))
                    {
                        case 0:
                            caseBoolChanges.Add(tempChange);
                            break;
                        case 1:
                            break;
                        case 2:
                            Debug.LogError($"{errorMessage} At {nameString}'s case towards {nextNode}. Bool Change {(i + 2) / 3}");
                            break;
                    }
                }
                tempEventCase.boolChanges = caseBoolChanges;

                // All the event case to the node.
                newEvent.eventCases.Add(tempEventCase);
            }

            // Parse the default package.
            string dNextNode = defaultCaseString[0].Split('\t')[1]; // row 1, col b - Next Node.
            string dTimeString = defaultCaseString[0].Split('\t')[2]; //row 1, col c - Time to take
            string dRewardString = defaultCaseString[0].Split('\t')[3]; // row 1, col d - the reward space
            string dPartyBond = defaultCaseString[0].Split('\t')[4]; // row 1, col e - the reward space
            string dNodeCompletionString = defaultCaseString[0].Split('\t')[5]; // row 1, col f - node completion string
            string[] dIntChangeStrings = defaultCaseString[1].Split('\t'); // row 2, int changes.
            string[] dFloatChangeStrings = defaultCaseString[2].Split('\t'); // row 3, float changes.
            string[] dBoolChangeStrings = defaultCaseString[3].Split('\t'); // row 4, bool changes.

            EventNode.EventCase defaultCase = new EventNode.EventCase();

            // Lookup the progressionNode
            EventNode dTemporaryLookupNode;
            if (dNextNode == "") { defaultCase.nextNode = null; }
            else if (!eventLookup.TryGetValue(dNextNode, out dTemporaryLookupNode))
            {
                Debug.LogError($"{nameString}'s success Node could not be found, skipping.");
                continue;
            }

            // Input a bunch of descriptors and values for the node.
            if (dRewardString != "") { defaultCase.reward = int.Parse(dRewardString); }
            if (string.IsNullOrEmpty(dPartyBond)) { defaultCase.bondupdate = 0; }
            else { defaultCase.bondupdate = int.Parse(dPartyBond); }
            defaultCase.progressionDescription = dNodeCompletionString;
            //Get Time
            float dEventHours = float.Parse(dTimeString);
            defaultCase.time = Mathf.CeilToInt(dEventHours * ticksPerHour);

            // Null all the triggers.
            defaultCase.statTriggers = new List<EventNode.StatCheck>();
            defaultCase.intTriggers = new List<Storylet.TriggerInt>();
            defaultCase.floatTriggers = new List<Storylet.TriggerValue>();
            defaultCase.boolTriggers = new List<Storylet.TriggerState>();
            
            // The Int Changes
            List<Storylet.IntChange> dCaseIntChanges = new List<Storylet.IntChange>();
            for (int i = 1; i < dIntChangeStrings.Length-2; i += 3)
            {
                string[] triggerPackage = new string[3];
                System.Array.Copy(dIntChangeStrings, i, triggerPackage, 0, 3);

                Storylet.IntChange tempChange;
                string errorMessage;
                switch (tryParseIntChange(triggerPackage, out tempChange, out errorMessage))
                {
                    case 0:
                        dCaseIntChanges.Add(tempChange);
                        break;
                    case 1:
                        break;
                    case 2:
                        Debug.LogError($"{errorMessage} At {nameString}'s case towards {dNextNode}. Int Change {(i + 2) / 3}");
                        break;
                }
            }
            defaultCase.intChanges = dCaseIntChanges;

            // The Float Changes
            List<Storylet.ValueChange> dCaseFloatChanges = new List<Storylet.ValueChange>();
            for (int i = 1; i < dFloatChangeStrings.Length-2; i += 3)
            {
                string[] triggerPackage = new string[3];
                System.Array.Copy(dFloatChangeStrings, i, triggerPackage, 0, 3);

                Storylet.ValueChange tempChange;
                string errorMessage;
                switch (tryParseFloatChange(triggerPackage, out tempChange, out errorMessage))
                {
                    case 0:
                        dCaseFloatChanges.Add(tempChange);
                        break;
                    case 1:
                        break;
                    case 2:
                        Debug.LogError($"{errorMessage} At {nameString}'s case towards {dNextNode}. Float Change {(i + 2) / 3}");
                        break;
                }
            }
            defaultCase.floatChanges = dCaseFloatChanges;

            // The Float Changes
            List<Storylet.StateChange> dCaseBoolChanges = new List<Storylet.StateChange>();
            for (int i = 1; i < dBoolChangeStrings.Length-2; i += 3)
            {
                string[] triggerPackage = new string[3];
                System.Array.Copy(dBoolChangeStrings, i, triggerPackage, 0, 3);

                Storylet.StateChange tempChange;
                string errorMessage;
                switch (tryParseBoolChange(triggerPackage, out tempChange, out errorMessage))
                {
                    case 0:
                        dCaseBoolChanges.Add(tempChange);
                        break;
                    case 1:
                        break;
                    case 2:
                        Debug.LogError($"{errorMessage} At {nameString}'s case towards {dNextNode}. Bool Change {(i + 2) / 3}");
                        break;
                }
            }
            defaultCase.boolChanges = dCaseBoolChanges;

            // Add the default case to the list and the reference.
            newEvent.eventCases.Add(defaultCase);
            newEvent.defaultCase = defaultCase;

            allEvents.Add(newEvent);
            Debug.Log($"Added Event {nameString}");
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


    /// <summary>
    /// Tries to parse the trigger Ints from a TSV.
    /// </summary>
    /// <param name="inputs">A 3 item string array.</param>
    /// <param name="trigger">The outputed trigger.</param>
    /// <param name="error">The outputed error log.</param>
    /// <returns>0 if correctly parsed. 1 if skipped (no error). 2 if format error.</returns>
    private int tryParseTriggerInt(string[] inputs, out Storylet.TriggerInt trigger, out string error){

        Storylet.TriggerInt newTriggerInt = new Storylet.TriggerInt();
        trigger = newTriggerInt;

        //Set the name.
        if (inputs[0] == ""){error = "No name of world stat, skipped."; return 1;}
        trigger.name = inputs[0];

        //get type of sign
        Storylet.NumberTriggerType sign = new Storylet.NumberTriggerType();
        if (!tryFindSign(inputs[1], ref sign)){ error = "Sign not recognized, skipping"; return 2; }
        trigger.triggerType = sign;

        // Get the value.
        int inputValue;
        if (!int.TryParse(inputs[2], out inputValue)){error = "Int unable to be parsed, skipping"; return 2; }
        trigger.value = inputValue;

        error = "none";
        return 0;
    }

    /// <summary>
    /// Tries to parse the trigger Floats from a TSV.
    /// </summary>
    /// <param name="inputs">A 3 item string array.</param>
    /// <param name="trigger">The outputed trigger.</param>
    /// <param name="error">The outputed error log.</param>
    /// <returns>0 if correctly parsed. 1 if skipped (no error). 2 if format error.</returns>
    private int tryParseTriggerFloat(string[] inputs, out Storylet.TriggerValue trigger, out string error)
    {

        Storylet.TriggerValue newTrigger = new Storylet.TriggerValue();
        trigger = newTrigger;

        //Set the name.
        if (inputs[0] == "") { error = "No name of world stat, skipped."; return 1; }
        trigger.name = inputs[0];

        //get type of sign
        Storylet.NumberTriggerType sign = new Storylet.NumberTriggerType();
        if (!tryFindSign(inputs[1], ref sign)) { error = "Sign not recognized, skipping"; return 2; }
        trigger.triggerType = sign;

        // Get the value.
        float inputValue;
        if (!float.TryParse(inputs[2], out inputValue)) { error = "Float unable to be parsed, skipping"; return 2; }
        trigger.value = inputValue;

        error = "none";
        return 0;
    }

    /// <summary>
    /// Tries to parse the trigger Bools from a TSV.
    /// </summary>
    /// <param name="inputs">A 3 item string array.</param>
    /// <param name="trigger">The outputed trigger.</param>
    /// <param name="error">The outputed error log.</param>
    /// <returns>0 if correctly parsed. 1 if skipped (no error). 2 if format error.</returns>
    private int tryParseTriggerBool(string[] inputs, out Storylet.TriggerState trigger, out string error)
	{
        Storylet.TriggerState newTrigger = new Storylet.TriggerState();
        trigger = newTrigger;

        //Set the name.
        if (inputs[0] == "") { error = "No name of world stat, skipped."; return 1; }
        trigger.name = inputs[0];

        // Get the value.
        bool inputValue;
        if (!bool.TryParse(inputs[1].ToLower(), out inputValue)) { error = "Bool unable to be parsed, skipping"; return 2;}
        trigger.state = inputValue;

        // Return it.
        error = "none";
        return 0;
    }

    private int tryParseIntChange(string[] inputs, out Storylet.IntChange change, out string error)
	{
        Storylet.IntChange newChange = new Storylet.IntChange();
        change = newChange;

        // Set the name.
        if (inputs[0] == "") { error = "No name of world stat, skipped."; return 1; }
        change.name = inputs[0];

        // get the change amount
        int inputValue;
        if (!int.TryParse(inputs[1], out inputValue)) { error = "Int unable to be parsed, skipping"; return 2; }
        change.value = inputValue;

		// Set or noSet? 
		switch (inputs[2])
		{
            case "SET":
                change.set = true; break;
            case "NOSET":
                change.set = false; break;
            default:
                change.set = false;
                error = "SET NOSET unable to be parsed, skipping"; return 0;
        }

        error = "none";
        return 0;
    }

    private int tryParseFloatChange(string[] inputs, out Storylet.ValueChange change, out string error)
    {
        Storylet.ValueChange newChange = new Storylet.ValueChange();
        change = newChange;

        // Set the name.
        if (inputs[0] == "") { error = "No name of world stat, skipped."; return 1; }
        change.name = inputs[0];

        // get the change amount
        float inputValue;
        if (!float.TryParse(inputs[1], out inputValue)) { error = "Float unable to be parsed, skipping"; return 2; }
        change.value = inputValue;

        // Set or noSet? 
        switch (inputs[2])
        {
            case "SET":
                change.set = true; break;
            case "NOSET":
                change.set = false; break;
            default:
                change.set = false;
                error = "SET NOSET unable to be parsed, skipping"; return 0;
        }

        error = "none";
        return 0;
    }

    private int tryParseBoolChange(string[] inputs, out Storylet.StateChange change, out string error)
    {
        Storylet.StateChange newChange = new Storylet.StateChange();
        change = newChange;

        // Set the name.
        if (inputs[0] == "") { error = "No name of world stat, skipped."; return 1; }
        change.name = inputs[0];

        // Get the value.
        bool inputValue;
        if (!bool.TryParse(inputs[1].ToLower(), out inputValue)) { error = "Bool unable to be parsed, skipping"; return 2; }
        change.state = inputValue;


        error = "none";
        return 0;
    }

    /*
    private bool tryFindCharacterSheet(string name, out CharacterSheet foundCharacter)
	{
        foundCharacter = null;
        foreach (CharacterSheet character in allCharacters)
		{
            if(character.name == name)
			{
                foundCharacter = character;
                return true;
			}
		}

        return false;
	}*/
}