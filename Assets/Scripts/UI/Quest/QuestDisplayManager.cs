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
    private int currentDay;
    private int pageNumber;
    private bool currentQuestsDisplayed;

    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject prevButton;
    [SerializeField] private GameObject textInput;
    [SerializeField] private GameObject toggleViewButton;
    // Start is called before the first frame update
    void Start()
    {
        questingManager = GameObject.Find("QuestingManager").GetComponent<QuestingManager>();
        timeSystem = GameObject.Find("TimeSystem").GetComponent<TimeSystem>();
        questListContent = GameObject.Find("QuestDisplayManager/QuestDisplay/QuestList/QuestListViewport/ListContent");
        questBannerPrefab = Resources.Load<GameObject>("QuestBanner");
        input = transform.Find("QuestDisplay/QuestList/PageNumberInput").gameObject.GetComponent<InputField>();

        questingManager.QuestAdded += AddNewQuest;
        questingManager.QuestFinished += AddFinishedQuest;
        timeSystem.NewDay += UpdateCurrentDayPage;

        pageNumber = 0;
        currentDay = 0;
        currentQuestsDisplayed = true;
        input.text = "1";
    }

    public void UpdateCurrentDayPage(object o, GameTime gameTime)
    {
        currentDay = timeSystem.getTime().day;
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
        if (pageNumber == currentDay && !currentQuestsDisplayed)
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
        foreach (Transform child in questListContent.transform)
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
        if (pageNumber > 1)
        {
            pageNumber--;
            input.text = pageNumber.ToString();
        }
        DisplayQuests();
    }

    public void NextPage()
    {
        int currentDay = timeSystem.getTime().day;
        if (pageNumber < currentDay)
        {
            pageNumber++;
            input.text = pageNumber.ToString();
        }
        DisplayQuests();
    }

    public void JumpToPage(string page)
    {
        int num = int.Parse(page);
        int currentDay = timeSystem.getTime().day;

        pageNumber = Mathf.Clamp(num, 0, currentDay);
        input.text = pageNumber.ToString();
        DisplayQuests();
    }

    public void ToggleListView()
    {
        //if showing active/banked quests, toggle to show quest log
        if (currentQuestsDisplayed)
        {
            prevButton.gameObject.SetActive(true);
            nextButton.gameObject.SetActive(true);
            textInput.gameObject.SetActive(true);
            int currentDay = timeSystem.getTime().day;
            currentQuestsDisplayed = false;
            JumpToPage(currentDay.ToString());

            toggleViewButton.transform.GetChild(0).GetComponent<Text>().text = "Active Quests";
        }
        //otherwise show active/banked quests
        else
        {
            prevButton.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(false);
            textInput.gameObject.SetActive(false);

            currentQuestsDisplayed = true;
            DisplayQuests();

            toggleViewButton.transform.GetChild(0).GetComponent<Text>().text = "Quest Log";
        }
    }
}
