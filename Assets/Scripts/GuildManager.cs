using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuildManager : MonoBehaviour
{
    public QuestingManager questingManager;
    public CharacterSheetManager characterManager;
    // Start is called before the first frame update

    private void Awake()
    {
        questingManager = new QuestingManager();
        characterManager = new CharacterSheetManager();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
