using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestReturnUI : MonoBehaviour
{
    public void GenerateQuestReturnBox(QuestSheet questSheet)
    {
        GameObject prefab = Resources.Load<GameObject>("QuestReturnBox");
        GameObject returnBox = Instantiate(prefab);
        returnBox.transform.Find("Canvas").Find("OkButton").GetComponent<Button>().onClick.AddListener(delegate { DeleteReturnBox(returnBox); });

        Text returnBoxText = returnBox.transform.Find("Canvas").Find("Text").GetComponent<Text>();

        for(int i = 0; i < questSheet.visitedNodes.Count; i++)
        {
            returnBoxText.text += i + ") Node Type: " + questSheet.visitedNodes[i].ToString() + "\n";
        }
        returnBoxText.text += "Gold: " + questSheet.accumutatedGold;
    }

    public void DeleteReturnBox(GameObject gameObject)
    {
        Destroy(gameObject);
    }
}
