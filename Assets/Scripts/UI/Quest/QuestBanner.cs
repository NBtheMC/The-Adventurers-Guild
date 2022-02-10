using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBanner : MonoBehaviour
{

    [HideInInspector] public QuestSheet questSheet;
    private GameObject QuestDisplay;
    private GameObject questUIPrefab; // QuestUI prefab to display


    // Start is called before the first frame update
    public void Start()
    {
        QuestDisplay = GameObject.Find("QuestDisplay");
        questUIPrefab = Resources.Load<GameObject>("QuestUI");
        GameObject.Find("QuestingManager").GetComponent<QuestingManager>().QuestStarted += RemoveQuestBanner;
    }

    public void displayQuestUI()
    {
        GameObject questUIObj = Instantiate(questUIPrefab);
        //add quest to QuestDisplay canvas
        questUIObj.transform.SetParent(QuestDisplay.transform, false);
        questUIObj.transform.localPosition = new Vector3(-230, 90, 0);

        //move to top of child object hierarchy for rendering order reasons
        questUIObj.transform.SetAsFirstSibling();

        //pass quest info to quest UI
        QuestUI questUI = questUIObj.GetComponent<QuestUI>();
        questUI.SetupQuestUI(questSheet);
    }

    public void RemoveQuestBanner(object source, QuestSheet e)
    {
        Destroy(this.gameObject);
        GameObject.Find("QuestingManager").GetComponent<QuestingManager>().QuestStarted -= RemoveQuestBanner;
    }
}
