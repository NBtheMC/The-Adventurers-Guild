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
    private GameObject partyFormation;
    public GameObject dropPointPrefab;

    // Start is called before the first frame update
    void Start()
    {
        attachedObject = this.transform;
        questName = attachedObject.Find("Canvas/Title").gameObject.GetComponent<Text>();
        questDescription = attachedObject.Find("Canvas/Description").gameObject.GetComponent<Text>();
        questReward = attachedObject.Find("Canvas/Description").gameObject.GetComponent<Text>();
        Canvas canv = attachedObject.Find("Canvas").gameObject.GetComponent<Canvas>();
        canv.worldCamera = Camera.main;
        partyFormation = attachedObject.Find("Canvas/Party").gameObject;
        partyFormation.SetActive(false);
    }

    //Creates Quest as a UI GameObject
    
    public void SetupQuestUI(QuestSheet questSheet){
        attachedSheet = questSheet;
        //Setup name

        //setup description

        //setup first event

        //setup reward

        //setup party formation drop points
        //idk why but partyFormation is null whenever this function is called from another script, so i have to set it again
        partyFormation = this.transform.Find("Canvas/Party").gameObject;
        Rect rect = partyFormation.GetComponent<RectTransform>().rect;
        float dropPointOffset = rect.width / (questSheet.partySize + 1);

        for(int i = 1; i <= questSheet.partySize; i++)
        {
            //make the drop point, set to be a child of Party object
            GameObject dropPoint = Instantiate(dropPointPrefab);
            dropPoint.transform.SetParent(partyFormation.transform, false);

            //set anchor points
            RectTransform dropPointRT = dropPoint.GetComponent<RectTransform>();
            dropPointRT.anchorMin = new Vector2(0, 0.5f);
            dropPointRT.anchorMax = new Vector2(0, 0.5f);
            dropPointRT.pivot = new Vector2(0.5f, 0.5f);

            //set position
            dropPoint.transform.localPosition = new Vector3(150 - dropPointOffset * i, 0, 0);
        }
    }


    //To be used by QuestSheet in order to update with new quests
    public void UpdateQuestUI(QuestSheet.EventInfo newEvent){
        //Add on new group

        //Add description, stat, and dc

        return;
    }

    /// <summary>
    /// Toggles visibility of the party formation box on a quest card
    /// </summary>
    public void TogglePartyFormationVisibility()
    {
        //hide any characters that are held by  a drop point on the quest card
        foreach(Transform child in partyFormation.transform)
        {
            DraggerController character = child.GetComponent<ObjectDropPoint>().heldObject;
            if(character)
                character.gameObject.SetActive(!character.gameObject.activeSelf);
        }
        //hide party formation section and drop points
        partyFormation.SetActive(!partyFormation.activeSelf);
    }

    
}
