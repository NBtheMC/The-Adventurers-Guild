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
        GameObject prefab = Resources.Load<GameObject>("QuestReturnBox");
        GameObject returnBox = Instantiate(prefab);
        returnBox.transform.Find("Canvas").Find("OkButton").GetComponent<Button>().onClick.AddListener(delegate { DeleteReturnBox(returnBox); });

        Text eventText = returnBox.transform.Find("Canvas").Find("EventRecap").GetComponent<Text>();

        // for (int i = 0; i < questSheet.visitedNodes.Count; i++)
        // {
        //     eventText.text += i + ") Node Type: " + questSheet.visitedNodes[i].ToString() + "\n";
        // }

        Text adventurerText = returnBox.transform.Find("Canvas").Find("AdventurerRecap").GetComponent<Text>();
        adventurerText.text = questSheet.GetQuestRecap();
    }

    public void DeleteReturnBox(GameObject gameObject)
    {
        Destroy(gameObject);
    }
}
