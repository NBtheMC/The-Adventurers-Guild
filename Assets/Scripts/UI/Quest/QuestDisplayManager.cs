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
    private InputField input;
    private Text PageCountText;
    private int currentDay;
    public int pageNumber;
    public bool currentQuestsDisplayed;
    // Start is called before the first frame update
    void Start()
    {
        questingManager = GameObject.Find("QuestingManager").GetComponent<QuestingManager>();
        timeSystem = GameObject.Find("TimeSystem").GetComponent<TimeSystem>();
        questListContent = GameObject.Find("QuestDisplayManager/QuestDisplay/QuestList/QuestListViewport/ListContent");
        questBannerPrefab = Resources.Load<GameObject>("QuestBanner");
        input = transform.Find("QuestDisplay/QuestList/PageNumberInput").gameObject.GetComponent<InputField>();
        PageCountText = transform.Find("QuestDisplay/QuestList/PageCount/Text").gameObject.GetComponent<Text>();

        questingManager.QuestAdded += AddNewQuest;
        questingManager.QuestFinished += AddFinishedQuest;
        timeSystem.NewDay += UpdateCurrentDayPage;

        pageNumber = 0;
        currentDay = 0;
        currentQuestsDisplayed = true;
        input.text = "1";
        PageCountText.text = "/ 1";
    }

    public void UpdateCurrentDayPage(object o, GameTime gameTime)
    {
        currentDay = timeSystem.getTime().day;
        PageCountText.text = "/ " + (currentDay + 1);
        if (currentQuestsDisplayed)
        {
            pageNumber = currentDay;
            input.text = 1 + pageNumber + "";
        }
    }

    public void AddNewQuest(object o, QuestSheet quest)
    {
        if (currentQuestsDisplayed)
            GenerateQuestDisplayUI(quest);
    }

    public void AddFinishedQuest(object o, QuestSheet quest)
    {
        if(pageNumber == currentDay && !currentQuestsDisplayed)
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

        input.text = pageNumber.ToString();
    }

    public void NextPage()
    {
        //if not showing current day
        if (!currentQuestsDisplayed)
        {
            if(pageNumber != currentDay)
            {
                pageNumber++;
                input.text = pageNumber.ToString();
            }
            else
            {
                currentQuestsDisplayed = true;
                input.text = currentDay + 1 + "";
            }
            DisplayQuests();
        }              
    }

    public void JumpToPage(string page)
    {
        int num = int.Parse(page);
        int currentDay = timeSystem.getTime().day;
        if (num > currentDay)
        {
            currentQuestsDisplayed = true;
            pageNumber = currentDay;
            input.text = currentDay + 1 + "";
        }
        else
        {
            currentQuestsDisplayed = false;
            pageNumber = Mathf.Clamp(num, 0, currentDay);
            input.text = pageNumber.ToString();   
        }
        DisplayQuests();
    }
}
