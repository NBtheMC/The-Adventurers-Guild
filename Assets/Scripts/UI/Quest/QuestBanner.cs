using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBanner : MonoBehaviour
{

    public QuestSheet questSheet;
    public DropHandler dropHandler;
    public GameObject QuestDisplay;

    public GameObject questUIPrefab; // QuestUI prefab to display


    // Start is called before the first frame update
    public void Start()
    {
        dropHandler = GameObject.Find("DropHandler").GetComponent<DropHandler>();
        QuestDisplay = GameObject.Find("QuestDisplay");
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
        questUI.questBanner = this.gameObject;

        //add drop points from quest UI to DropHandler
        Transform party = questUIObj.transform.Find("Canvas/Party");
        foreach (Transform child in party)
        {
            dropHandler.AddDropPoint(child.gameObject.GetComponent<ObjectDropPoint>());
        }
    }
}
