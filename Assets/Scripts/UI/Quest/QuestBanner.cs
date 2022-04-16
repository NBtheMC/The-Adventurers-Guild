using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestBanner : MonoBehaviour
{

    [HideInInspector] public QuestSheet questSheet;
    private GameObject QuestDisplay;
    private GameObject QuestUISpawn;
    private GameObject questUIPrefab; // QuestUI prefab to display
    [HideInInspector] public bool isDisplayed;
    private bool questIsActive = false;


    // Start is called before the first frame update
    public void Awake()
    {
        QuestDisplay = GameObject.Find("QuestDisplay");
        //questUIPrefab = Resources.Load<GameObject>("QuestUI");
        questUIPrefab = Resources.Load<GameObject>("QuestUI_V2");
        QuestUISpawn = GameObject.Find("QuestUISpawn");
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

    public void displayQuestUI()
    {   
        if (!isDisplayed)
        {
            GameObject questUIObj = Instantiate(questUIPrefab);
            
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
            questUIObj.transform.SetParent(QuestDisplay.transform, false);
            questUIObj.GetComponent<RectTransform>().anchoredPosition = QuestUISpawn.transform.localPosition;

            //move to bottom of child object hierarchy for rendering order reasons
            questUIObj.transform.SetAsLastSibling();

            //pass quest info to quest UI
            QuestUI questUI = questUIObj.GetComponent<QuestUI>();
            questUI.SetupQuestUI(questSheet, questIsActive);
            Debug.Log(questSheet.questName);
            isDisplayed = true;

            var i = UnityEngine.Random.Range(0, 3);
            if (i == 0) {SoundManagerScript.PlaySound("parchment1");}
            else if (i == 1) {SoundManagerScript.PlaySound("parchment2");}
            else {SoundManagerScript.PlaySound("parchment3");}
        }
    }


}
