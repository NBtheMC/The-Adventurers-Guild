using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestBanner : MonoBehaviour
{

    [HideInInspector] public QuestSheet questSheet;
    [HideInInspector] public ItemDisplayManager displayManager;

    [SerializeField] private Slider timerObject;
    [SerializeField] private Text questStatus;

    [HideInInspector] public bool isDisplayed;
    // Start is called before the first frame update
    public void Awake()
    {
        displayManager = GameObject.Find("CurrentItemDisplay").GetComponent<ItemDisplayManager>();
        
        isDisplayed = false;
        GameObject.Find("QuestingManager").GetComponent<QuestingManager>().QuestFinished += DeleteBanner;
        
    }
    public void Start()
    {
        UpdateTimer();
    }

    public void UpdateTimer()
    {
        switch (questSheet.currentState)
        {
            case QuestState.WAITING:
                questStatus.gameObject.SetActive(false);
                if(questSheet.timeToExpire > 0)
                {
                    timerObject.gameObject.SetActive(true);
                    timerObject.value = questSheet.expirationTimer / questSheet.timeToExpire;
                }
                else
                {
                    timerObject.gameObject.SetActive(false);
                }   
                break;
            case QuestState.ADVENTURING:
                questStatus.gameObject.SetActive(false);
                timerObject.gameObject.SetActive(true);
                if(questSheet.timeUntilProgression > 0)
                    timerObject.value = questSheet.eventTicksElapsed / questSheet.totalTimeToComplete;
                else
                    timerObject.value = 0;
                break;
            case QuestState.COMPLETED:
                questStatus.gameObject.SetActive(true);
                timerObject.gameObject.SetActive(false);
                questStatus.text = "COMPLETE";
                break;
            default:
                questStatus.gameObject.SetActive(true);
                timerObject.gameObject.SetActive(false);
                questStatus.text = "REJECTED";
                break;
        }
    }

    public void DeleteBanner(object source, QuestSheet questSheet)
    {
        if (questSheet == this.questSheet && this != null)
            Destroy(this.gameObject);
    }

    public void displayQuestUI(bool displayOnly = false)
    {
        if (PauseMenu.gamePaused)
            return;

        displayManager.DisplayQuest(questSheet, this.gameObject);
    }


}
