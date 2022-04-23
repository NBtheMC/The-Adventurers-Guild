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
    private bool questIsActive = false;

    // Start is called before the first frame update
    public void Awake()
    {
        questUIPrefab = Resources.Load<GameObject>("QuestUI");
        displayManager = GameObject.Find("CurrentItemDisplay").GetComponent<ItemDisplayManager>();
        QuestUISpawn = displayManager.questDisplay;
        isDisplayed = false;
        GameObject.Find("QuestingManager").GetComponent<QuestingManager>().QuestFinished += DeleteBanner;
    }

    public void DeleteBanner(object source, QuestSheet questSheet)
    {
        if (questSheet == this.questSheet)
            Destroy(this.gameObject);
    }

    public void ToggleQuestActiveState()
    {
        questIsActive = !questIsActive;
        transform.Find("Active").gameObject.SetActive(questIsActive);
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
            if(questIsActive)
            {
                questUIObj.transform.Find("Send Party").gameObject.SetActive(false);
                questUIObj.transform.Find("Reject").gameObject.SetActive(false);
                GameObject dropPoints = questUIObj.transform.Find("Drop Points").gameObject;

                int index = 0;
                foreach(CharacterSheet character in questSheet.PartyMembers)
                {
                    dropPoints.transform.GetChild(index).Find("Portrait").GetComponent<Image>().sprite = character.portrait;
                    dropPoints.transform.GetChild(index).Find("Portrait").gameObject.SetActive(true);
                    index++;
                }
            }

            questUIObj.GetComponent<QuestUI>().questBanner = this.gameObject;

            //add quest to QuestDisplay canvas
            questUIObj.transform.SetParent(QuestUISpawn.transform, false);

            //pass quest info to quest UI
            QuestUI questUI = questUIObj.GetComponent<QuestUI>();
            questUI.SetupQuestUI(questSheet, questIsActive);
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
