using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Takes in new quests and creates them on screen as UI objects
public class QuestUI : MonoBehaviour
{
    private QuestSheet attachedSheet;
    private Transform attachedObject;
    private Text questName;
    private Text questDescription;
    private Text questReward;

    // Start is called before the first frame update
    void Start()
    {
        attachedObject = this.transform;
        questName = attachedObject.Find("Canvas/FormPartyButton/FormPartyText").gameObject.GetComponent<Text>();
    }

    //Creates Quest as a UI GameObject
    
    public void SetupQuestUI(QuestSheet questSheet){
        attachedSheet = questSheet;
        //Setup name

        //setup description

        //setup first event

        //setup reward
    }

    //To be used by QuestSheet in order to update with new quests
    public void UpdateQuestUI(QuestSheet.EventInfo newEvent){
        //Add on new group

        //Add description, stat, and dc

        return;
    }
}
