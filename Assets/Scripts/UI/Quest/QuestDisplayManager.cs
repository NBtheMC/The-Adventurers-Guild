using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestDisplayManager : MonoBehaviour
{
    private GameObject questBannerPrefab;
    private GameObject questListContent;

    private GameObject QuestDisplay; // The canvas that the quest objects are put onto.
    private GameObject QuestUISpawn; // where the new UI objects should spawn.
    private GameObject questUIPrefab; // The prefab we use to spawn things in.

    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("QuestingManager").GetComponent<QuestingManager>().QuestAdded += GenerateQuestDisplayUI;
        //questListContent = GameObject.Find("QuestDisplayManager/QuestDisplay/QuestList/QuestListViewport/ListContent");
        //questBannerPrefab = Resources.Load<GameObject>("QuestBanner");
        QuestDisplay = GameObject.Find("QuestDisplay"); // Get the correct Quest Display object.
        questUIPrefab = Resources.Load<GameObject>("QuestUI"); // Load in the script for QuestUI.
        QuestUISpawn = GameObject.Find("QuestUISpawn");
    }

    public void GenerateQuestDisplayUI(object source, QuestSheet quest)
    {
        Debug.Log("Making Quest Banner");

        // Make a new Quest UI object.
        GameObject questUIObj = Instantiate(questUIPrefab);

        //add quest to QuestDisplay canvas
        questUIObj.transform.SetParent(QuestDisplay.transform, false);
        questUIObj.GetComponent<RectTransform>().anchoredPosition = QuestUISpawn.transform.localPosition;

        //move to bottom of child object hierarchy for rendering order reasons
        questUIObj.transform.SetAsLastSibling();

        //pass quest info to quest UI
        QuestUI questUI = questUIObj.GetComponent<QuestUI>();
        questUI.SetupQuestUI(quest, false);
        Debug.Log(quest.questName);

        var i = UnityEngine.Random.Range(0, 3);
        if (i == 0) { SoundManagerScript.PlaySound("parchment1"); }
        else if (i == 1) { SoundManagerScript.PlaySound("parchment2"); }
        else { SoundManagerScript.PlaySound("parchment3"); }

        // Commented out to be the old quest generation. Now we just have all the quests plop on the table.
        /*
        GameObject newQuest = Instantiate(questBannerPrefab);
        //add questBanner obj to questListContent
        newQuest.transform.SetParent(questListContent.transform, false);
        //pass questSheet to questBanner
        newQuest.GetComponent<QuestBanner>().questSheet = quest;
        //set banner text
        newQuest.transform.GetChild(0).gameObject.GetComponent<Text>().text = quest.questName;
        */

        Debug.Log("Done making Quest Banner");
    }
}
