using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestReturnUI : MonoBehaviour
{

    private void Start()
    {
        GameObject.Find("QuestingManager").GetComponent<QuestingManager>().QuestFinished += GenerateQuestReturnBox;
            // GameObject.Find("GuildManager").GetComponent<GuildManager>.questingManager.QuestFinished += GenerateQuestReturnBox
    }
    public void GenerateQuestReturnBox(object source, QuestSheet questSheet)
    {
        //questSheet.isActive = false;
        //questSheet.isComplete = true;

        GameObject prefab = Resources.Load<GameObject>("QuestReturnBox");
        GameObject returnBox = Instantiate(prefab);
        returnBox.transform.Find("Canvas").Find("OkButton").GetComponent<Button>().onClick.AddListener(delegate { DeleteReturnBox(returnBox); });

        Text eventText = returnBox.transform.Find("Canvas").Find("EventBG/EventRecap").GetComponent<Text>();

        // for (int i = 0; i < questSheet.visitedNodes.Count; i++)
        // {
        //     eventText.text += i + ") Node Type: " + questSheet.visitedNodes[i].ToString() + "\n";
        // }

        Text adventurerText = returnBox.transform.Find("Canvas").Find("QuestRecapBG/QuestRecap").GetComponent<Text>();
        adventurerText.text = questSheet.GetQuestRecap();

        Text questName = returnBox.transform.Find("Canvas").Find("QuestName").GetComponent<Text>();
        questName.text = questSheet.questName;

        //Text assignedBy = returnBox.transform.Find("Canvas").Find("AssignedBy/QuestGiver").GetComponent<Text>();
        //assignedBy.text = questSheet.questGiver;

        Text rewardAmount = returnBox.transform.Find("Canvas").Find("Reward/Gold").GetComponent<Text>();
        rewardAmount.text = questSheet.totalGold.ToString();

        Image cg = returnBox.transform.Find("Canvas").Find("CG").GetComponent<Image>();
        cg.sprite = questSheet.GetCG();

        SoundManagerScript.PlaySound("bell");
    }

    public void DeleteReturnBox(GameObject gameObject)
    {
        Destroy(gameObject);
    }
}
