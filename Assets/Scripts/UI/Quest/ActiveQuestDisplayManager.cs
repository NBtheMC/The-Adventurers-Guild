using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveQuestDisplayManager : MonoBehaviour
{
    private QuestingManager questingManager;
    private GameObject questBoxesContainer;
    private List<QuestBox> questBoxes;

    [SerializeField]
    private int numQuests = 0;
    private int currentGroup = 0;

    private struct QuestBox
    {
        public GameObject gameObject;
        public Text text;

        public QuestBox(GameObject gameObject)
        {
            this.gameObject = gameObject;
            text = gameObject.GetComponentInChildren<Text>();
        }
    }

    private void Awake()
    {
        questBoxes = new List<QuestBox>();

        questBoxesContainer = transform.Find("Canvas").Find("Background").Find("Quests").gameObject;
        foreach(Transform group in questBoxesContainer.transform)
        {
            foreach(Transform questBox in group)
            {
                questBoxes.Add(new QuestBox(questBox.gameObject));
                //questBox.gameObject.SetActive(false);
            }
        }
        Debug.Log("questBoxes size: " + questBoxes.Count);

        questingManager = GameObject.Find("QuestingManager").GetComponent<QuestingManager>();
    }

    private void Start()
    {
        questingManager.QuestFinished += UpdateQuestBoxes;
        questingManager.QuestStarted += UpdateQuestBoxes;

    }

    private void UpdateQuestBoxes(object src, QuestSheet quest)
    {
        numQuests = questingManager.activeQuests.Count;

        if (numQuests / 4 <= currentGroup)
        {
            StopAllCoroutines();
            questBoxesContainer.transform.localPosition = new Vector3(0, 0);
            currentGroup = 0;
        }

        for(int i = 0; i < questingManager.activeQuests.Count; i++)
        {
            questBoxes[i].gameObject.SetActive(true);
            questBoxes[i].text.text = questingManager.activeQuests[i].questName;
        }

        for(int i = questingManager.activeQuests.Count; i < 12; i++)
        {
            questBoxes[i].gameObject.SetActive(false);
        }
    }

    public void NextGroup()
    {
        if (currentGroup != 2 && (numQuests / 4) > currentGroup)
        {
            StartCoroutine(AnimateQuestBox(-400));
            //questBoxesContainer.transform.position += new Vector3(-400, 0, 0);
            currentGroup++;
        }
    }

    public void PrevGroup()
    {
        if (currentGroup != 0)
        {
            StartCoroutine(AnimateQuestBox(400));
            //questBoxesContainer.transform.position += new Vector3(400, 0, 0);
            currentGroup--;
        }
    }

    private IEnumerator AnimateQuestBox(int xMove)
    {
        if (xMove < 0)
        {
            for(int i = 0; i > xMove/2; i--)
            {
                int temp = xMove / 400;
                yield return new WaitForSeconds(0.001f);
                questBoxesContainer.transform.position += new Vector3(temp*2, 0, 0);
            }
        }
        else
        {
            for (int i = 0; i < xMove/2; i++)
            {
                int temp = xMove / 400;
                yield return new WaitForSeconds(0.001f);
                questBoxesContainer.transform.position += new Vector3(temp*2, 0, 0);
            }
        }
        yield return null;
    }
}
