using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBanner : MonoBehaviour
{

    [HideInInspector] public QuestSheet questSheet;
    private GameObject QuestDisplay;
    private GameObject questUIPrefab; // QuestUI prefab to display
    [HideInInspector] public bool isDisplayed;


    // Start is called before the first frame update
    public void Awake()
    {
        QuestDisplay = GameObject.Find("QuestDisplay");
        questUIPrefab = Resources.Load<GameObject>("QuestUI");
        //GameObject.Find("QuestingManager").GetComponent<QuestingManager>().QuestStarted += RemoveQuestBanner;
        isDisplayed = false;
    }

    public void displayQuestUI()
    {
        if (!isDisplayed)
        {
            GameObject questUIObj = Instantiate(questUIPrefab);
            questUIObj.GetComponent<QuestUI>().questBanner = this.gameObject;

            //add quest to QuestDisplay canvas
            questUIObj.transform.SetParent(QuestDisplay.transform, false);
            questUIObj.transform.localPosition = new Vector3(-230, 90, 0);

            //move to bottom of child object hierarchy for rendering order reasons
            questUIObj.transform.SetAsLastSibling();

            //pass quest info to quest UI
            QuestUI questUI = questUIObj.GetComponent<QuestUI>();
            questUI.SetupQuestUI(questSheet);
            isDisplayed = true;
        }
    }
}
