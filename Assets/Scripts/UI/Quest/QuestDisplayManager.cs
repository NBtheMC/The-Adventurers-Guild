using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class QuestDisplayManager : MonoBehaviour
{
    public TimeSystem timeSystem;
    private GameObject questBannerPrefab;
    private GameObject questListContent;
    private QuestingManager questingManager;
    private Text pageNumberText;
    public int pageNumber;
    public bool currentQuestsDisplayed;
    // Start is called before the first frame update
    void Start()
    {
        pageNumber = 0;
        currentQuestsDisplayed = true;
        questingManager = GameObject.Find("QuestingManager").GetComponent<QuestingManager>();
        timeSystem = GameObject.Find("TimeSystem").GetComponent<TimeSystem>();
        questListContent = GameObject.Find("QuestDisplayManager/QuestDisplay/QuestList/QuestListViewport/ListContent");
        questBannerPrefab = Resources.Load<GameObject>("QuestBanner");
        pageNumberText = transform.Find("QuestDisplay/QuestList/PageBackground/CurrentPage").gameObject.GetComponent<Text>();

        questingManager.QuestAdded += AddNewQuest;
        questingManager.QuestFinished += AddFinishedQuest;
        timeSystem.NewDay += UpdateCurrentDayPage;
    }

    public void UpdateCurrentDayPage(object o, EventArgs e)
    {
        if (currentQuestsDisplayed)
        {
            pageNumber = timeSystem.getTime().day;
            pageNumberText.text = pageNumber + "";
        }
    }

    public void AddNewQuest(object o, QuestSheet quest)
    {
        if (currentQuestsDisplayed)
            GenerateQuestDisplayUI(quest);
    }

    public void AddFinishedQuest(object o, QuestSheet quest)
    {
        if(pageNumber == timeSystem.getTime().day && !currentQuestsDisplayed)
        {
            GenerateQuestDisplayUI(quest);
        }
    }

    private void GenerateQuestDisplayUI(QuestSheet quest)
    {
        GameObject newQuest = Instantiate(questBannerPrefab);
        //add questBanner obj to questListContent
        newQuest.transform.SetParent(questListContent.transform, false);
        //pass questSheet to questBanner
        newQuest.GetComponent<QuestBanner>().questSheet = quest;
        //set banner text
        newQuest.transform.GetChild(0).gameObject.GetComponent<Text>().text = quest.questName;
    }

    private void ClearQuestList()
    {
        foreach(Transform child in questListContent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void DisplayQuests() 
    {
        ClearQuestList();
        List<QuestSheet> questsToDisplay;

        if (currentQuestsDisplayed)
        {
            questsToDisplay = questingManager.activeQuests.Concat(questingManager.bankedQuests).ToList();
        }
        else
            questsToDisplay = questingManager.finishedQuests[pageNumber];

        foreach (QuestSheet quest in questsToDisplay)
        {
            GenerateQuestDisplayUI(quest);
        }
    }

    public void PrevPage() 
    {
        int currentDay = timeSystem.getTime().day;
        //if not showing current day
        if (currentQuestsDisplayed)
        {
            currentQuestsDisplayed = false;
        }
        else if(pageNumber != 0)
        {
            pageNumber--;
        }
        DisplayQuests();

        pageNumberText.text = pageNumber + "";
    }

    public void NextPage()
    {
        int currentDay = timeSystem.getTime().day;
        //if not showing current day
        if (!currentQuestsDisplayed)
        {
            if(pageNumber != currentDay)
            {
                pageNumber++;
                pageNumberText.text = pageNumber + "";
            }
            else
            {
                currentQuestsDisplayed = true;
                pageNumberText.text = "Current";
            }
            DisplayQuests();
        }              
    }

    public void JumpToPage(string page)
    {
        int num = int.Parse(page);
        print(num);
        Mathf.Clamp(num, 0, timeSystem.getTime().day);
        pageNumber = num;
        pageNumberText.text = pageNumber + "";
        DisplayQuests();
    }
}
