using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDisplayManager : MonoBehaviour
{
    public GameObject questUIPrefab;
    public DropHandler dropHandler;
    public GameObject QuestDisplay;
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("QuestingManager").GetComponent<QuestingManager>().QuestAdded += GenerateQuestDisplayUI;
    }

    public void GenerateQuestDisplayUI(object source, QuestSheet quest)
    {
        //create questUI object and attach it to this questsheet
        Debug.Log("Making UI Element");
        GameObject newQuest = Instantiate(questUIPrefab); //add position to this later

        //add quest to QuestDisplay canvas
        newQuest.transform.parent = QuestDisplay.transform;
        newQuest.transform.localPosition = new Vector3(-100, 100, 0);

        //move to top of child object hierarchy for rendering order reasons
        newQuest.transform.SetAsFirstSibling();

        //pass quest info to quest UI
        QuestUI questUI = newQuest.GetComponent<QuestUI>();
        questUI.SetupQuestUI(quest);

        //add drop points from quest UI to DropHandler
        Transform party = newQuest.transform.Find("Canvas/Party");
        foreach (Transform child in party)
        {
            dropHandler.AddDropPoint(child.gameObject.GetComponent<ObjectDropPoint>());
        }

        Debug.Log("Done making UI Element");
    }
}
