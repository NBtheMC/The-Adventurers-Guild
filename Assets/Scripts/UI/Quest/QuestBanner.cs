using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestBanner : MonoBehaviour
{

    [HideInInspector] public QuestSheet questSheet;
    [HideInInspector] public ItemDisplayManager displayManager;
    private GameObject QuestUISpawn;
    private GameObject questUIPrefab;
    [HideInInspector] public bool isDisplayed;

    // Start is called before the first frame update
    public void Awake()
    {
        questUIPrefab = Resources.Load<GameObject>("QuestUI");
        displayManager = GameObject.Find("CurrentItemDisplay").GetComponent<ItemDisplayManager>();
        QuestUISpawn = displayManager.questDisplay;
        isDisplayed = false;
        GameObject.Find("QuestingManager").GetComponent<QuestingManager>().QuestFinished += DeleteBanner;
        
    }
    public void Start()
    {
        transform.Find("Active").gameObject.SetActive(questSheet.isActive);
    }

    public void DeleteBanner(object source, QuestSheet questSheet)
    {
        if (questSheet == this.questSheet && this != null)
            Destroy(this.gameObject);
    }

    public void ToggleQuestActiveState()
    {
        transform.Find("Active").gameObject.SetActive(questSheet.isActive);
    }

    public void displayQuestUI(bool displayOnly = false)
    {
        if (PauseMenu.gamePaused)
            return;
        if (!isDisplayed)
        {
            displayManager.DisplayQuest(true);

            //if a different quest is in the display manager, remove it
            if (QuestUISpawn.transform.childCount != 0)
            {
                QuestUISpawn.transform.GetChild(0).GetComponent<QuestUI>().DestroyUI();
            }

            GameObject questUIObj = Instantiate(questUIPrefab);
            
            //if quest is marked as active, deactivate extra buttons
            if(questSheet.isActive || questSheet.isComplete)
            {
                questUIObj.transform.Find("Send Party").gameObject.SetActive(false);
                questUIObj.transform.Find("Reject").gameObject.SetActive(false);
                GameObject dropPoints = questUIObj.transform.Find("Drop Points").gameObject;

                int index = 0;
                //display characters assigned to this quest
                foreach(CharacterSheet character in questSheet.PartyMembers)
                {
                    Transform child = dropPoints.transform.GetChild(index);
                    child.Find("Portrait").GetComponent<Image>().sprite = character.portrait;
                    child.Find("Portrait").gameObject.SetActive(true);
                    child.Find("EmptyCharacter").gameObject.SetActive(false);
                    child.Find("FilledCharacter").gameObject.SetActive(true);
                    child.Find("Name").gameObject.SetActive(true);
                    child.Find("Name").gameObject.GetComponent<Text>().text = character.name;
                    index++;
                }
            }

            questUIObj.GetComponent<QuestUI>().questBanner = this.gameObject;

            //add quest to QuestDisplay canvas
            questUIObj.transform.SetParent(QuestUISpawn.transform, false);

            //pass quest info to quest UI
            QuestUI questUI = questUIObj.GetComponent<QuestUI>();
            questUI.SetupQuestUI(questSheet, questSheet.isActive);
            isDisplayed = true;

            var i = UnityEngine.Random.Range(0, 3);
            if (i == 0) {SoundManagerScript.PlaySound("parchment1");}
            else if (i == 1) {SoundManagerScript.PlaySound("parchment2");}
            else {SoundManagerScript.PlaySound("parchment3");}
        }
        else if (isDisplayed && !displayManager.questDisplay.activeInHierarchy)
        {
            displayManager.DisplayQuest(true);
        }
        else if (isDisplayed && displayManager.questDisplay.activeInHierarchy)
        {
            displayManager.DisplayQuest(false);
        }
    }


}
