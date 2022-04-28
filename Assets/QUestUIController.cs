using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The purpose of this script is to invoke specific methods in QuestUI when certain events are triggered

public class QUestUIController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void QuestTestMethod(object src, CharacterSheet character)
    {
        QuestUI questUI = transform.GetComponentInChildren<QuestUI>();
        if (questUI == null) return;

        questUI.AddCharacter(character);
    }
}
