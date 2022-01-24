using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestReturnUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateQuestReturnBox()
    {
        Debug.Log("Here");
        GameObject go = Resources.Load<GameObject>("QuestReturnBox");
        Instantiate(go);
    }
}
