using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestDisplayManager : MonoBehaviour
{
    public GameObject questBannerPrefab;
    public DropHandler dropHandler;
    public GameObject QuestDisplay;
    public GameObject questListContent;
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("QuestingManager").GetComponent<QuestingManager>().QuestAdded += GenerateQuestDisplayUI;
    }

    public void GenerateQuestDisplayUI(object source, QuestSheet quest)
    {
        Debug.Log("Making Quest Banner");
        GameObject newQuest = Instantiate(questBannerPrefab);
        //add questBanner obj to questListContent
        newQuest.transform.SetParent(questListContent.transform, false);
        //pass questSheet to questBanner
        newQuest.GetComponent<QuestBanner>().questSheet = quest;
        //set banner text
        newQuest.transform.GetChild(0).gameObject.GetComponent<Text>().text = quest.questName;

        Debug.Log("Done making Quest Banner");
    }
}
