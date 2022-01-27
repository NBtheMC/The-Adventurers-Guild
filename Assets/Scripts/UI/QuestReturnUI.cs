using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestReturnUI : MonoBehaviour
{
    public void GenerateQuestReturnBox()
    {
        GameObject prefab = Resources.Load<GameObject>("QuestReturnBox");
        GameObject returnBox = Instantiate(prefab);
        returnBox.transform.Find("Canvas").Find("OkButton").GetComponent<Button>().onClick.AddListener(delegate { DeleteReturnBox(returnBox); });
    }

    public void DeleteReturnBox(GameObject gameObject)
    {
        Destroy(gameObject);
    }
}
