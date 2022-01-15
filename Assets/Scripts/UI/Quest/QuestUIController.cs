using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Takes in new quests and creates them on screen as UI objects
public class QuestController : MonoBehaviour
{
    private GameObject questPrefab;
    private QuestGenerator questGenerator;

    // Start is called before the first frame update
    void Start()
    {
        questPrefab = Resources.Load("Assets/Scripts/UI/Quest/QuestUI.prefab") as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
